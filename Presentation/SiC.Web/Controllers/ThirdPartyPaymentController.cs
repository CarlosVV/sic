namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using CDI.WebApplication.DataTables.Result;
    using Domain.Core.Model;
    using Infrastructure.Core.Helpers;
    using Infrastructure.Web.Controllers;
    using Infrastructure.Web.Utilities;
    using Models.ThirdPartyPayment;
    using System.Collections.Generic;

    #endregion
    public class ThirdPartyPaymentController : BaseController
    {
        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            model.Courts = EntityService.GetAllCourts().ToSelectList(court => court.FullName, court => new JavaScriptSerializer().Serialize(new
            {
                CourtId = court.EntityId,
                CourtName = court.FullName,
                AddressLine1 = court.Addresses.FirstOrDefault() != null ? court.Addresses.FirstOrDefault().Line1 : string.Empty,
                AddressLine2 = court.Addresses.FirstOrDefault() != null ? court.Addresses.FirstOrDefault().Line2 : string.Empty,
                City = string.Empty,
                Region = string.Empty,
                ZipCode = string.Empty,
                ZipCodeExt = string.Empty
            }), null, "Seleccionar");
            model.Courts.Add(new SelectListItem
            {
                Text = "Otro",
                Value = "-1"
            });
            model.Cities = LocationService.GetAllCities().ToSelectList(c => c.City1, c => c.CityId.ToString(), null, "Seleccionar");

            return View(model);
        }

        [HttpPost]
        public JsonResult Insert(ThirdPartyPaymentModel model)
        {
            int? newEntityId = null;
            var entity = new Entity();
            var address = new Address();

            if (model.EntityTypeId == 1)
            {
                entity = new Entity
                {
                    SourceId = 8,
                    FirstName = model.CustodyFirstName,
                    MiddleName = model.CustodySecondName,
                    LastName = model.CustodyFirstLastName,
                    SecondLastName = model.CustodySecondLastName,
                    FullName = string.Format("{0} {1} {2} {3}", model.CustodyFirstName, model.CustodySecondName, model.CustodyFirstLastName, model.CustodySecondLastName)
                };

                address = new Address
                {
                    EntityId = entity.EntityId,
                    SourceId = 8,
                    FullAddress = string.Format("{0} {1}", model.CustodyAddressLine1, model.CustodyAddressLine2),
                    Line1 = model.CustodyAddressLine1,
                    Line2 = model.CustodyAddressLine2,
                    CityId = model.CustodyCityId,
                    ZipCode = model.CustodyPostalCode == null ? string.Empty : model.CustodyPostalCode.Trim().Replace('-', '\0').PadRight(9, '0').Substring(0, 5),
                    ZipCodeExt = model.CustodyPostalCode == null ? string.Empty : model.CustodyPostalCode.Trim().Replace('-', '\0').PadRight(9, '0').Substring(5, 4)
                };
                entity.Addresses.Add(address);

                EntityService.CreateEntity(entity);

                newEntityId = entity.EntityId;
            }
            else if (model.EntityTypeId == 2)
            {
                entity = new Entity
                {
                    SourceId = 9,
                    FullName = model.EntityName
                };

                address = new Address
                {
                    SourceId = 9,
                    EntityId = entity.EntityId,
                    FullAddress = string.Format("{0} {1}", model.EntityAddressLine1, model.EntityAddressLine2),
                    Line1 = model.EntityAddressLine1,
                    Line2 = model.EntityAddressLine2,
                    CityId = model.EntityCityId,
                    ZipCode = model.EntityPostalCode == null ? string.Empty : model.EntityPostalCode.Trim().Replace('-', '\0').PadRight(9, '0').Substring(0, 5),
                    ZipCodeExt = model.EntityPostalCode == null ? string.Empty : model.EntityPostalCode.Trim().Replace('-', '\0').PadRight(9, '0').Substring(5, 4)
                };
                entity.Addresses.Add(address);

                EntityService.CreateEntity(entity);

                newEntityId = entity.EntityId;
            }
            else if (model.EntityTypeId == 3)
            {
                newEntityId = model.CourtId;
            }

            decimal amount = decimal.Zero;
            if (model.SinglePaymentAmount.HasValue)
            {
                amount = model.SinglePaymentAmount.Value;
            }
            else
            {
                amount = model.FirstInstallmentAmount.GetValueOrDefault(decimal.Zero) + model.SecondInstallmentAmount.GetValueOrDefault(decimal.Zero);
            }

            var thirdPartySchedule = new ThirdPartySchedule
            {
                CaseId = model.CaseId,
                CaseDetailId = model.CaseDetailId,
                EntityId_RemitTo = model.EntityTypeId == 2 || model.EntityTypeId == 3 ? newEntityId : null,
                ClaimNumber = model.ClaimNumber,
                OrderIdentifier = model.OrderIdentifier,
                TerminationFlag = model.TerminationFlag,
                EffectiveDate = model.EffectiveDate,
                TerminationOrderNumber = model.TerminationOrderNumber,
                SinglePaymentAmount = model.SinglePaymentAmount,
                FirstInstallmentAmount = model.FirstInstallmentAmount,
                SecondInstallmentAmount = model.SecondInstallmentAmount,
                Comment = model.Comment,
                OrderAmount = model.OrderAmount,
                TerminationDate = model.TerminationDate,
                CreatedDateTime = System.DateTime.UtcNow,
                CreatedBy = WebHelper.GetUserName(),
            };

            PaymentService.CreateThirdPayment(thirdPartySchedule);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }
        [HttpPost]
        public JsonResult Update(ThirdPartyPaymentModel model)
        {
            var thirdpayment = PaymentService.FindThirdPartyPaymentById(model.ThirdPartyScheduleId);

            Dictionary<string, object> dicThirdPartySchedule = new Dictionary<string, object>();
            ActivityLogService.CreateDictionaryForThirdParty(ref dicThirdPartySchedule, thirdpayment);

            ActivityLogService.CreateActivityLog(new ActivityLog {
                Comment = XmlHelper.SerializeKeyValuePairs("ThirdPartySchedule", dicThirdPartySchedule),
                ObjectTypeId = (int)AuditObjectType.PaymentThirdPartySchedule,
                ObjectId = thirdpayment.ThirdPartyScheduleId.ToString()
            }, "ThirdPartySchedule.Update");

            thirdpayment.FirstInstallmentAmount = 0;
            thirdpayment.SecondInstallmentAmount = 0;
            thirdpayment.SinglePaymentAmount = 0;

            if (model.IsSinglePayment)
            {
                thirdpayment.SinglePaymentAmount = model.SinglePaymentAmount;
                thirdpayment.OrderAmount = model.SinglePaymentAmount;
            }
            else
            {
                thirdpayment.FirstInstallmentAmount = model.FirstInstallmentAmount;
                thirdpayment.SecondInstallmentAmount = model.SecondInstallmentAmount;
                thirdpayment.OrderAmount = model.FirstInstallmentAmount + model.SecondInstallmentAmount;
            }
            thirdpayment.Comment = model.Comment;
            thirdpayment.ModifiedDateTime = System.DateTime.UtcNow;
            thirdpayment.ModifiedBy = WebHelper.GetUserName();

            PaymentService.ModifyThirdPayment(thirdpayment);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }
        [HttpPost]
        public async Task<ActionResult> FindByCaseId(int caseId)
        {
            var thirdPartySchedules = await PaymentService.FindThirdPartyPaymentsAsync(null, null, caseId);
            var result = thirdPartySchedules.Select(t => new
            {
                NombreEntidad = t.Remitter == null ? string.Empty : t.Remitter.FullName,
                NroOrden = t.ClaimNumber,
                NroParticipante = t.OrderIdentifier,
                MontoOrden = t.OrderAmount.ToCurrency(),
                TerminacionFlag = t.TerminationFlag,
                FechaTerminacion = t.TerminationDate.HasValue ? t.TerminationDate.Value.ToShortDateString() : string.Empty,
                FechaVigencia = t.EffectiveDate.HasValue ? t.EffectiveDate.Value.ToShortDateString() : string.Empty,
                OrdenTerminacion = t.TerminationOrderNumber
            });

            return Json(new BasicDataTablesResult(result));
        }

        [HttpGet]
        [Route("thirdpartypayment/findtoeditbyid/{thirdpartyscheduleid}")]
        public JsonResult FindToEditById(int thirdPartyScheduleId)
        {
            var query = PaymentService.FindThirdPartyPaymentById(thirdPartyScheduleId);
            var result = new EditViewModel
            {
                ThirdPartyScheduleId = query.ThirdPartyScheduleId,
                CaseId = query.CaseId,
                CaseDetailId = query.CaseDetailId,
                CaseNumber = query.CaseDetailId !=null ? query.CaseDetail.CaseNumber : string.Empty,
                Lesionado = query.CaseDetailId != null ? query.CaseDetail.Entity.FullName : string.Empty,
                SSN = query.CaseDetailId != null ? query.CaseDetail.Entity.SSN : string.Empty,
                Custodio = query.Remitter == null ? string.Empty : query.Remitter.FullName,
                ClaimNumber = query.ClaimNumber,
                OrderIdentifier = query.OrderIdentifier,
                SinglePaymentAmount = query.SinglePaymentAmount,
                FirstInstallAmount = query.FirstInstallmentAmount,
                SecondInstallAmount = query.SecondInstallmentAmount,
                EffectiveDate = query.EffectiveDate !=null ? query.EffectiveDate.Value.ToShortDateString() : string.Empty,
                Observaciones = query.Comment
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
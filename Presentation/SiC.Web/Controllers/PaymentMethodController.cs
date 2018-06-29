namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using CDI.WebApplication.DataTables.Result;
    using Infrastructure.Web.Controllers;
    using Infrastructure.Web.Utilities;
    using Infrastructure.Web.ViewModels;

    #endregion
    [Authorize]
    public class PaymentMethodController : BaseController
    {
        #region Private Methods
        
        private IEnumerable<SelectListItem> GetAllCourts()
        {
            var courts = EntityService.GetAllCourts();

            List<SelectListItem> courtItems = new List<SelectListItem>();
            courtItems.Add(new SelectListItem()
            {
                Value = string.Empty,
                Text = "-- Seleccionar --"
            });
            foreach (var court in courts)
            {
                courtItems.Add(new SelectListItem()
                {
                    Value = new JavaScriptSerializer().Serialize(new
                    {
                        CourtId = court.EntityId,
                        CourtName = court.FullName,
                        AddressLine1 = court.Addresses.FirstOrDefault() != null ? court.Addresses.FirstOrDefault().Line1 : string.Empty,
                        AddressLine2 = court.Addresses.FirstOrDefault() != null ? court.Addresses.FirstOrDefault().Line2 : string.Empty,
                        City = string.Empty,
                        Region = string.Empty,
                        ZipCode = string.Empty,
                        ZipCodeExt = string.Empty
                    }),
                    Text = court.FullName
                });
            }
            courtItems.Add(new SelectListItem()
            {
                Value = "-1",
                Text = "Otro"
            });

            return courtItems;
        }
        
        #endregion
        
        private const string decimalFormat = "{0:C2}";

        public ActionResult Index()
        {
            return RedirectToAction("Payments");
        }
        
        public ActionResult EditPayments()
        {
            var modelo = new EditPaymentViewModel();

            modelo.RegionList = LocationService.GetAllRegions().ToSelectList(r => r.Region1, r => r.RegionId.ToString(), null, "Seleccionar");
            modelo.ClinicList = LocationService.GetAllClinics().ToSelectList(c => string.Format("{0} [{1}]", c.Clinic1, c.RegionId), c => c.ClinicId.ToString(), null, "Seleccionar");
            modelo.CityList = LocationService.GetAllCities().ToSelectList(c => string.Format("{0} [{1}]", c.City1, c.StateId), c => c.CityId.ToString(), null, "Seleccionar");
            modelo.CourtList = GetAllCourts().ToList();
            modelo.PaymentStatus = PaymentService.GetAllPaymentStatuses().ToSelectList(p => p.Status1, p => p.StatusId.ToString(), null, "Seleccionar");

            return View(modelo);
        }
        
        [HttpPost]
        public JsonResult GetThirdPartyPartySummary(int? entityId)
        {
            var payments = PaymentService.FindThirdPartyPayments(null, entityId, null);
            var summary = payments.Select(m => new
            {
                EntidadCustodio = m.Remitter == null ? string.Empty : m.Remitter.FullName,
                CasoOrden = m.ClaimNumber,
                ParticipanteCaso = m.OrderIdentifier,
                UnSoloPago = string.Format(decimalFormat, m.SinglePaymentAmount),
                Pago1 = string.Format(decimalFormat, m.FirstInstallmentAmount),
                Pago2 = string.Format(decimalFormat, m.SecondInstallmentAmount)
            });
            return Json(new BasicDataTablesResult(summary));
        }
        
        [HttpPost]
        public JsonResult UpdatePagoTerceros(int thirdpartyscheduleid, decimal? unsolopago, decimal? pago1, decimal? pago2, string observaciones)
        {
            var thirdPartyPayment = PaymentService.FindThirdPartyPaymentById(thirdpartyscheduleid);

            thirdPartyPayment.SinglePaymentAmount = unsolopago;
            thirdPartyPayment.FirstInstallmentAmount = pago1;
            thirdPartyPayment.SecondInstallmentAmount = pago2;
            thirdPartyPayment.Comment = observaciones;

            PaymentService.ModifyThirdPayment(thirdPartyPayment);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpGet]
        [Route("paymentmethod/GetPayment/{paymentid}")]
        public JsonResult GetPayment(int paymentid)
        {
            var payment = PaymentService.FindPaymentById(paymentid);
            var transaction = TransactionService.FindTransactionById(int.Parse(payment.TransactionNum));
            var result = new
            {
                PaymentId = payment.PaymentId,
                Cantidad = payment.Amount,
                AdjudicacionAdicional = transaction.TransactionAmount,
                Mensualidad = transaction.MonthlyInstallment,
                Semanas = transaction.NumberOfWeeks,
                Observaciones = transaction.Comment
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
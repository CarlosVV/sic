namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CDI.WebApplication.DataTables.Result;
    using Infrastructure.Core.Helpers;
    using Infrastructure.Core.Log;
    using Infrastructure.Web.Controllers;
    using Infrastructure.Web.Utilities;
    using Models.AdjustmentEBT;
    using Domain.Core.Model;
    using System.Net.Mail;

    #endregion

    public class AdjustmentEBTController : BaseController
    {
        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            model.AdjustmentStatuses = PaymentService.GetAllAdjustmentStatuses()
                                                     .Where(a => a.AdjustmentStatusId == 1)
                                                     .ToSelectList(a => a.AdjustmentStatus1, a => a.AdjustmentStatusId.ToString(), null, "Seleccionar");
            model.AdjustmentTypes.Add(new SelectListItem
            {
                Text = "Débito",
                Value = "Débito",
            });
            model.AdjustmentTypes.Add(new SelectListItem
            {
                Text = "Crédito",
                Value = "Crédito",
            });
            model.AdjustmentTypes.Add(new SelectListItem
            {
                Text = "N/A",
                Value = "N/A",
            });

            return View(model.Rebuild(model));
        }

        [HttpPost]
        public JsonResult FindRequests(SearchViewModel model)
        {
            var payments = PaymentService.SearchPaymentsAdjustmentEBT(model.CaseNumber, model.EntityName, model.SocialSecurityNumber, model.BirthDate, model.FilingDate, model.RegionId, model.ClinicId, model.EBTNumber).ToList();
            var result = new List<AdjusmentEbtViewModel>();

            foreach (var payment in payments)
            {
                var paymentAjusteEbtModel = new AdjusmentEbtViewModel
                {
                    PaymentId = payment.PaymentId,
                    CaseId = payment.CaseId.Value,
                    CaseDetailId = payment.CaseDetailId.Value,
                    CaseNumber = payment.CaseNumber,
                    CaseKey = payment.CaseKey,
                    AdjustmentStatusId = payment.AdjustmentStatusId,
                    AdjustmentStatus = payment.AdjustmentStatusId.HasValue ? payment.AdjustmentStatus.AdjustmentStatus1 : string.Empty,
                    AdjustmentType = payment.AdjustmentType,
                    AdjustmentRequestedDate = payment.AdjustmentRequestedDate.ToShortDateString(),
                    AdjustmentRequestedBy = payment.AdjustmentRequestedBy,
                    ConceptId = payment.ConceptId,
                    Concept = payment.ConceptId.HasValue ? payment.Concept.Concept1 : string.Empty,
                    TransactionNumber = payment.TransactionNum,
                    IssueDate = payment.IssueDate.ToShortDateString(),
                    FromDate = payment.FromDate.ToShortDateString(),
                    ToDate = payment.ToDate.ToShortDateString(),
                    PaymentDay = payment.PaymentDay.Value,
                    Amount = payment.Amount.ToCurrency()
                };

                result.Add(paymentAjusteEbtModel);
            }

            return Json(new BasicDataTablesResult(result), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRequests(List<SaveRequestModel> requests)
        {
            var injuredPerson = new Dictionary<String, List<AdjusmentEbtViewModel>>();
            DateTime workingDate = DateTime.Now;

            foreach (var request in requests)
            {
                var payment = PaymentService.FindPaymentById(request.PaymentId);
                if (payment == null || payment.CaseDetail == null && payment.CaseDetail.Entity == null)
                    continue;

                payment.AdjustmentStatusId = request.AdjustmentStatusId;
                payment.AdjustmentType = request.AdjustmentType;
                payment.AdjustmentRequestedBy = WebHelper.GetUserName();
                payment.AdjustmentRequestedDate = workingDate;

                var itemEmailEbt = GetTableRowAdjusmentEBT(payment);
                var personFullName = payment.CaseDetail.Entity.FullName;

                if (!injuredPerson.ContainsKey(personFullName))
                {
                    injuredPerson.Add(personFullName, new List<AdjusmentEbtViewModel>());
                }

                injuredPerson[personFullName].Add(itemEmailEbt);
                // DeleteAdjusmentEbtRelated(payment);
                PaymentService.ModifyPayment(payment);
            }

            //TODO: Test Send Email
            SendNotificationEmail(injuredPerson);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpGet]
        public ActionResult Approve()
        {
            var model = new ApproveViewModel();

            model.AdjustmentStatuses = PaymentService.GetAllAdjustmentStatuses()
                                                     .Where(a => a.AdjustmentStatusId == 2 || a.AdjustmentStatusId == 3)
                                                     .ToSelectList(a => a.AdjustmentStatus1, a => a.AdjustmentStatusId.ToString(), null, null);

            return View(model);
        }

        [HttpPost]
        public JsonResult FindRequestsToApprove()
        {
            var payments = PaymentService.FindAdjustmentsEBTToApprove().Where(p => !p.CaseDetail.EBTStatus.Equals("Expunged", StringComparison.OrdinalIgnoreCase)).ToList();
            var result = new List<AdjusmentEbtViewModel>();

            foreach (var payment in payments)
            {
                var paymentAjusteEbtModel = new AdjusmentEbtViewModel
                {
                    PaymentId = payment.PaymentId,
                    CaseId = payment.CaseId.Value,
                    CaseDetailId = payment.CaseDetailId.Value,
                    CaseNumber = payment.CaseNumber,
                    CaseKey = payment.CaseKey,
                    AdjustmentStatusId = payment.AdjustmentStatusId,
                    AdjustmentStatus = payment.AdjustmentStatusId.HasValue ? payment.AdjustmentStatus.AdjustmentStatus1 : string.Empty,
                    AdjustmentType = payment.AdjustmentType,
                    AdjustmentRequestedDate = payment.AdjustmentRequestedDate.ToShortDateString(),
                    AdjustmentRequestedBy = payment.AdjustmentRequestedBy,
                    ConceptId = payment.ConceptId,
                    Concept = payment.ConceptId.HasValue ? payment.Concept.Concept1 : string.Empty,
                    TransactionNumber = payment.TransactionNum,
                    IssueDate = payment.IssueDate.ToShortDateString(),
                    FromDate = payment.FromDate.ToShortDateString(),
                    ToDate = payment.ToDate.ToShortDateString(),
                    PaymentDay = payment.PaymentDay.Value,
                    Amount = payment.Amount.ToCurrency()
                };

                result.Add(paymentAjusteEbtModel);
            }

            return Json(new BasicDataTablesResult(result), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveApprovals(List<ApproveRequestModel> requests)
        {
            foreach (var request in requests)
            {
                var payment = PaymentService.FindPaymentById(request.PaymentId);
                payment.AdjustmentStatusId = request.AdjustmentStatusId;
                payment.Comments = request.Comments;

                PaymentService.ModifyPayment(payment);
            }

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpGet]
        public ActionResult Document()
        {
            var model = new DocumentViewModel();

            model.AdjustmentStatuses = PaymentService.GetAllAdjustmentStatuses()
                                                     .Where(a => a.AdjustmentStatusId == 4)
                                                     .ToSelectList(a => a.AdjustmentStatus1, a => a.AdjustmentStatusId.ToString(), null, null);



            return View(model);
        }

        [HttpPost]
        public JsonResult FindRequestsToDocument()
        {

            var payments = PaymentService.FindAdjustmentEBTToDocument();
            var result = new List<AdjusmentEbtViewModel>();

            if (payments == null)
            {
                return Json(new BasicDataTablesResult(result), JsonRequestBehavior.AllowGet);
            }

            payments = payments.ToList();
            foreach (var payment in payments)
            {
                var paymentAjusteEbtModel = new AdjusmentEbtViewModel
                {
                    PaymentId = payment.PaymentId,
                    CaseId = payment.CaseId.Value,
                    CaseDetailId = payment.CaseDetailId.Value,
                    CaseNumber = payment.CaseNumber,
                    CaseKey = payment.CaseKey,
                    AdjustmentStatusId = payment.AdjustmentStatusId,
                    AdjustmentStatus = payment.AdjustmentStatusId.HasValue ? payment.AdjustmentStatus.AdjustmentStatus1 : string.Empty,
                    AdjustmentType = payment.AdjustmentType,
                    AdjustmentRequestedDate = payment.AdjustmentRequestedDate.ToShortDateString(),
                    AdjustmentRequestedBy = payment.AdjustmentRequestedBy,
                    ConceptId = payment.ConceptId,
                    Concept = payment.ConceptId.HasValue ? payment.Concept.Concept1 : string.Empty,
                    TransactionNumber = payment.TransactionNum,
                    IssueDate = payment.IssueDate.ToShortDateString(),
                    FromDate = payment.FromDate.ToShortDateString(),
                    ToDate = payment.ToDate.ToShortDateString(),
                    PaymentDay = payment.PaymentDay.Value,
                    Amount = payment.Amount.ToCurrency()
                };

                result.Add(paymentAjusteEbtModel);
            }

            return Json(new BasicDataTablesResult(result), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDocumentRequests(List<DocumentRequestModel> requests)
        {
            foreach (var request in requests)
            {
                var payment = PaymentService.FindPaymentById(request.PaymentId);

                Payment paymentDocument = new Payment();

                paymentDocument.Amount = payment.Amount;
                paymentDocument.BaseAmount = payment.BaseAmount;
                paymentDocument.CaseId = payment.CaseId;
                paymentDocument.CaseDetailId = payment.CaseDetailId;
                paymentDocument.CaseKey = payment.CaseKey;
                paymentDocument.CaseNumber = payment.CaseNumber;
                paymentDocument.CheckBk = payment.CheckBk;
                paymentDocument.ClassId = payment.ClassId;
                paymentDocument.ClinicId = payment.ClinicId;
                paymentDocument.ConceptId = payment.ConceptId;
                paymentDocument.Discount = payment.Discount;
                paymentDocument.EntityId_RemitTo = payment.EntityId_RemitTo;
                paymentDocument.FromDate = payment.FromDate;
                paymentDocument.IssueDate = payment.IssueDate;
                paymentDocument.KeyRiskIndicatorId = payment.KeyRiskIndicatorId;
                paymentDocument.PaymentDay = payment.PaymentDay;
                paymentDocument.RegionId = payment.RegionId;
                paymentDocument.StatusChangeDate = payment.StatusChangeDate;
                paymentDocument.StatusId = payment.StatusId;
                paymentDocument.ThirdPartyScheduleId = payment.ThirdPartyScheduleId;
                paymentDocument.ToDate = payment.ToDate;
                paymentDocument.TransactionId = payment.TransactionId;
                paymentDocument.TransactionNum = payment.TransactionNum;
                paymentDocument.TransferTypeId = payment.TransferTypeId;
                paymentDocument.AdjustmentRequestedBy = payment.AdjustmentRequestedBy;
                paymentDocument.AdjustmentRequestedDate = payment.AdjustmentRequestedDate;
                paymentDocument.AdjustmentStatusId = request.AdjustmentStatusId;
                paymentDocument.AdjustmentCompletedDate = request.AdjustmentCompletedDate;
                paymentDocument.AdjustmentCompletedBy = WebHelper.GetUserName();
                paymentDocument.AdjustmentAmount = request.AdjustmentAmount;
                paymentDocument.Comments = request.Comments;

                PaymentService.CreatePayment(paymentDocument);
            }

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        #endregion

        #region private methods

        private void SendNotificationEmail(Dictionary<string, List<AdjusmentEbtViewModel>> injuredPerson)
        {
            MessageTemplate templateEBT = MessageTemplateService.GetMessageTemplate("AjustesEBT.Documentar.EmailTemplate");

            if (templateEBT.IsNull())
            {
                Logger.Error("No se ha configurado la Plantilla de E-Mail para Solicitudes EBT");
                return;
            }
             

            foreach (var item in injuredPerson)
            {
                var paymentListToBeNotified = new AdjusmentEbtNotificationEmail();
                paymentListToBeNotified.Lesionado = item.Key;
                paymentListToBeNotified.NumeroCaso = item.Value[0].CaseNumber;
                paymentListToBeNotified.NumeroCuentaEbt = item.Value[0].EbtNumber;
                paymentListToBeNotified.PaymentTable = item.Value;

                try
                {
                    var setting_ebt_mail_from = SettingService.GetSettingValueByName("AjustesEBT.Notificaciones.Email.To");
                    var setting_ebt_mail_to = SettingService.GetSettingValueByName("AjustesEBT.Notificaciones.Email.From");
                    var ebt_mail_from = new MailAddress(!string.IsNullOrEmpty(setting_ebt_mail_from) ? setting_ebt_mail_from : "diego.carbajal@nagnoi.com");
                    var ebt_mail_to = new List<MailAddress>();
                    var ebt_mail_body = CreateBodyForEmail(paymentListToBeNotified, templateEBT);

                    ebt_mail_to.Add(new MailAddress(!string.IsNullOrEmpty(setting_ebt_mail_to) ? setting_ebt_mail_to : "diego.carbajal@nagnoi.com"));

                    templateEBT.Subject = templateEBT.Subject.Replace("@tipoajuste", item.Value[0].AdjustmentType);  //por Debito o Credito y @concepto
                    templateEBT.Subject = templateEBT.Subject.Replace("@concepto", item.Value[0].Concept); 
                    EmailSenderService.SendEmail(templateEBT.Subject, ebt_mail_body, ebt_mail_from, ebt_mail_to, null, null, null, null);
                }
                catch (Exception ex)
                {
                    Logger.Error("No se ha configurado  E-Mail para Solicitudes EBT", ex);
                }
            }
        }

        private AdjusmentEbtViewModel GetTableRowAdjusmentEBT(Payment payment)
        {
            var itemEmailEbt = new AdjusmentEbtViewModel
            {
                PaymentId = payment.PaymentId,
                CaseId = payment.CaseId.Value,
                CaseDetailId = payment.CaseDetailId.Value,
                CaseNumber = payment.CaseNumber,
                EbtNumber = payment.CaseDetail.EBTAccount,
                CaseKey = payment.CaseKey,
                AdjustmentStatusId = payment.AdjustmentStatusId,
                AdjustmentStatus = payment.AdjustmentStatusId.HasValue && payment.AdjustmentStatus != null && payment.AdjustmentStatus.AdjustmentStatus1 != null ? payment.AdjustmentStatus.AdjustmentStatus1 : string.Empty,
                AdjustmentType = payment.AdjustmentType,
                AdjustmentRequestedDate = payment.AdjustmentRequestedDate != null ? payment.AdjustmentRequestedDate.ToShortDateString() : string.Empty,
                AdjustmentRequestedBy = payment.AdjustmentRequestedBy,
                ConceptId = payment.ConceptId,
                Concept = payment.ConceptId.HasValue && payment.Concept != null && payment.Concept.Concept1 != null ? payment.Concept.Concept1 : string.Empty,
                TransactionNumber = payment.TransactionNum,
                IssueDate = payment.IssueDate != null ? payment.IssueDate.ToShortDateString() : string.Empty,
                FromDate = payment.FromDate != null ? payment.FromDate.ToShortDateString() : string.Empty,
                ToDate = payment.ToDate != null ? payment.ToDate.ToShortDateString() : string.Empty,
                PaymentDay = payment.PaymentDay != null ? payment.PaymentDay.Value : (short)0,
                Amount = payment.Amount != null ? payment.Amount.ToCurrency() : string.Empty
            };
            return itemEmailEbt;
        }

        string CreateBodyForEmail(AdjusmentEbtNotificationEmail emailInfo, MessageTemplate emailTemplate)
        {
            var tablebody = @" <div><table border="" 1""  cellspacing="" 0""  cellpadding="" 0"" >
            <tbody><tr><td width="" 89"" ><p><strong>Concepto</strong></p></td><td width="" 89"" >
            <p><strong>N&uacute;mero</strong><strong> de Cheque</strong></p></td>
            <td width="" 89"" ><p><strong>Fecha</strong><strong> de emisi&oacute;n</strong></p>
            </td><td width="" 89"" ><p><strong>Desde</strong></p></td>
            <td width="" 89"" ><p><strong>Hasta</strong></p></td>
            <td width="" 89"" ><p><strong>D&iacute;as</strong></p></td>
            <td width="" 89"" ><p><strong>Monto</strong></p></td></tr>";

            foreach (var row in emailInfo.PaymentTable)
            {
                tablebody += String.Format(@"<tr><td width="" 89"" >{0}</td>
                <td width="" 89"" >{1}</td> <td width="" 89"" >{2}</td>
                <td width="" 89"" >{3}</td> <td width="" 89"" >{4}</td>
                <td width="" 89"" >{5}</td> <td width="" 89"" >{6}</td>
                </tr></tbody> </table></div>",
                row.Concept, row.TransactionNumber, row.AdjustmentRequestedDate,
                row.FromDate, row.ToDate, row.PaymentDay, row.Amount);
            }

            tablebody += @"</tbody> </table>  </div>";

            return
                emailTemplate.Body.Replace("%Lesionado%", emailInfo.Lesionado)
                .Replace("%NumeroCaso%", emailInfo.NumeroCaso)
                .Replace("%NumeroEBT%", emailInfo.NumeroCuentaEbt)
                .Replace("%Desglose%", tablebody);
        }
        #endregion
    }
}
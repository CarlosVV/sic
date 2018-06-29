namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CDI.WebApplication.DataTables.Result;
    using Domain.Core.Model;
    using Infrastructure.Core.Helpers;
    using Infrastructure.Web.Controllers;
    using Infrastructure.Web.ViewModels;
    using Models;
    using Models.Payment;
    using Newtonsoft.Json;
    using Nagnoi.SiC.Web.Models.PaymentRegistration;

    #endregion

    public class PaymentsController : BaseController
    {
        #region actions
        [HttpGet]
        public ActionResult Index()
        {
            var model = new PaymentViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Search(string nombre, string sSN, string eBT, string numeroCaso)
        {
            return Json(new BasicDataTablesResult(CaseService.BuscarCasos(nombre, sSN, eBT, numeroCaso)));
        }

        [HttpGet]
        public ActionResult Resumen(int? id)
        {
            try
            {
                var datos = CaseService.InformacionCaso(id).First();
                if (datos != null)
                {
                    ViewBag.caseFolderId = id;
                    ViewBag.caseNumber = datos.CaseNumber;

                    return View("Resumen", datos);
                }
                else
                {
                    ViewBag.caseFolderId = id;
                    return View("_SinDatos");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ViewBag.caseFolderId = id;
                return View("_SinDatos");
            }
        }

       // [HttpGet]
       // [Route("Payments/ChequesPorConcepto/{*caseNumber}")]
        public JsonResult ChequesPorConcepto(string caseNumber)
        {
            return Json(new BasicDataTablesResult(CaseService.ResumenPagosPorConcepto(caseNumber)), JsonRequestBehavior.AllowGet);
        }

      //  [HttpGet]
      //  [Route("Payments/ChequesPorBeneficiario/{*caseNumber}")]
        public JsonResult ChequesPorBeneficiario(string caseNumber)
        {
            return Json(new BasicDataTablesResult(CaseService.ResumenPagosPorBeneficiario(caseNumber)), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("payments/findbyid/{*paymentId}")]
        public JsonResult FindById(int paymentId)
        {
            var model = new PaymentTransactionModel();
            var payment = PaymentService.FindPaymentById(paymentId);
            if (payment != null && payment.Transaction != null)
            {
                model = new PaymentTransactionModel
                {
                    PaymentId = payment.PaymentId,
                    TransactionId = payment.TransactionId ?? 0,
                    TransactionDate = payment.Transaction.TransactionDate.HasValue ? payment.Transaction.TransactionDate.ToShortDateString() : string.Empty,
                    DecisionDate = payment.Transaction.DecisionDate.HasValue ? payment.Transaction.DecisionDate.ToShortDateString() : string.Empty,
                    NotificationDateIC = payment.Transaction.NotificationDateIC.HasValue ? payment.Transaction.NotificationDateIC.ToShortDateString() : string.Empty,
                    ICCaseNumber = payment.Transaction.ICCaseNumber,
                    TransactionAmount = payment.Transaction.TransactionAmount.HasValue ? payment.Transaction.TransactionAmount.Value.ToString() : string.Empty,
                    HearingDateIC = payment.Transaction.HearingDateIC.HasValue ? payment.Transaction.HearingDateIC.ToShortDateString() : string.Empty,
                    MonthlyInstallment = payment.Transaction.MonthlyInstallment.HasValue ? payment.Transaction.MonthlyInstallment.Value.ToString() : string.Empty,
                    NumberOfWeeks = payment.Transaction.NumberOfWeeks.HasValue ? payment.Transaction.NumberOfWeeks.ToString() : string.Empty,
                    Observaciones = payment.Transaction.Comment,
                    FromDate = payment.FromDate.HasValue ? payment.FromDate.ToShortDateString() : string.Empty,
                    ToDate = payment.ToDate.HasValue ? payment.ToDate.ToShortDateString() : string.Empty,
                    Discount = payment.Discount.GetValueOrDefault(decimal.Zero).ToString(),
                    PaymentDay = payment.PaymentDay.GetValueOrDefault(0).ToString()
                };
            }

            return Json(new BasicDataTablesResult(model), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateInvestments(UpdateInvestmentRequest model)
        {
            var transaction = TransactionService.FindTransactionById(model.TransactionId);
            var workingDate = DateTime.Now;
            var amount = decimal.Zero;

            var modelPayments = ConvertToPaymentModel(model);

            var paymentsToUpdate = BuildInvestmentPaymentSets(modelPayments, transaction.Payments);

            foreach (var newPayment in paymentsToUpdate["new"])
            {
                var newEntity = new Entity
                {
                    FullName = newPayment.Remitter.FullName,
                    SourceId = 9
                };

                EntityService.CreateEntity(newEntity);

                var payment = new Payment
                {
                    CaseId = model.CaseId,
                    CaseDetailId = model.CaseDetailId,
                    TransactionId = transaction.TransactionId,
                    CaseNumber = model.CaseNumber,
                    ConceptId = 2,
                    ClassId = 3,
                    Amount = newPayment.Amount,
                    EntityId_RemitTo = newEntity.EntityId,
                    Remitter = newEntity,
                    ToDate = transaction.TransactionDate,
                    TransactionNum = transaction.TransactionId.ToString().PadLeft(9, '0'),
                    CheckBk = 0,
                    StatusId = 2,
                    IssueDate = workingDate,
                    StatusChangeDate = workingDate
                };

                PaymentService.CreatePayment(payment);

                amount += payment.Amount.GetValueOrDefault(decimal.Zero);
            }

            foreach (var changedPayment in paymentsToUpdate["changed"])
            {
                if (changedPayment != null && changedPayment.EntityId_RemitTo == null) continue;
                if (changedPayment == null) continue;
                var existingEntity = EntityService.GetById(changedPayment.EntityId_RemitTo.Value);
                var payment = PaymentService.FindPaymentById(changedPayment.PaymentId);
               
                if (changedPayment.Remitter != null) existingEntity.FullName = changedPayment.Remitter.FullName;
                EntityService.ModifyEntity(existingEntity);

                payment.Amount = payment.Amount.GetValueOrDefault(decimal.Zero);
                PaymentService.ModifyPayment(payment);

                amount += payment.Amount.Value;
            }

            foreach (var removedPayment in paymentsToUpdate["removed"])
            {
                if (removedPayment == null) continue;
                var transactionId = removedPayment.TransactionId;
                PaymentService.Delete(removedPayment.PaymentId);
                if (removedPayment.EntityId_RemitTo != null) EntityService.Delete(removedPayment.EntityId_RemitTo);
                if (transactionId != null) TransactionService.Delete(transactionId.Value);
            }

            transaction.TransactionAmount = amount;
            transaction.Comment = model.Comment;

            TransactionService.ModifyTransaction(transaction);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpPost]
        public JsonResult UpdateIpp(IppUpdateModel model)
        {
            var payment = PaymentService.FindPaymentById(model.PaymentId);
            payment.Amount = model.CantidadAdjudicada;

            PaymentService.ModifyPayment(payment);

            var transaction = TransactionService.FindTransactionById(model.TransactionId);
            transaction.MonthlyInstallment = model.Mensualidad;
            transaction.NumberOfWeeks = model.Semanas;
            transaction.TransactionAmount = model.CantidadAdjudicada;
            transaction.Comment = model.Comments;
            transaction.DecisionDate = model.FechaAdjudicacion;

            foreach (var desglose in model.Desgloses)
            {
                transaction.TransactionDetails.Add(new TransactionDetail
                {
                    CompensationRegionId = desglose.CompensationRegionId,
                    Percent = desglose.Percent,
                    Amount = desglose.Percent * desglose.Weeks * model.CompSemanalInca
                });
            }

            TransactionService.ModifyTransaction(transaction);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpPost]
        public JsonResult UpdatePeremptory(int paymentid, int transactionid, decimal? cantidad, string observaciones)
        {
            var payment = PaymentService.FindPaymentById(paymentid);
            payment.Amount = cantidad;

            PaymentService.ModifyPayment(payment);

            var transaction = TransactionService.FindTransactionById(transactionid);
            transaction.TransactionAmount = cantidad;
            transaction.Comment = observaciones;

            TransactionService.ModifyTransaction(transaction);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpPost]
        public JsonResult UpdateLawyer(int paymentid, int transactionid, DateTime? fechadecision, DateTime? fechavisita,
            DateTime? fechanotificacion, string numerocaso, decimal? montototal, string observaciones)
        {
            var payment = PaymentService.FindPaymentById(paymentid);
            payment.Amount = montototal;

            PaymentService.ModifyPayment(payment);

            var transaction = TransactionService.FindTransactionById(transactionid);
            transaction.DecisionDate = fechadecision;
            transaction.ICCaseNumber = numerocaso;
            transaction.NotificationDateIC = fechanotificacion;
            transaction.HearingDateIC = fechavisita;
            transaction.TransactionAmount = montototal;
            transaction.Comment = observaciones;

            TransactionService.ModifyTransaction(transaction);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpPost]
        public JsonResult UpdatePendingDiets(int caseId, int caseDetailId, int transactionId, DateTime? fechadecision, DateTime? fechavisita,
            DateTime? fechanotificacion, string numerocaso, decimal? montototal, string periods, string comment, string caseNumber)
        {
            DateTime workingDate = DateTime.Now;

            var transaction = TransactionService.FindTransactionById(transactionId);
            transaction.CaseDetailId = caseDetailId;
            transaction.TransactionAmount = montototal;
            transaction.Comment = comment;
            transaction.TransactionDate = workingDate;
            transaction.ICCaseNumber = numerocaso;
            transaction.NotificationDateIC = fechanotificacion;
            transaction.HearingDateIC = fechavisita;

            TransactionService.ModifyTransaction(transaction);

            var serializedPeriods = JsonConvert.DeserializeObject<List<PeriodDiet>>(periods);
            foreach (var period in serializedPeriods)
            {
                short totalDays = (short)(period.Hasta - period.Desde).TotalDays;

                bool isNewPayment = period.PaymentId == 0;
                if (isNewPayment)
                {
                    var payment = new Payment()
                    {
                        CaseId = transaction.CaseDetail.CaseId,
                        CaseDetailId = transaction.CaseDetailId,
                        CaseNumber = transaction.CaseDetail.CaseNumber,
                        TransactionId = transaction.TransactionId,
                        ConceptId = 2,
                        ClassId = 11,
                        Amount = montototal,
                        StatusId = 12,
                        IssueDate = fechadecision,
                        StatusChangeDate = workingDate,
                        FromDate = period.Desde,
                        ToDate = period.Hasta,
                        Discount = period.Descuento,
                        PaymentDay = totalDays
                    };

                    PaymentService.CreatePayment(payment);
                }
                else
                {
                    var existingPayment = PaymentService.FindPaymentById(period.PaymentId);
                    existingPayment.IssueDate = fechadecision;
                    existingPayment.StatusChangeDate = workingDate;
                    existingPayment.FromDate = period.Desde;
                    existingPayment.ToDate = period.Hasta;
                    existingPayment.Discount = period.Descuento;
                    existingPayment.PaymentDay = totalDays;

                    PaymentService.ModifyPayment(existingPayment);
                }
            }

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        #endregion

        #region private

        private Dictionary<string, IEnumerable<Payment>> BuildInvestmentPaymentSets(IEnumerable<Payment> sourcePayments,
            IEnumerable<Payment> existingPayments)
        {
            var result = new Dictionary<string, IEnumerable<Payment>>();

            result["new"] = sourcePayments.Where(m => m.PaymentId == 0).ToList();
            result["changed"] = sourcePayments.Where(m => m.PaymentId != 0).ToList();
            result["removed"] = existingPayments.Where(p => !sourcePayments.Any(s => s.PaymentId == p.PaymentId)).ToList();

            return result;
        }

        private static IEnumerable<Payment> ConvertToPaymentModel(UpdateInvestmentRequest model)
        {
            var modelPayments = model.Payments.Select(p => new Payment
            {
                PaymentId = p.PaymentId,
                EntityId_RemitTo = p.EntityId,
                Remitter = new Entity { EntityId = p.EntityId, FullName = p.Entidad },
                Amount = p.Inversion
            }
            ).ToList();
            return modelPayments;
        }
        #endregion
    }
}
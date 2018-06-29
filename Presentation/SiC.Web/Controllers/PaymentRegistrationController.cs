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
    using Infrastructure.Web.Helpers;
    using Infrastructure.Web.Utilities;
    using Models;
    using Models.PaymentRegistration;
    using Newtonsoft.Json;

    #endregion
    
    public class PaymentRegistrationController : BaseController
    {
        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            var allCompensationRegions = CompensationRegionService.GetAllCompensationRegions();
            var groupedCompensationRegions = CompensationRegionService.GetCompensationRegionsGroupedByRegion();

            model.CompensationRegions = allCompensationRegions.ToSelectList(c => c.Code, c => string.Format("{0}|{1}|{2}|{3}|{4}", c.Code, c.Region, c.SubRegion, c.Weeks, c.CompensationRegionId), null, "Seleccionar");
            model.GroupedCompensationRegions = groupedCompensationRegions.ToSelectList(c => c.Region, c => c.Region, null, "Seleccionar");

            return View(model);
        }

        [HttpPost]
        public JsonResult InsertInvestment(int caseId, int caseDetailId, DateTime? fechadecision, string payments, string comment, string caseNumber, int beneficiario)
        {
            DateTime workingDate = DateTime.Now;

            var currentCase = CaseService.FindCaseByNumber(caseNumber);

            var transaction = new Transaction
            {
                CaseDetailId = caseDetailId,
                TransactionTypeId = (int) TransactionTypeEnum.Inversion,
                TransactionDate = workingDate,
                TransactionAmount = decimal.Zero,
                Comment = comment
            };

            TransactionService.CreateTransaction(transaction);

           
            var amount = decimal.Zero;

            var serializedPayments = JsonConvert.DeserializeObject<List<PaymentTransaction>>(payments);
            foreach (var serializedPayment in serializedPayments)
            {
                var entidad = new Entity
                {
                    FullName = serializedPayment.Entidad,
                    SourceId = 9
                };
                EntityService.CreateEntity(entidad);

                var payment = new Payment
                {
                    CaseId = caseId,
                    CaseDetailId = caseDetailId,
                    TransactionId = transaction.TransactionId,
                    CaseNumber = caseNumber,
                    ConceptId = currentCase.ConceptId.Value,
                    ClassId = 3,
                    Amount = serializedPayment.Inversion,
                    EntityId_RemitTo = entidad.EntityId,
                    Remitter = entidad,
                    ToDate = fechadecision,
                    TransactionNum = transaction.TransactionId.ToString().PadLeft(9, '0'),
                    CheckBk = 0,
                    StatusId = 12,
                    IssueDate = workingDate,
                    StatusChangeDate = workingDate
                };
                amount += serializedPayment.Inversion.Value;

                PaymentService.CreatePayment(payment);
            }

            transaction.TransactionAmount = amount;

            TransactionService.ModifyTransaction(transaction);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpPost]
        public JsonResult InsertPeremptory(int caseId, int caseDetailId, int entityid, decimal? cantidad, int beneficiario, string comment, string caseNumber)
        {
            DateTime workingDate = DateTime.Now;

            var transaction = new Transaction
            {
                CaseDetailId = caseDetailId,
                TransactionTypeId = (int) TransactionTypeEnum.Anticipos,
                TransactionDate = workingDate,
                TransactionAmount = cantidad,
                Comment = comment
            };
            TransactionService.CreateTransaction(transaction);

            var payment = new Payment
            {
                CaseId = caseId,
                CaseDetailId = caseDetailId,
                TransactionId = transaction.TransactionId,
                CaseNumber = caseNumber,
                ConceptId = 2,
                ClassId = 2,
                Amount = cantidad,
                TransactionNum = transaction.TransactionId.ToString().PadLeft(9, '0'),
                StatusId = 12,
                IssueDate = workingDate,
                StatusChangeDate = workingDate            
            };
            PaymentService.CreatePayment(payment);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpPost]
        public JsonResult InsertPendingDiet(int caseId, int caseDetailId, DateTime? fechadecision, DateTime? fechavisita,
            DateTime? fechanotificacion, string numerocaso, decimal? montototal, string periods, string comment, string caseNumber)
        {
            CaseDetail currentCase = CaseService.FindCaseDetailById(caseDetailId);

            DateTime workingDate = DateTime.Now;

            var transaction = new Transaction()
            {
                CaseDetailId = caseDetailId,
                TransactionTypeId = (int) TransactionTypeEnum.Retroactivo,
                TransactionAmount = montototal,
                Comment = comment,
                TransactionDate = workingDate,
                ICCaseNumber = numerocaso,
                NotificationDateIC = fechanotificacion,
                HearingDateIC = fechavisita
            };

            TransactionService.CreateTransaction(transaction);

            var serializedPeriods = JsonConvert.DeserializeObject<List<PeriodDiet>>(periods);

            foreach (var period in serializedPeriods)
            {
                short totalDays = Common.GetBusinessDays(period.Desde, period.Hasta, currentCase.Case.DaysWeek.GetValueOrDefault(0));
                
                var payment = new Payment()
                {
                    CaseId = caseId,
                    CaseDetailId = caseDetailId,
                    CaseNumber = caseNumber,
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
            
            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpPost]
        public JsonResult InsertHonoraryLawyer(int caseId, int caseDetailId, DateTime? fechadecision, DateTime? fechavisita,
            DateTime? fechanotificacion, string numerocaso, decimal? montototal, string comment, string caseNumber)
        {
            DateTime workingDate = DateTime.Now;

            var transaction = new Transaction()
            {
                CaseDetailId = caseDetailId,
                TransactionTypeId = (int)TransactionTypeEnum.HonorarioAbogados,
                TransactionAmount = montototal,
                Comment = comment,
                DecisionDate = fechadecision,
                TransactionDate = workingDate,
                ICCaseNumber = numerocaso,
                NotificationDateIC = fechanotificacion,
                HearingDateIC = fechavisita
            };
            TransactionService.CreateTransaction(transaction);

            var payment = new Payment
            {
                CaseId = caseId,
                CaseDetailId = caseDetailId,
                TransactionId = transaction.TransactionId,
                CaseNumber = caseNumber,
                ConceptId = 1,
                ClassId = 8,
                Amount = montototal,
                TransactionNum = transaction.TransactionId.ToString().PadLeft(9, '0'),
                StatusId = 12,
                IssueDate = workingDate,
                StatusChangeDate = workingDate
            };
            PaymentService.CreatePayment(payment);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpPost]
        public JsonResult InsertIpp(IppCreateModel model)
        {
            DateTime workingDate = DateTime.Now;

            var transaction = new Transaction()
            {
                CaseDetailId = model.CaseDetailId,
                TransactionTypeId = (int)TransactionTypeEnum.AdjudicacionAdicional,
                TransactionAmount = model.CantidadAdjudicada,
                MonthlyInstallment = model.Mensualidad,
                NumberOfWeeks = model.Semanas,
                DecisionDate = model.FechaAdjudicacion,
                TransactionDate = workingDate,
                Comment = model.Comments,
                ConceptId = 3
            };

            foreach (var desglose in model.Desgloses)
            {
                transaction.TransactionDetails.Add(new TransactionDetail
                {
                    CompensationRegionId = desglose.CompensationRegionId,
                    Percent = desglose.Percent,
                    Amount = desglose.Percent * desglose.Weeks * model.CompSemanalInca
                });
            }
            TransactionService.CreateTransaction(transaction);
            
            var payment = new Payment
            {
                CaseId = model.CaseId,
                CaseDetailId = model.CaseDetailId,
                TransactionId = transaction.TransactionId,
                CaseNumber = model.CaseNumber,
                CaseKey = model.CaseKey,
                PaymentDay = 0,
                ConceptId = 3,
                ClassId = 8,
                Amount = model.CantidadAdjudicada,
                TransactionNum = transaction.TransactionId.ToString().PadLeft(9, '0'),
                StatusId = 12,
                IssueDate = workingDate,
                StatusChangeDate = workingDate
            };
            PaymentService.CreatePayment(payment);

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        [HttpPost]
        public JsonResult GetSummaryInvestments(int caseId, int caseDetailId, bool esBeneficiario)
        {
            IEnumerable<Transaction> transactions;

            if (esBeneficiario)
            {
                transactions = TransactionService.SearchInvestmentsTransactions(null, caseDetailId)
                    .OrderBy(transaction => transaction.TransactionDate);                
            }
            else
            {
                transactions = TransactionService.SearchInvestmentsTransactions(caseId, null)
                    .OrderBy(transaction => transaction.TransactionDate);                
            }

            var summary = transactions.Select(transaction => new
            {
                Fecha = transaction.TransactionDate.HasValue ? transaction.TransactionDate.ToShortDateString() : string.Empty,
                Beneficiario = new
                {
                    Nombre = transaction.CaseDetail.Entity == null ? string.Empty : transaction.CaseDetail.Entity.FullName,
                    Relacion = transaction.CaseDetail.Entity == null ? string.Empty : transaction.CaseDetail.RelationshipType == null ? string.Empty : transaction.CaseDetail.RelationshipType.RelationshipType1
                },
                Cantidad = transaction.TransactionAmount.ToCurrency(),
                PagoInicial = decimal.Zero
            });
            return Json(new BasicDataTablesResult(summary));
        }

        [HttpPost]
        public JsonResult GetSummaryPeremptories(int caseId, int caseDetailId, bool esBeneficiario)
        {
            IEnumerable<Payment> payments;

            if (esBeneficiario)
            {
                payments = this.PaymentService.SearchPeremptoryPayments(null, caseDetailId)
                                              .OrderBy(payment => payment.IssueDate)
                                              .ToList();
            }
            else
            {
                payments = this.PaymentService.SearchPeremptoryPayments(caseId, null)
                                              .OrderBy(payment => payment.IssueDate)
                                              .ToList();
            }
            
            var summary = payments.Select(payment => new
            {
                Beneficiario = payment.CaseDetail.Entity == null ? string.Empty : payment.CaseDetail.Entity.FullName,
                Relacion = payment.CaseDetail.Entity == null ? string.Empty : payment.CaseDetail.RelationshipType == null ? string.Empty : payment.CaseDetail.RelationshipType.RelationshipType1,
                Fecha = payment.IssueDate.HasValue ? payment.IssueDate.Value.ToShortDateString() : string.Empty,
                MontoPagado = payment.Amount.ToCurrency()
            });
            
            return Json(new BasicDataTablesResult(summary));
        }

        [HttpPost]
        public JsonResult GetSummaryIpps(int caseDetailId)
        {
            var transactions = TransactionService.SearchIppTransactions(caseDetailId)
                                                 .OrderBy(t => t.TransactionDate)
                                                 .ToList();
            var summary = transactions.Select(transaction => new
            {
                FechaAdjudicacion = transaction.DecisionDate.HasValue ? transaction.DecisionDate.Value.ToShortDateString() : string.Empty,
                Semanas = transaction.NumberOfWeeks,
                Mensualidad = transaction.MonthlyInstallment.ToCurrency(),
                CantidadAdjudicada = transaction.TransactionAmount.ToCurrency()
            });

            return Json(new BasicDataTablesResult(summary));
        }

        [HttpPost]
        public JsonResult GetMonthlyConceptAndTransactionInformation(string caseDate, int caseId)
        {
            DateTime newCaseDate;

            DateTime.TryParse(caseDate, out newCaseDate);

            object[] result = new object [4];

            if (newCaseDate.IsNull())
                return Json(new { Success = false }, JsonRequestBehavior.AllowGet);

            var monthlyConcept = ConceptService.GetByConceptAndYear("IPP", "Incapacidad", newCaseDate.Year);
            
            var success = true;

            if (monthlyConcept == null)
                success = false;

            var transactionsIPP = TransactionService.SearchIppTransactions(caseId);

            DateTime? MaxDate = (from d in transactionsIPP select d.DecisionDate).Max();

            decimal? TotalTransactionAmount = (from d in transactionsIPP select d.TransactionAmount).Sum();

            result[0] = MaxDate; //Maximun transaction date accross all IPP Transactions
            result[1] = !monthlyConcept.IsNull() ? monthlyConcept.MonthlyPayment : null; //Monthly Payment
            result[2] = !monthlyConcept.IsNull() ? monthlyConcept.Maximum : null; //Maximun amount to be paid for a case year
            result[3] = TotalTransactionAmount; //Total transaction amounts of IPP transactions

            return Json(new { Success = success, Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("paymentregistration/getentityrelatives/{caseId}")]
        public JsonResult GetEntityRelatives(int caseId)
        {
            var relatives = CaseService.FindRelatives(caseId).ToList();
            var results = relatives.Select(relationship => new
            {
                Value = string.Format("{0}_{1}_{2}_{3}", relationship.EntityId, relationship.RelationshipType.RelationshipType1, relationship.CaseDetailId, relationship.RelationshipType.RelationshipType1.IndexOf("Viuda") >= 0 || relationship.RelationshipType.RelationshipType1.IndexOf("Concubina") >= 0 || relationship.RelationshipType.RelationshipType1.IndexOf("Padre") >= 0 || relationship.RelationshipType.RelationshipType1.IndexOf("Madre") >= 0 ? true : false),
                Text = relationship.Entity.FullName
            });

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
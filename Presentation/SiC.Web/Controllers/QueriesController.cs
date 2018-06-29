using Nagnoi.SiC.Infrastructure.Web.Controllers;
using Nagnoi.SiC.Web.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Infrastructure.Core.Helpers;
using CDI.WebApplication.DataTables.Result; 

namespace Nagnoi.SiC.Web.Controllers {    
    public class QueriesController : BaseController
    {
        //
        // GET: /Queries/
        [Route("Queries/Index/")]
        public ActionResult Index(string CaseNumber)
        {
           var model = new QueriesViewModel();
           return View(model);
        }

        public ActionResult PaymentsWithMoreThan500()
        {
            return View();
        }

        public ActionResult NonCompliantPayments()
        {
            return View();
        }

        public ActionResult CasesInDormantOrExpunged()
        {
            return View();
        } 



        [HttpGet]
        //[Route("Queries/PaymentsWithMoreThan500/")]
        public JsonResult FindPaymentGreaterThan500()
        {
            QueriesViewModel model = new QueriesViewModel();

            var caseData = PaymentService.PaymentQuery("PaymentGreaterThan500");

            var results = caseData.Select(r => new
                {
                    caseNumber = r.CaseNumber,
                    caseKey = r.CaseKey,
                    fromDate = r.FromDate != null ? ((DateTime)r.FromDate).ToString("yyyy-MM-dd") : "N/A",
                    toDate = r.ToDate != null ? ((DateTime)r.ToDate).ToString("yyyy-MM-dd") : "N/A",
                    paymentDay = r.PaymentDay,
                    paymentAmount = r.Amount,
                    fullName = r.CaseDetail.Entity.FullName,
                    ssn = r.CaseDetail.Entity.SSN.ToSSN(),
                    birthdate = r.CaseDetail.Entity.BirthDate,
                    dailywage = r.Case.DailyWage,
                    weeklycomp = r.Case.WeeklyComp,
                    status = r.Status.Status1
                });

            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FindNonComplaintPayments(){

            QueriesViewModel model = new QueriesViewModel();

            var caseData = PaymentService.PaymentQuery("NonCompliantPayment");

            var results = caseData.Select(r => new
            {
                caseNumber = r.CaseNumber,
                caseKey = r.CaseKey,
                fromDate = r.FromDate != null ? ((DateTime)r.FromDate).ToString("yyyy-MM-dd") : "N/A",
                toDate = r.ToDate != null ? ((DateTime)r.ToDate).ToString("yyyy-MM-dd") : "N/A",
                paymentDay = r.PaymentDay,
                paymentAmount = r.Amount,
                fullName = r.CaseDetail.Entity.FullName,
                ssn = r.CaseDetail.Entity.SSN.ToSSN(),
                birthdate = r.CaseDetail.Entity.BirthDate,
                dailywage = r.Case.DailyWage,
                weeklycomp = r.Case.WeeklyComp,
                status = r.Status.Status1
            });

            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FindCasesDormantOrExpunged()
        {
            var caseData = CaseService.FindCasesInDormantExpunged();

               var results = caseData.Select(r => new
               {
                       caseNumber =r.CaseNumber,
                       caseKey = r.CaseKey,
                       fullName = r.Entity.FullName,
                       ssn = r.Entity.SSN.ToSSN(),
                       birthDate = !r.Entity.BirthDate.IsNull() ? ((DateTime)r.Entity.BirthDate).ToString("yyyy-MM-dd") : "N/A",
                       EBTAccount = !r.EBTAccount.IsNull() ? r.EBTAccount : "N/A",
                       EBTStatus  = !r.EBTStatus.IsNull()  ? r.EBTStatus : "N/A",
                       Region = !r.Case.Region.IsNull() ? r.Case.Region.Region1 : "N/A",
                       Clinic = !r.Case.Clinic.IsNull() ? r.Case.Clinic.Clinic1 : "N/A",
                       Injury = !r.Case.Concept.IsNull() ? r.Case.Concept.Concept1 : "N/A"
                   });
               //return Json(results, JsonRequestBehavior.AllowGet);
               return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("Queries/CaseInformation/{CaseNumber}")]
        public ActionResult CaseInformation(string CaseNumber)
        {
            QueriesViewModel model = new QueriesViewModel();

            model.CaseModel.Case = CaseService.FindCaseDetailByNumber(CaseNumber);

            return View(model);
        }

        public JsonResult IPPSummary(int CaseDetailId, String CaseKey){
            QueriesViewModel model = new QueriesViewModel();

            var caseResult = TransactionService.GetTransactionByConceptCasefolder(CaseDetailId,CaseKey, 3);
         
            var results = caseResult.Select(r => new
            {
                Amount = !r.TransactionAmount.IsNull() ? r.TransactionAmount : 0 ,
                Weeks = r.NumberOfWeeks,
                Date = !r.DecisionDate.IsNull() ? ((DateTime)r.DecisionDate).ToString("yyyy-MM-dd") : "N/A"
            });

            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ITPSummary(int CaseDetailId, String CaseKey)
        {

            QueriesViewModel model = new QueriesViewModel();



            var caseResult = TransactionService.GetTransactionByConceptCasefolder(CaseDetailId, CaseKey, 5);

            var results = caseResult.Select(r => new
            {
                Date = !r.DecisionDate.IsNull() ? ((DateTime)r.DecisionDate).ToString("yyyy-MM-dd") : "N/A",
                Reserve = !r.CaseDetail.Reserve.IsNull() ? r.CaseDetail.Reserve : 0,
                Installment = !r.CaseDetail.MonthlyInstallment.IsNull() ? r.CaseDetail.MonthlyInstallment : 0,
                Payment = !r.CaseDetail.Payments.IsNull() ? r.CaseDetail.Payments.Where(x => x.Amount > 0.00m).Where(x => x.Status.Effect == "+").Where(x => x.ConceptId != 2).Sum(x => x.Amount): 0.00m,
                Balance = !r.CaseDetail.Payments.IsNull() ? !r.CaseDetail.Transactions.IsNull() ?  r.CaseDetail.Transactions.Where(x => (x.TransactionTypeId == 1 || x.TransactionTypeId == 2))
                                                                                                                            .Sum(x => x.TransactionAmount) - r.CaseDetail.Payments.Where(x => x.Amount > 0.00m)
                                                                                                                                                                         .Where(x => x.Status.Effect == "+")
                                                                                                                                                                         .Where(x => x.Concept.ConceptType == "Incapacidad")
                                                                                                                                                                         .Sum(x => x.Amount) 
                                                                                                   : r.CaseDetail.Payments.Where(x => x.Amount > 0.00m)
                                                                                                                          .Where(x => x.Status.Effect == "+")
                                                                                                                          .Where(x => x.Concept.ConceptType == "Incapacidad")
                                                                                                                          .Sum(x => x.Amount) : !r.CaseDetail.Transactions.IsNull() ?   r.CaseDetail.Transactions.Where(x => (x.TransactionTypeId == 1 || x.TransactionTypeId == 2))
                                                                                                                                                                                                                 .Sum(x => x.TransactionAmount) : 0.00m,
               BalanceIPP = !r.CaseDetail.Payments.IsNull() ? !r.CaseDetail.Transactions.IsNull() ? r.CaseDetail.Transactions.Where(x => (x.TransactionTypeId == 1 || x.TransactionTypeId == 2)).Where(x => (x.ConceptId ==3 || x.ConceptId == 7 || x.ConceptId == 9))
                                                                                                                            .Sum(x => x.TransactionAmount) - r.CaseDetail.Payments.Where(x => (x.ConceptId == 3 || x.ConceptId == 7 || x.ConceptId == 9)).Where(x => x.Status.Effect == "+")
                                                                                                                                                                                  .Sum(x => x.Amount) : r.CaseDetail.Payments.Where(x => (x.ConceptId == 3 || x.ConceptId == 7 || x.ConceptId == 9)).Where(x => x.Status.Effect == "+")
                                                                                                                                                                                  .Sum(x => x.Amount) : !r.CaseDetail.Transactions.IsNull() ? r.CaseDetail.Transactions.Where(x => (x.TransactionTypeId == 1 || x.TransactionTypeId == 2))//.Where(x => (x.ConceptId == 3 || x.ConceptId == 7 || x.ConceptId == 9))
                                                                                                                                                                                                                                                          .Sum(x => x.TransactionAmount) : 0.00m
            });

            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);
        }

        public JsonResult InversionSummary(int CaseDetailId, String CaseKey)
        {
            QueriesViewModel model = new QueriesViewModel();

            var caseResult = TransactionService.GetTransactionTypeInversionCase(CaseDetailId,CaseKey);

            var results = caseResult.Select(r => new
                {
                    Date = !r.DecisionDate.IsNull() ? ((DateTime)r.DecisionDate).ToString("yyyy-MM-dd") : "N/A",
                    Amount = !r.TransactionAmount.IsNull() ? r.TransactionAmount : 0
                });

           return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet); 
        }

        public JsonResult BeneficiarySummary(int CaseDetailId, String CaseKey, int CaseId)
        {
            QueriesViewModel model = new QueriesViewModel();

            var caseResults = TransactionService.FindCaseBeneficiariesByCase(CaseDetailId, CaseKey, CaseId);

            var results = caseResults.Select(r => new
            {
                FullName = !r.CaseDetail.Entity.FullName.IsNull() ? r.CaseDetail.Entity.FullName : "N/A",
                Concept = !r.CaseDetail.Case.Concept.Concept1.IsNull() ? r.CaseDetail.Case.Concept.Concept1 : "N/A",
                Suffix = !r.CaseDetail.CaseKey.IsNull() ? r.CaseDetail.CaseKey : "N/A",
                BirthDate = !r.CaseDetail.Entity.BirthDate.IsNull() ? ((DateTime)r.CaseDetail.Entity.BirthDate).ToString("yyyy-MM-dd") : "N/A",
                SSN = !r.CaseDetail.Entity.SSN.IsNull() ? r.CaseDetail.Entity.SSN.ToSSN() : "N/A",
                Relation = !r.CaseDetail.RelationshipType.IsNull() ? r.CaseDetail.RelationshipType.RelationshipType1 : "N/A",
                Reserve = !r.CaseDetail.Reserve.IsNull() ? r.CaseDetail.Reserve : 0, 
                Minstallment = !r.CaseDetail.MonthlyInstallment.IsNull() ? r.CaseDetail.MonthlyInstallment : 0,
                Payed = !r.CaseDetail.Payments.IsNull() ? r.CaseDetail.Payments.Where(x => x.Amount > 0.00m).Where(x => x.Status.Effect == "+").Where(x => x.Concept.ConceptType == "Incapacidad").Sum(x => x.Amount) : 0, 
                Balance = !r.CaseDetail.Payments.IsNull() ? !r.CaseDetail.Transactions.IsNull() ?  r.CaseDetail.Transactions.Where(x => (x.TransactionTypeId == 1 || x.TransactionTypeId == 2))
                                                                                                                            .Sum(x => x.TransactionAmount) - r.CaseDetail.Payments.Where(x => x.Amount > 0.00m)
                                                                                                                                                                         .Where(x => x.Status.Effect == "+")
                                                                                                                                                                         .Where(x => x.Concept.ConceptType == "Incapacidad")
                                                                                                                                                                         .Sum(x => x.Amount) 
                                                                                                   : r.CaseDetail.Payments.Where(x => x.Amount > 0.00m)
                                                                                                                          .Where(x => x.Status.Effect == "+")
                                                                                                                          .Where(x => x.Concept.ConceptType == "Incapacidad")
                                                                                                                          .Sum(x => x.Amount) : !r.CaseDetail.Transactions.IsNull() ?   r.CaseDetail.Transactions.Where(x => (x.TransactionTypeId == 1 || x.TransactionTypeId == 2))
                                                                                                                                                                                                                 .Sum(x => x.TransactionAmount) : 0.00m
            }).Distinct();


            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);
        }

        public JsonResult BeneficiaryDetail(int CaseDetailId, String CaseKey, int CaseId)
        {
            QueriesViewModel model = new QueriesViewModel();

            var caseResults = TransactionService.FindBeneficiaryDetail(CaseDetailId,CaseKey,CaseId);

            var results = caseResults.Select(r => new
                {
                    FullName = !r.CaseDetail.Entity.FullName.IsNull() ? r.CaseDetail.Entity.FullName : "N/A",
                    TType = !r.TransactionType.TransactionType1.IsNull() ? r.TransactionType.TransactionType1 : "N/A",
                    Amount = !r.TransactionAmount.IsNull() ? r.TransactionAmount : 0,
                    Date = !r.TransactionDate.IsNull() ? ((DateTime)r.TransactionDate).ToString("yyyy-MM-dd") : "N/A",
                });

            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet); 

        }
    }
}


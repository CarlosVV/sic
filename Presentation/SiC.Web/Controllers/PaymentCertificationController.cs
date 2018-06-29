namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CDI.WebApplication.DataTables.Result;
    using Infrastructure.Core.Helpers;
    using Nagnoi.SiC.Infrastructure.Web.Controllers;
    using Nagnoi.SiC.Infrastructure.Web.ViewModels;
    using System.Data;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Web.Models.PaymentCertification;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    #endregion    
    public class PaymentCertificationController : BaseController
    {
        public ActionResult Index()
        {
            var certificationModel = new CertificationViewModel();
            
            return View(certificationModel);
        }

        [HttpGet]
        [Route("PaymentCertification/Certification/{caseDetailId}")]
        public ActionResult Certification(string caseDetailId)
        {
            int id = Convert.ToInt32(caseDetailId);

            var caseData = CaseService.FindCaseDetailById(id);

            ViewBag.CaseNumber = caseData.CaseNumber;
            ViewBag.CaseKey = caseData.CaseKey;
           
            return View();
        }

        [HttpPost]
        [Route("PaymentCertification/Result/")]
        public JsonResult Result(string caseNumber, string caseKey, int? caseDetailId)
        {
            if (caseDetailId == null)
            {
                caseDetailId = CaseService.FindCaseDetailByNumber(caseNumber, caseKey).CaseDetailId;
                caseKey = "00";
            }

            var paymentResult = PaymentService.FindPaymentCertificationsByCaseNumber(caseNumber, caseKey).ToList();
            var balanceResult = TransactionService.GetBalanceDetailByCase(caseDetailId);

            if (balanceResult != null)
            {
                Payment balance = new Payment()
                {
                    PaymentId = -1,
                    CaseNumber = balanceResult.CaseNumber,
                    CaseKey = balanceResult.CaseKey,
                    Amount = balanceResult.Amount,
                    CaseId = balanceResult.CaseId,
                    CaseDetailId = balanceResult.CaseDetailId,
                    ConceptId = balanceResult.ConceptId,
                    ClassId = balanceResult.ClassId,
                    FromDate = Convert.ToDateTime("01-01-1900"),
                    ToDate = Convert.ToDateTime("01-01-1900"),
                    StatusId = -1,
                    Concept = balanceResult.Concept
                };

                paymentResult.Add(balance);
            }

            var displayResults = paymentResult.Select(r => new
                {
                    paymentId = r.PaymentId,
                    caseNumber = r.CaseNumber,
                    caseKey = r.CaseKey != null ? r.CaseKey : "00",
                    concept = r.Concept.Concept1 != null ? r.Concept.Concept1 :  "N/A",
                    fromDate = r.FromDate != null ? ((DateTime)r.FromDate).ToString("yyyy-MM-dd") : "N/A",
                    toDate = r.ToDate != null ? ((DateTime)r.ToDate).ToString("yyyy-MM-dd") : "N/A",
                    status = "Pago", //Tipo de Pago -- Todos los datos que vengan de los resultados son pagos.
                    invoiceNumber = "",
                    amount = r.Amount,
                    caseDetailId = r.CaseDetailId
                });
            
            return Json(new BasicDataTablesResult(displayResults));
        }

        public JsonResult CertificationAdd(string action)
        {
            if (action == "create")
            {
                string fromDateStr = Request.Form["data[0][fromDate]"].ToString();
                string toDateStr = Request.Form["data[0][toDate]"].ToString();

                string caseNumber = Request.Form["data[0][caseNumber]"].ToString();
                string caseKey = Request.Form["data[0][caseKey]"].ToString();
                if(String.IsNullOrEmpty(caseKey)){
                    caseKey = "00";
                }
                string invoiceNumber = Request.Form["data[0][invoiceNumber]"].ToString();
                string status = Request.Form["data[0][status]"].ToString();
                string concept = Request.Form["data[0][concept]"].ToString();
                
                DateTime fromDate = concept != "Dieta" && concept != "Remanente Dieta" ?
                    Convert.ToDateTime("1900-01-01") :
                    DateTime.ParseExact(fromDateStr, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime toDate = concept != "Dieta" && concept != "Remanente Dieta" ?
                    Convert.ToDateTime("1900-01-01") :
                    DateTime.ParseExact(toDateStr, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string amount = Request.Form["data[0][amount]"].ToString();
               
                var dataInserts = new List<Object>
                {
                    new {
                        paymentId = -1,
                        caseNumber = caseNumber,
                        caseKey = caseKey,
                        concept =  concept,
                        fromDate = fromDate,
                        toDate = toDate,
                        status = status,
                        invoiceNumber = invoiceNumber,
                        amount = amount
                    }
                };

                return Json(new { data = dataInserts}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CertificationEditor(CertificationViewModel model)
        {

            var currentCase = CaseService.FindCaseDetailByNumber(model.CaseNumber, model.CaseKey);                            

            var transactionTemp = new List<Transaction>();
            var concept = PaymentService.GetAllConcepts();
            var status = PaymentService.GetAllPaymentStatuses().Where(x => x.Status1 == "Certificado").Select(s => s.StatusId).FirstOrDefault(); ;
            var transactionTypeId = TransactionTypeService.GetTransactionTypes().Where(x => x.TransactionType1 == "Descuento").Select(s => s.TransactionTypeId).FirstOrDefault();

            foreach (var certification in model.Certification)
            {
                var payment = new Payment();
                var conceptId = concept.Where(x => x.Concept1 == certification.Concept).Select(s => s.ConceptId).FirstOrDefault();
                if (certification.PaymentId != -1)
                {
                    payment = PaymentService.FindPaymentById(certification.PaymentId);
                    payment.ModifiedBy = WebHelper.GetUserName();
                    payment.ModifiedDateTime = DateTime.Now;
                }
                else //Assigns necessary information when the payment doesn't exist in the table.
                {
                    payment.PaymentId = certification.PaymentId;
                    payment.CaseId = currentCase.CaseId;
                    payment.CaseDetailId = currentCase.CaseDetailId;
                    payment.CaseNumber = model.CaseNumber;
                    payment.CaseKey = model.CaseKey == null ? "00" : model.CaseKey;
                    payment.TransactionNum = certification.InvoiceNumber;
                    payment.Amount = certification.Status == "Descuento" ? Math.Abs(Convert.ToDecimal(certification.Amount)) * -1 : Convert.ToDecimal(certification.Amount);
                    payment.FromDate = Convert.ToDateTime(certification.FromDate);
                    payment.ToDate = Convert.ToDateTime(certification.ToDate); 
                    payment.ConceptId = conceptId;
                    payment.StatusId = status;
                    payment.CreatedBy = WebHelper.GetUserName();
                    payment.CreatedDateTime = DateTime.Now;
                }

                payment.StatusId = status;
                payment.Comments = model.Comment;

                bool IsDiscount = certification.Status == "Descuento" ? true : false;

                if (IsDiscount && certification.PaymentId == -1)
                {
                    int? referenceCaseId = null;
                    string referenceCaseNumber = null;

                    if (model.CaseNumber != certification.CaseNumber)
                    {
                        var caseReference = CaseService.FindCaseByNumber(certification.CaseNumber);
                        referenceCaseId = caseReference.CaseId;
                        referenceCaseNumber = caseReference.CaseNumber;
                    }

                    Transaction transactionLine = null;
                    transactionLine = TransactionService.GetTransaction(currentCase.CaseDetailId, certification.InvoiceNumber, conceptId, transactionTypeId);

                    if (transactionLine == null)
                    {
                        transactionLine = new Transaction();
                        transactionLine.CaseDetailId = currentCase.CaseDetailId;
                        transactionLine.TransactionTypeId = transactionTypeId;
                        transactionLine.TransactionAmount = payment.Amount;
                        transactionLine.CaseId_Reference = referenceCaseId;
                        transactionLine.CaseNumber_Reference = referenceCaseNumber;
                        transactionLine.ConceptId = conceptId;
                        transactionLine.InvoiceNumber = certification.InvoiceNumber.IsNullOrEmpty() ? null : certification.InvoiceNumber;
                        transactionLine.Comment = model.Comment;
                        transactionLine.CreatedBy = WebHelper.GetUserName();
                        transactionLine.CreatedDateTime = DateTime.Now;
                        TransactionService.CreateTransaction(transactionLine);
                    }
                    else
                    {
                        transactionLine.TransactionAmount += payment.Amount;
                        transactionLine.Comment = model.Comment;
                        transactionLine.ModifiedBy = WebHelper.GetUserName();
                        transactionLine.ModifiedDateTime = DateTime.Now;
                        TransactionService.ModifyTransaction(transactionLine);
                    }
                    payment.TransactionId = transactionLine.TransactionId;
                }

                if (payment.PaymentId == -1)
                    PaymentService.CreatePayment(payment);
                else
                    PaymentService.ModifyPayment(payment);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllConcept()
        {
            IList<Object> lstConcepts = new List<Object>();
            var concept = PaymentService.GetAllConcepts();

            foreach (var c in concept)
            {
                if (c.ConceptType != "NA")
                {
                    lstConcepts.Add(new { label = c.Concept1, value = c.Concept1 });
                }
            }

            return Json(lstConcepts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllStatus()
        {
            IList<Object> lstStatus = new List<Object>();
            var status = PaymentService.GetAllPaymentStatuses();
            foreach (var s in status)
            {
                if (s.StatusCode == "D")
                {
                    lstStatus.Add(new { label = s.Status1, value = s.Status1 });
                }
            }
            return Json(lstStatus, JsonRequestBehavior.AllowGet);
        } 

        [HttpPost]
        public JsonResult CaseExist(string caseNumber)
        {
            bool CaseExist;
            try
            {
                var Case = CaseService.FindCaseByNumber(caseNumber);
                if (Case != null)
                    CaseExist = true;
                else
                    CaseExist = false;
            }
            catch {
                CaseExist = false;
            }

            return Json(new { caseExist = CaseExist}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CaseRelated(string caseNumber, string searchNumber)
        {
            bool caseRelated = false;
            try
            {
                var Case = CaseService.FindRelatedCasesByCaseNumber(searchNumber);
                foreach (var c in Case)
                {
                    if (c.CaseNumber == caseNumber)
                    {
                        caseRelated = true;
                    }
                }
            }
            catch
            {
                caseRelated = false;
            }

            return Json(new { caseRelated = caseRelated }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CaseDetailInfo(string CaseDetailId)
        {
            try
            {
                int id = Convert.ToInt32(CaseDetailId);
                var caseDetail = CaseService.FindCaseDetailById(id);
                return Json(new { 
                    success = true, 
                    caseNumber = caseDetail.CaseNumber, 
                    caseKey = caseDetail.CaseKey,
                    caseDetailId = caseDetail.CaseDetailId
                }, JsonRequestBehavior.AllowGet);
            }
            catch {
                return Json(new { success = false, caseNumber = "", caseKey = "", caseDetailId = 0}, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}
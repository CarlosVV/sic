using Nagnoi.SiC.Infrastructure.Web.Controllers;
using Nagnoi.SiC.Web.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nagnoi.SiC.Web.Models;
using Nagnoi.SiC.Web.Models.Paging;
using Nagnoi.SiC.Domain.Core.Model;
using Nagnoi.SiC.Infrastructure.Core.Helpers;
using CDI.WebApplication.DataTables.Result;
using Nagnoi.SiC.Web.Models.PreExisting;
using Nagnoi.SiC.Web.Models.Case;
using X.PagedList;
using Nagnoi.SiC.Web.Models.PaymentRegistration;
//using Nagnoi.SiC.Infrastructure.Web.Controllers;
using Nagnoi.SiC.Infrastructure.Web.Utilities;


namespace Nagnoi.SiC.Web.Controllers {    
    public class PreExistingController : BaseController
    {
        #region Public Methods

        public ActionResult Index()
        {
            var model = new PreExistingViewModel();
            model.SearchModel = new Models.Case.SearchViewModel();

            var searchController = new SearchController();
            model.SearchModel.Regions = searchController.GetAllRegions().ToList();


            var allCompensationRegions = CompensationRegionService.GetAllCompensationRegions();
            var groupedCompensationRegions = CompensationRegionService.GetCompensationRegionsGroupedByRegion();

            //model.transactionDetailModel = transactionDetailController.GetCompensationRegionModel();

            return View(model);
        }

        [HttpPost]
        public JsonResult AddPreexistingCase(string CaseNumber, string PreexistingCaseNumber)
        {
            string Status;
            if (CaseService.AddPreexistingCase(CaseNumber, PreexistingCaseNumber))
            {
                Status = "OK";
            }
            else
            {
                Status = "Error";
            }

            var result = new Dictionary<string, string>();

            result["CaseNumber"] = (CaseNumber);
            result["PreexistingCaseNumber"] = (PreexistingCaseNumber);
            result["Status"] = (Status);

            return Json(new BasicDataTablesResult(result));
        }

        [HttpPost]
        public JsonResult RemovePreexistingCase(string CaseNumber, string PreexistingCaseNumber, string Status)
        {
            if (CaseService.RemovePreexistingCase(CaseNumber, PreexistingCaseNumber)) {
                Status = "OK";
            } else {
                Status = "Error";
            }

            var result = new Dictionary<string, string>();

            result["CaseNumber"] = (CaseNumber);
            result["PreexistingCaseNumber"] = (PreexistingCaseNumber);
            result["Status"] = (Status);

            return Json(new BasicDataTablesResult(result));
        }

        [HttpPost]
        public JsonResult GetTotalAdjudicationAmount(int CaseId=0)
        {

            decimal? AdjudicationAmount = 0;
            decimal? OtherAdjudicationAmount = 0;
            if (CaseId != 0)
            {
               AdjudicationAmount = TransactionService.GetTotalAdjudicationByCase(CaseId);
               OtherAdjudicationAmount = TransactionService.GetTotalAdjudicationByOtherCases(CaseId);

            }
            
            var AdjudicationIW  = AdjudicationAmount + OtherAdjudicationAmount;

            Dictionary<object,object> results = new Dictionary<object,object>();

            results["AdjudicationAmount"] = AdjudicationAmount;
            results["OtherAdjudicationAmount"] = OtherAdjudicationAmount;
            results["AdjudicationIW"] = AdjudicationIW;

            var j = Json(new BasicDataTablesResult(results));

            return j;
        }

        [HttpPost]
        public JsonResult GetTransaction(int CaseId)
        {
           // var trans = TransactionService.FindTransactionsByCaseId(caseId);

            var trans = TransactionService.SearchTransactionsByCaseIdWithEffect(CaseId,"+");

            var results = trans.Select(r => new
            {
                TransactionDate = !r.TransactionDate.IsNull() ? ((DateTime)r.TransactionDate).ToString("yyyy-MM-dd") : "N/A",
                TransactionType = !r.TransactionType.TransactionType1.IsNull() ? r.TransactionType.TransactionType1 : "Desconocido",
                TransactionAmount = !r.TransactionAmount.IsNull() ? r.TransactionAmount : 0,
                TransactionId = r.TransactionId,
                CaseId = r.CaseDetail.CaseId,
                CaseNumber = r.CaseDetail.CaseNumber
            });

            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTransactionDetail(int TransactionId)
        {
            var trans = TransactionService.GetTransactionDetailById(TransactionId);

            var results = trans.Select(r => new
            {
                TransactionDetailId = r.TransactionDetailId,
                Amount = r.Amount,
                Percent = r.Percent,
                CompensationRegionId = r.CompensationRegionId,
            });

            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);

            //Case caso = CaseService.find
            //var trans = TransactionService.transa
            //var model = new Models.PaymentRegistration.IndexViewModel();
            ////model.PaymentRegistration = new IndexViewModel();
            //var allCompensationRegions = CompensationRegionService.GetAllCompensationRegions();
            //var groupedCompensationRegions = CompensationRegionService.GetCompensationRegionsGroupedByRegion();
            //
            //model.CompensationRegions = allCompensationRegions.ToSelectList(c => c.Code, c => string.Format("{0}|{1}|{2}|{3}|{4}", c.Code, c.Region, c.SubRegion, c.Weeks, c.CompensationRegionId), null, "Seleccionar");
            //model.GroupedCompensationRegions = groupedCompensationRegions.ToSelectList(c => c.Region, c => c.Region, null, "Seleccionar");
            //
            //
            ////model.CaseModel. = caso;
            //
            ////model.CaseModel = CaseService.in  ;
            //
            //return PartialView("_DesgloseIPP");
        }

        [HttpPost]
        public JsonResult SearchRelatedCases(string CaseNumber)
        {
            IEnumerable<CaseDetail> Casos = CaseService.FindRelatedCasesDetailByCaseNumber(CaseNumber,"00");

            IEnumerable<Case> UsedInDecision = CaseService.FindRelatedCasesUsedInDecision(CaseNumber);

            //var results = Casos.Select(r => new
            //{
            //    CaseNumber = r.CaseNumber,
            //    FullName = r.Entity.FullName,
            //    SSN = r.Entity.SSN,
            //    Adjudication = TransactionService.GetTotalAdjudicationByCase(r.CaseId),
            //    CaseId = r.CaseId,
            //    IsTheMaster = r.CaseNumber == CaseNumber
            //});
            //
            //return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);

            var result = new  List<Dictionary<string, object>>();

            foreach (var c in Casos)
            {
                var r = new Dictionary<string, object>();

                r["CaseNumber"] = c.CaseNumber;
                r["FullName"] = c.Entity.FullName;
                r["SSN"] = c.Entity.SSN;
                r["Adjudication"] = TransactionService.GetTotalAdjudicationByCase(c.CaseId);
                r["CaseId"] = c.CaseId;
                //r["IsTheMaster"] = c.CaseNumber == CaseNumber;

                r["HideDelete"] = false;
                if (c.CaseNumber == CaseNumber)
                {
                    r["HideDelete"] = true;
                }
                else
                {
                    foreach (var u in UsedInDecision)
                    {
                        if (u.CaseNumber == c.CaseNumber)
                        {
                            r["HideDelete"] = true;
                        }
                    }
                }

                result.Add(r);
            }
            
            return Json(new BasicDataTablesResult(result));
        }

        [HttpPost]
        public JsonResult SearchOtherRelatedCases(string CaseNumber)
        {
            IEnumerable<CaseDetail> Casos = CaseService.FindOtherRelatedCasesDetailByCaseNumber(CaseNumber,"00");

            var results = Casos.Select(r => new
            {
                CaseNumber = r.CaseNumber,
                FullName = r.Entity.FullName,
                SSN = r.Entity.SSN,
                Adjudication = TransactionService.GetTotalAdjudicationByCase(r.CaseId),
                CaseId = r.CaseId
            });

            return Json(new BasicDataTablesResult(results));
        }

        [HttpPost]
        public JsonResult SearchForOtherRelatedCases(DataTableParamCase param, string CurrentCaseNumber)
        {

            IEnumerable<CaseDetail> RelatedCases = CaseService.FindRelatedCasesDetailByCaseNumber(CurrentCaseNumber,"00");

            IEnumerable<CaseDetail> Cases = CaseService.SearchCaseDetails(param.CaseNumber, param.CaseKey, param.EntityName, param.SocialSecurityNumber, param.BirthDate, param.FilingDate, param.RegionId, param.ClinicId, param.EBTNumber)
                .ToPagedList(param.Start + 1, param.Length);

            var results = Cases.Select(r => new
            {
                CaseNumber = r.CaseNumber,
                FullName = r.Entity.FullName,
                SSN = r.Entity.SSN,
                Birthdate = r.Entity.BirthDate,
                EBT = r.EBTAccount,
                Adjudication = TransactionService.GetTotalAdjudicationByCase(r.CaseId),
                CaseId = r.CaseId,
                IsRelated = RelatedCases.Contains(r) 
            });

            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);

            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompensationRegionCode()
        {
            var compensation = TransactionService.GetCompensationRegion();

            var result = new Dictionary<String, Dictionary<String, String>>();
            var regions = new SortedDictionary<String, String>();
            var regionsid = new Dictionary<String, String>();
            var weeks = new Dictionary<String, String>();
            var codes = new Dictionary<String, String>();

            foreach (var c in compensation)
            {
                var re = c.Region.ToString();
                if (!result.ContainsKey(re)){
                    result.Add(re,null);
                    regions.Add(re,re);
                }
            }
            
            foreach(var c in compensation){

                var re = c.Region.ToString();
                var co = c.CodeDescription.ToString();
                var id = c.CompensationRegionId.ToString();
                var week = c.Weeks.ToString();

                var code = new Dictionary<String, String>();

                foreach(var c2 in compensation){
                    if (re == c2.Region.ToString())
                        code.Add(c2.CompensationRegionId.ToString(),c2.CodeDescription);
                }

                if (result[re] == null)
                    result[re] = code;

                weeks.Add(id, week);
                codes.Add(id, co);
                regionsid.Add(id, re);
            }

            result.Add("Regions", regions.ToDictionary(x => x.Key, x=> x.Value));
            result.Add("RegionsId", regionsid);
            result.Add("Weeks", weeks);
            result.Add("Codes", codes);

            return Json(new BasicDataTablesResult(result), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetTransactionDetail(IEnumerable<TransactionDetail2> json)
        {
            var toUpdate = new List<TransactionDetail2>();  
            var toInsert = new List<TransactionDetail2>();
            
            int TrasactionId = json.ElementAt<TransactionDetail2>(0).TransactionId ?? default(int);
            var trans = TransactionService.GetTransactionDetailById(TrasactionId);

            foreach (var j in json)
            {
                if (j.TransactionDetailId == 0)
                {
                    toInsert.Add(j);
                }
                else
                {
                    bool update = false;
                    int last = trans.Last().TransactionDetailId;
                    foreach (var t in trans)
                    {
                        j.Hidden = true;
                        if (j.TransactionDetailId == t.TransactionDetailId)
                        {
                            if (j.CompensationRegionId != t.CompensationRegionId || j.Amount != t.Amount || j.Percent != t.Percent)
                            {
                                update = true;
                            }
                            j.Hidden = false;
                            break;
                        }
                    }
                    if (j.Hidden == true || update)
                    {
                        toUpdate.Add(j);
                    }
                }
            }
            foreach (var t in trans)
            {
                bool Hidden = true;
                foreach (var j in json)
                {
                    if (j.TransactionDetailId == t.TransactionDetailId)
                    {
                        Hidden = false;
                    }
                }
                if (Hidden)
                {
                    t.Hidden = true;
                    toUpdate.Add(t);
                }
                
            }

            if(toInsert.Count > 0){
                var r = TransactionService.InsertTransactionDetail(toInsert);
            }

            if (toUpdate.Count > 0)
            {
                var r = TransactionService.UpdateTransactionDetail(toUpdate);
            }

            var result = new Dictionary<string, string>();
            result.Add("Status", "Ok");
            return Json(new BasicDataTablesResult(result));
        }

        [HttpPost]
        public JsonResult SearchRelatedCasesByCompensationRegion(string CaseNumber)
        {
            IEnumerable<RelatedCasesByCompensationRegion> Casos = CaseService.FindRelatedCasesByCompensationRegion(CaseNumber);

            var results = Casos.Select(r => new
            {
                CaseNumber = r.CaseNumber,
                CaseKey = r.CaseKey,
                CaseId = r.CaseId,
                CaseDetailId = r.CaseDetailId,
                TransactionId = r.TransactionId,
                TransactionDetailId = r.TransactionDetailId,
                Region = r.Region,
                Code = r.Code,
                Percent = r.Percent,
                Amount = r.Amount,
                SubRegion = r.SubRegion
            });

            return Json(new BasicDataTablesResult(results), JsonRequestBehavior.AllowGet);
        }





        //ToDelete

        [HttpGet]
        [Route("Preexisting/Case/{CaseNumber}")]
        public ActionResult Case(string CaseNumber)
        {
            var model = new PreExistingViewModel();

            //model.CaseModel.Case = CaseService.FindCaseDetailByNumber(CaseNumber);
            //model.TotalAdjudicationAmount = TransactionService.GetBalanceByCase(model.CaseModel.Case.CaseId);



            // model.SearchModel = new SearchViewModel();
            //
            //model.CaseModel.Case = CaseService.FindCaseByNumber(CaseNumber);
            //
            // model.TotalAdjudicationAmount = (decimal)TransactionService.GetTotalAdjudicationByCase(model.CaseModel.Case.CaseId);
            //
            // var transactionDetailController = new TransactionDetailController();
            // model.transactionDetailModel = transactionDetailController.GetCompensationRegionModel();

            return View(model);
        }

        //Old Code
        //[HttpPost]
        //public JsonResult SetTransaction(IEnumerable<TransactionRepository> json)
        //{
        //    foreach (TransactionRepository j in json)
        //    {
        //        j.TransactionAmount = 1;
        //    }
        //    var result = new Dictionary<string, string>();
        //    result.Add("Status", "Ok");
        //    return Json(new BasicDataTablesResult(result));
        //}

        //public JsonResult GetTransactionDetail(IEnumerable<TransactionRepository> json)
        //{
        //    foreach (TransactionRepository j in json)
        //    {
        //        j.TransactionAmount = 1;
        //    }
        //    var result = new Dictionary<string, string>();
        //    result.Add("Status", "Ok");
        //    return Json(new BasicDataTablesResult(result));
        //}



        #endregion
    }
}
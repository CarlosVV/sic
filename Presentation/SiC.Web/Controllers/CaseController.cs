namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CDI.WebApplication.DataTables.Result;
    using Infrastructure.Core.Helpers;
    using Domain.Core.Model;
    using Infrastructure.Web.Controllers;
    using Models.Case;
    using Models.Paging;
    using X.PagedList;

    #endregion
    
    public class CaseController : BaseController
    {
        #region Actions

        [HttpGet]
        public ActionResult SearchBox()
        {
            var model = new SearchViewModel();

            return PartialView("~/Views/Case/_SearchBox.cshtml", model.Rebuild(model));
        }

        [HttpPost]
        public JsonResult Search(DataTableParamCase param)
        {
            var cases = CaseService.SearchCaseDetails(param.CaseNumber, param.CaseKey, param.EntityName, param.SocialSecurityNumber, param.BirthDate, param.FilingDate, param.RegionId, param.ClinicId, param.EBTNumber)
                .OrderBy(m=>m.CaseId)                   
                .ToPagedList(param.Start + 1, param.Length);

            var data = CaseViewModel.CreateFrom(cases).ToList();

            var result = new DataTableResult<CaseViewModel>
            {
                draw = param.Draw,
                data = data,
                recordsFiltered = cases.Count,
                recordsTotal = cases.TotalItemCount
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("case/GetBalance/{caseDetailId}")]
        public JsonResult GetBalance(int caseDetailId)
        {
            var balance = TransactionService.GetBalanceByCase(caseDetailId);
            
            return Json(balance.GetValueOrDefault(decimal.Zero), JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        [Route("case/GetCaseDetailById/{caseDetailId}")]
        public JsonResult GetCaseDetailById(int caseDetailId)
        {
            var caseDetail = CaseService.FindCaseDetailById(caseDetailId);
            var caseMain = CaseService.FindCaseDetailByIdAndKey(caseDetail.CaseId.Value, "00");

            var balanceBeneficiario = TransactionService.GetBalanceByCase(caseDetailId);
            var balanceLesionado = TransactionService.GetBalanceByCase(caseMain.CaseDetailId);

            var model = CaseViewModel.CreateFrom(caseDetail, balanceBeneficiario.GetValueOrDefault(decimal.Zero), caseMain, balanceLesionado.GetValueOrDefault(decimal.Zero));

            return Json(new BasicDataTablesResult(model), JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpGet]
        public ActionResult Index()
        {
            return View("Index", "_CaseLayout");
        }
        
        [HttpGet]
        public ActionResult Resumen(int? caseId)
        {
            InformacionCaso_Result model = CaseService.InformacionCaso(caseId).First();

            ViewBag.caseFolderId = caseId;

            ViewBag.caseNumber = model.CaseNumber;

            return View("Resumen", "_CaseLayout", model);
        }

        [HttpGet]
        public ActionResult ResumenTab(int? id)
        {

            //CaseWebservice.WsSessionEJBEndPointClient service = new CaseWebservice.WsSessionEJBEndPointClient();

            //service.ClientCredentials.UserName.UserName  = "SONORA";
            //service.ClientCredentials.UserName.Password = "ExpCASE2011";

            //var query = new doQueryByName();
            //query.String_1 = "GetCaseNumberByCaseId";
            
            //FieldPropertiesTO props = new FieldPropertiesTO();
            //props.stringValue = id;

            //query.arrayOfFieldPropertiesTO_2 = new FieldPropertiesTO[1];
            //query.arrayOfFieldPropertiesTO_2[0] = props;

            //FmsRowSetTO result = service.doQueryByName(query)[0];

            //var caseFolderId = result.resultRows.First().fieldList[0].stringValue;
            try
            {


                //String caseNumber = sif.GetCaseNumber(Int32.Parse(id)).First();
              

                InformacionCaso_Result datos = CaseService.InformacionCaso(id).First();

                datos.ReportLink = String.Format(SettingService.GetSettingValueByName("PaymentHistory.ReportLink"), datos.CaseNumber);
                
                if (!datos.IsNull())
                {
                    ViewBag.caseFolderId = id;
                    ViewBag.caseNumber = datos.CaseNumber;

                    return View("ResumenTab", "_CaseLayout", datos);
                }
                else
                {
                    ViewBag.caseFolderId = id;
                    return View("_SinDatos", "_CaseLayout");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ViewBag.caseFolderId = id;
                return View("_SinDatos", "_CaseLayout");
            }
        }

        [HttpGet]
        [Route("Case/CertificationTab/{caseFolderId}")]
        public ActionResult CertificationTab(string caseFolderId)
        {
            var model = new Nagnoi.SiC.Web.Models.PaymentCertification.CertificationViewModel();

            int id = Convert.ToInt32(caseFolderId);
            InformacionCaso_Result caseData = CaseService.InformacionCaso(id).First();

            ViewBag.CaseNumber = caseData.CaseNumber;
            ViewBag.CaseKey = null;
            
            model.Status = GetAllStatus();
            model.Concept = GetAllConcept();
            return View("CertificationTab", "_CaseLayout", model);
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
    }
}
namespace Nagnoi.SiC.Web.Controllers
{
    #region References

    using System.Web.Mvc;
    using Infrastructure.Web.Controllers;
    using Domain.Core.Model;
    using Models.Adjustments;
    using System.Linq;
    using System.Collections;
    using Infrastructure.Core.Helpers;
    using CDI.WebApplication.DataTables.Result;
    using System;
    using Nagnoi.SiC.Infrastructure.Web.ViewModels;
    #endregion
    
    public class AdjustmentsController : BaseController
    {
        #region actions
        [HttpGet]
        public ActionResult Index()
        {
            var adjustmentReasons = TransactionService.GetAllAdjustmentReasons();
            var transactionTypes = TransactionTypeService.GetTransactionTypes();
            var cancellation = CaseService.GetAllCancellation();
            
            var model = new IndexViewModel();

            return View(model.Rebuild(model, adjustmentReasons, transactionTypes, cancellation));
        }

        [HttpPost]
        public ActionResult Save(IndexViewModel model)
        {
            var caseDetail = CaseService.FindCaseDetailById(model.CaseDetailId);
            CaseDetailViewModel cdv = new CaseDetailViewModel();
           
            var esMuerte = caseDetail.Case.ConceptId == ConceptService.GetAllConcepts().Where(p => p.ConceptCode == ((int)PaymentConceptEnum.Muerte).ToString()).FirstOrDefault().ConceptId ? true: false;
            var esITP = caseDetail.Case.ConceptId == ConceptService.GetAllConcepts().Where(p => p.ConceptCode == ((int)PaymentConceptEnum.ITP).ToString()).FirstOrDefault().ConceptId ? true : false;
            var caseMain = CaseService.FindCaseDetailByIdAndKey(caseDetail.CaseId.Value, caseDetail.CaseKey);
            var adjustmentType = (AdjustmentTypeEnum) model.AdjustmentTypeId;

            switch (adjustmentType)
            {
                case AdjustmentTypeEnum.AjusteMensualidad:
                    CreateTransactionAjusteMensualidad(model, caseDetail, caseMain);                   
                    break;
                case AdjustmentTypeEnum.AjusteReserva:
                     CreateTransactionAjusteReserva(model, caseDetail, esMuerte, caseMain);                   
                    break;
                case AdjustmentTypeEnum.Reevaluacion:
                    CreateTransactionReevaluacion(model, caseDetail, esMuerte, caseMain);
                    break;
                case AdjustmentTypeEnum.Cancelacion:
                    UpdateCancellationCaseDetail(model, caseDetail);
                    break;
                case AdjustmentTypeEnum.Cierre:
                    CreateTransactionCierreBalanceMuerteItp(model, caseDetail, esMuerte, caseMain);
                    break;
                case AdjustmentTypeEnum.Reanudacion:
                    ResumeCaseDetail(model, caseDetail);
                    break;
                default:
                    break;
            }

            return Json(new BasicDataTablesResult(new { Status = "OK" }));
        }

        
        #endregion

        #region private
        private void CreateTransactionCierreBalanceMuerteItp(IndexViewModel model, CaseDetail caseDetail, bool esMuerte, CaseDetail caseMain)
        {
            Transaction sicTransactionCierre = new Transaction
            {
                CaseDetailId = caseDetail.CaseDetailId,
                TransactionTypeId = esMuerte ? ((int)SimeraTransactionEnum.CierreBalanceMuerte) : ((int)SimeraTransactionEnum.CierreBalanceITP),
                TransactionAmount = 0,
                TransactionDate = DateTime.UtcNow,
                ConceptId = caseDetail.Case.ConceptId.Value,
                DecisionDate = DateTime.UtcNow,
                AdjustmentReasonId = model.AdjustmentReasonId
            };

            SimeraTransaction transactionCierre = new SimeraTransaction
            {
                CaseDetailId = caseDetail.CaseDetailId,
                CaseKey = caseDetail.CaseKey,
                CaseNumber = caseDetail.CaseNumber,
                SSN = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.SSN : caseDetail.Entity.SSN,
                FullName = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.FullName : caseDetail.Entity.FullName,
                BirthDate = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.BirthDate : caseDetail.Entity.BirthDate,
                SSN_Beneficiary = !caseDetail.CaseKey.Equals("00") ? caseDetail.Entity.SSN : string.Empty,
                FullName_Beneficiary = !caseDetail.CaseKey.Equals("00") ? caseDetail.Entity.FullName : string.Empty,
                Relationship = !caseDetail.CaseKey.Equals("00") ? caseDetail.RelationshipType.RelationshipType1 : string.Empty,
                Date = System.DateTime.UtcNow,
                AdjustmentReason = model.AdjustmentReasonText,
                TransactionType = esMuerte ? ((int)SimeraTransactionEnum.CierreBalanceMuerte).ToString() : ((int)SimeraTransactionEnum.CierreBalanceITP).ToString(),
                RequestCreatedByUser = WebHelper.GetUserName(),
                StatusSIC = "S",
                CreatedBy = "SIC Web",
                CreatedDateTime = System.DateTime.UtcNow
            };

            SimeraTransactionService.InsertTransactions(transactionCierre, sicTransactionCierre);
        }

        private void UpdateCancellationCaseDetail(IndexViewModel model, CaseDetail caseDetail)
        {
            caseDetail.CancellationId = model.CancellationId.Value;
            caseDetail.CancellationDate = model.FromDate;
            caseDetail.ModifiedBy = WebHelper.GetUserName();
            caseDetail.ModifiedDateTime = System.DateTime.UtcNow;
            caseDetail.ActiveIdent = "I";

            CaseService.UpdateCaseDetail(caseDetail);
        }
        private void ResumeCaseDetail(IndexViewModel model, CaseDetail caseDetail)
        {
            caseDetail.CancellationId = model.CancellationId.Value;
            caseDetail.RestartDate = model.FromDate;
            caseDetail.CancellationDate = model.FromDate;
            caseDetail.ModifiedBy = WebHelper.GetUserName();
            caseDetail.ModifiedDateTime = System.DateTime.UtcNow;
            caseDetail.ActiveIdent = "B";

            CaseService.UpdateCaseDetail(caseDetail);
        }
        private void CreateTransactionReevaluacion(IndexViewModel model, CaseDetail caseDetail, bool esMuerte, CaseDetail caseMain)
        {
            Transaction sicTransactionAjusteReevaluacion= new Transaction
            {
                CaseDetailId = caseDetail.CaseDetailId,
                TransactionTypeId = esMuerte ? ((int)SimeraTransactionEnum.ReEvaluacionMuerte) : ((int)SimeraTransactionEnum.ReEvaluacionITP),
                TransactionAmount = 0,
                TransactionDate = DateTime.UtcNow,
                ConceptId = caseDetail.Case.ConceptId.Value,
                DecisionDate = DateTime.UtcNow,
                AdjustmentReasonId = model.AdjustmentReasonId
            };

            SimeraTransaction transactionReevaluacion = new SimeraTransaction
            {
                CaseDetailId = caseDetail.CaseDetailId,
                CaseKey = caseDetail.CaseKey,
                CaseNumber = caseDetail.CaseNumber,
                SSN = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.SSN : caseDetail.Entity.SSN,
                FullName = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.FullName : caseDetail.Entity.FullName,
                BirthDate = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.BirthDate : caseDetail.Entity.BirthDate,
                SSN_Beneficiary = !caseDetail.CaseKey.Equals("00") ? caseDetail.Entity.SSN : string.Empty,
                FullName_Beneficiary = !caseDetail.CaseKey.Equals("00") ? caseDetail.Entity.FullName : string.Empty,
                Relationship = !caseDetail.CaseKey.Equals("00") ? caseDetail.RelationshipType.RelationshipType1 : string.Empty,
                Date = System.DateTime.UtcNow,
                AdjustmentReason = model.AdjustmentReasonText,
                TransactionType = esMuerte ? ((int)SimeraTransactionEnum.ReEvaluacionMuerte).ToString() : ((int)SimeraTransactionEnum.ReEvaluacionITP).ToString(),
                RequestCreatedByUser = WebHelper.GetUserName(),
                StatusSIC = "S",
                CreatedBy = "SIC Web",
                CreatedDateTime = System.DateTime.UtcNow
            };

            SimeraTransactionService.InsertTransactions(transactionReevaluacion, sicTransactionAjusteReevaluacion);
        }

        private void CreateTransactionAjusteReserva(IndexViewModel model, CaseDetail caseDetail, bool esMuerte, CaseDetail caseMain)
        {
            Transaction sicTransactionAjusteReserva = new Transaction
            {
                CaseDetailId = caseDetail.CaseDetailId,
                TransactionTypeId = (int)TransactionTypeEnum.AjusteReserva,
                TransactionAmount = model.CurrentBalance.Value,
                TransactionDate = DateTime.UtcNow,
                ConceptId = caseDetail.Case.ConceptId.Value,
                DecisionDate = DateTime.UtcNow,
                AdjustmentReasonId = model.AdjustmentReasonId
            };

            SimeraTransaction transactionAjusteReserva = new SimeraTransaction
            {
                CaseDetailId = caseDetail.CaseDetailId,
                CaseKey = caseDetail.CaseKey,
                CaseNumber = caseDetail.CaseNumber,
                SSN = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.SSN : caseDetail.Entity.SSN,
                FullName = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.FullName : caseDetail.Entity.FullName,
                BirthDate = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.BirthDate : caseDetail.Entity.BirthDate,
                SSN_Beneficiary = !caseDetail.CaseKey.Equals("00") ? caseDetail.Entity.SSN : string.Empty,
                FullName_Beneficiary = !caseDetail.CaseKey.Equals("00") ? caseDetail.Entity.FullName : string.Empty,
                Relationship = !caseDetail.CaseKey.Equals("00") ? caseDetail.RelationshipType.RelationshipType1 : string.Empty,
                Date = System.DateTime.UtcNow,
                AdjustmentReason = model.AdjustmentReasonText,
                ReserveAdjustment = model.CurrentBalance.Value,
                TransactionType = esMuerte ? ((int)SimeraTransactionEnum.AjusteReservaMuerte).ToString() : ((int)SimeraTransactionEnum.AjusteReservaITP).ToString(),
                RequestCreatedByUser = WebHelper.GetUserName(),
                StatusSIC = "S",
                CreatedBy = "SIC Web",
                CreatedDateTime = System.DateTime.UtcNow
            };

            SimeraTransactionService.InsertTransactions(transactionAjusteReserva, sicTransactionAjusteReserva);
        }

        private void CreateTransactionAjusteMensualidad(IndexViewModel model, CaseDetail caseDetail, CaseDetail caseMain)
        {
            Transaction sicTransactionAjusteMensualidad = new Transaction
            {
                CaseDetailId = caseDetail.CaseDetailId,
                TransactionTypeId = (int)SimeraTransactionEnum.AjustesMensualidad,
                TransactionAmount = 0,
                TransactionDate = DateTime.UtcNow,
                ConceptId = caseDetail.Case.ConceptId.Value,
                DecisionDate = DateTime.UtcNow,
                AdjustmentReasonId = model.AdjustmentReasonId
            };

            SimeraTransaction transactionAjusteMensualidad = new SimeraTransaction
            {
                CaseDetailId = caseDetail.CaseDetailId,
                CaseKey = caseDetail.CaseKey,
                CaseNumber = caseDetail.CaseNumber,
                SSN = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.SSN : caseDetail.Entity.SSN,
                FullName = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.FullName : caseDetail.Entity.FullName,
                BirthDate = !caseDetail.CaseKey.Equals("00") ? caseMain.Entity.BirthDate : caseDetail.Entity.BirthDate,
                SSN_Beneficiary = !caseDetail.CaseKey.Equals("00") ? caseDetail.Entity.SSN : string.Empty,
                FullName_Beneficiary = !caseDetail.CaseKey.Equals("00") ? caseDetail.Entity.FullName : string.Empty,
                Relationship = !caseDetail.CaseKey.Equals("00") ? caseDetail.RelationshipType.RelationshipType1 : string.Empty,
                Date = System.DateTime.UtcNow,
                AdjustmentReason = model.AdjustmentReasonText,
                MonthlyAdjustment = model.MonthlyInstallment.Value,
                TransactionType = ((int)SimeraTransactionEnum.AjustesMensualidad).ToString(),
                RequestCreatedByUser = WebHelper.GetUserName(),
                StatusSIC = "S",
                CreatedBy = "SIC Web",
                CreatedDateTime = System.DateTime.UtcNow
            };           

            SimeraTransactionService.InsertTransactions(transactionAjusteMensualidad, sicTransactionAjusteMensualidad);
        }
        #endregion

        #region enums
        public enum AdjustmentTypeEnum
        {
            AjusteMensualidad = 1,
            AjusteReserva = 2,
            Reevaluacion = 3,
            Cancelacion = 4,
            Cierre = 5,
            Reanudacion = 6
        }
        #endregion
    }
}
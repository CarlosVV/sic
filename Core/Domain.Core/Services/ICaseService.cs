namespace Nagnoi.SiC.Domain.Core.Services
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Core.Model;

    #endregion

    public interface ICaseService
    {
        #region Cases
        
        IEnumerable<Case> SearchCases(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt);

        Task<List<Case>> SearchCasesAsync(string caseNumber, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt);

        Task<List<CaseDetail>> SearchCaseDetailsAsync(string caseNumber, string caseKey, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt);

        IEnumerable<CaseDetail> SearchCaseDetails(string caseNumber, string caseKey, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt);

        Case FindCaseByNumber(string caseNumber);

        CaseDetail FindCaseDetailByNumber(string caseNumber);

        CaseDetail FindCaseDetailByNumber(string caseNumber, string caseKey);

        Task<Case> FindCaseByNumberAsync(string caseNumber);

        CaseDetail FindCaseDetailById(int caseDetailId);

        Task<CaseDetail> FindCaseDetailByIdAsync(int caseDetailId);

        IEnumerable<Case> FindRelatedCasesByCaseNumber(string caseNumber);

        IEnumerable<CaseDetail> FindRelatedCasesDetailByCaseNumber(string caseNumber,string caseKey);

        IEnumerable<Case> FindOtherRelatedCasesByCaseNumber(string caseNumber);

        IEnumerable<CaseDetail> FindOtherRelatedCasesDetailByCaseNumber(string caseNumber, string caseKey);

        IEnumerable<RelatedCasesByCompensationRegion> FindRelatedCasesByCompensationRegion(string caseNumber);

        bool AddPreexistingCase(string CaseNumber, string PreexistingCaseNumber);

        bool RemovePreexistingCase(string CaseNumber, string PreexistingCaseNumber);
        
        #endregion

        IEnumerable<BuscarCasos_Result> BuscarCasos(string nombre, string sSN, string eBT, string numeroCaso);

        IEnumerable<InformacionCaso_Result> InformacionCaso(int? caseId);

        IEnumerable<ResumenPagosPorBeneficiario_Result> ResumenPagosPorBeneficiario(string caseNumber);

        IEnumerable<ResumenPagosPorConcepto_Result> ResumenPagosPorConcepto(string caseNumber);
        
        IEnumerable<CaseDetail> FindRelatives(int caseId);

        IEnumerable<CaseDetail> FindCasesInDormantExpunged();

        IEnumerable<CaseDetail> GetCaseBeneficiaries(string caseNumber);

        CaseDetail FindCaseDetailByIdAndKey(int caseId, string key);

        void UpdateCaseDetail(CaseDetail caseDetail);

        IEnumerable<Cancellation> GetAllCancellation();

        IEnumerable<Case> FindRelatedCasesUsedInDecision(string caseNumber);

        CaseDetailDemographic GetEntityPriority(int caseDetailId);

        CaseDetail FindInjuredDetail(int casedetailId, int entityId);
    }
}
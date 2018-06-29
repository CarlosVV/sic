namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Core.Model;

    #endregion

    public interface ICaseDetailRepository : IRepository<CaseDetail>
    {
        Task<List<CaseDetail>> SearchCaseDetailsAsync(string caseNumber, string caseKey, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt);

        IEnumerable<CaseDetail> SearchCaseDetails(string caseNumber, string caseKey, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt);

        CaseDetail GetById(int caseDetailId);

        Task<CaseDetail> GetByIdAsync(int caseDetailId);

        IEnumerable<CaseDetail> FindRelatives(int caseId);

        IEnumerable<CaseDetail> FindCasesDormantExpunged();

        CaseDetail GetCaseByNumber(string caseNumber);

        CaseDetail GetCaseByNumber(string caseNumber, string caseKey);

        IEnumerable<CaseDetail> GetCaseBeneficiariesByCaseNumber(string caseNumber);

        IEnumerable<CaseDetail> GetRelatedCasesDetailByCaseNumber(string caseNumber, string caseKey);

        IEnumerable<CaseDetail> GetOtherRelatedCasesDetailByCaseNumber(string caseNumber, string caseKey);

        CaseDetail FindCaseDetailByIdAndKey(int caseId, string key);

        CaseDetailDemographic GetEntityPriorityDetail(int caseDetailId);

        CaseDetail GetInjuredDetail(int caseDetailId, int entityId);
    }
}
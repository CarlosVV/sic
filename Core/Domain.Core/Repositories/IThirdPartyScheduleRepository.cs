namespace Nagnoi.SiC.Domain.Core.Repositories
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;

    #endregion

    public interface IThirdPartyScheduleRepository : IRepository<ThirdPartySchedule>
    {
        ThirdPartySchedule GetById(int scheduleId);

        IEnumerable<ThirdPartySchedule> Find(int? scheduleId, int? remitterId, int? caseId);

        Task<List<ThirdPartySchedule>> FindAsync(int? scheduleId, int? remitterId, int? caseId);

        IEnumerable<ThirdPartySchedule> FindToApprove(string EBTNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromTransactionDate,
            DateTime? toTransactionDate, int? transactionTypeId, int? paymentStatusId, int? ConceptId);
    }
}
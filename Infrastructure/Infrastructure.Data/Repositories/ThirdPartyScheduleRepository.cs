namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Core.Model;
    using Domain.Core.Repositories;

    #endregion

    public sealed class ThirdPartyScheduleRepository : EfRepository<ThirdPartySchedule>, IThirdPartyScheduleRepository
    {
        public IEnumerable<ThirdPartySchedule> Find(int? scheduleId, int? remitterId, int? caseId)
        {
            return Find(t => (!scheduleId.HasValue || t.ThirdPartyScheduleId == scheduleId) &&
                             (!remitterId.HasValue || t.EntityId_RemitTo == remitterId) &&
                             (!caseId.HasValue || t.CaseId == caseId),
                        t => t.Remitter, t => t.Case);
        }

        public Task<List<ThirdPartySchedule>> FindAsync(int? scheduleId, int? remitterId, int? caseId)
        {
            return FindAsync(t => (!scheduleId.HasValue || t.ThirdPartyScheduleId == scheduleId) &&
                                  (!remitterId.HasValue || t.EntityId_RemitTo == remitterId) &&
                                  (!caseId.HasValue || t.CaseId == caseId),
                             t => t.Remitter, t => t.Case);
        }

        public ThirdPartySchedule GetById(int scheduleId)
        {
            return this.FindOne(t => t.ThirdPartyScheduleId == scheduleId,
                                t => t.Remitter, t => t.Case, t => t.CaseDetail,
                                t => t.CaseDetail.Case, t => t.CaseDetail.Entity);
        }

        public IEnumerable<ThirdPartySchedule> FindToApprove(string EBTNumber,
            string caseNumber, string entityName, string ssn,
            DateTime? birthDate, DateTime? filingDate, int? regionId,
            int? dispensaryId, DateTime? fromTransactionDate,
            DateTime? toTransactionDate, int? transactionTypeId, int? paymentStatusId, int? ConceptId)
        {
            var query = Entities.AsQueryable();

            if (!string.IsNullOrEmpty(caseNumber))
            {
                query = query.Where(t => t.CaseDetail.CaseNumber == caseNumber);
            }

            if (!string.IsNullOrEmpty(entityName))
            {
                query = query.Where(t => t.CaseDetail.Entity.FullName.Contains(entityName));
            }

            if (!string.IsNullOrEmpty(ssn))
            {
                query = query.Where(t => t.CaseDetail.Entity.SSN == ssn);
            }

            if (birthDate.HasValue)
            {
                query = query.Where(t => t.CaseDetail.Entity.BirthDate == birthDate.Value);
            }

            if (filingDate.HasValue)
            {
                query = query.Where(t => t.CaseDetail.Case.CaseDate == filingDate.Value);
            }

            if (regionId.HasValue)
            {
                query = query.Where(t => t.CaseDetail.Case.RegionId == regionId.Value);
            }

            if (dispensaryId.HasValue)
            {
                query = query.Where(t => t.CaseDetail.Case.ClinicId == dispensaryId.Value);
            }

            if (fromTransactionDate.HasValue)
            {
                query = query.Where(t => t.CreatedDateTime.Value > fromTransactionDate.Value);
            }

            if (toTransactionDate.HasValue)
            {
                query = query.Where(t => t.CreatedDateTime.Value < toTransactionDate.Value);
            }

            if (transactionTypeId.HasValue)
            {
                //query = query.Where(t => t.Payments.Value == transactionTypeId.Value);
            }

            if (paymentStatusId.HasValue)
            {
                query = query.Where(t => t.Payments.Any(p => p.StatusId == paymentStatusId.Value));
            }

            return query.AsNoTracking()
                        .Include(t => t.Remitter)
                        .Include(t => t.CaseDetail)
                        .Include(t => t.CaseDetail.Entity)
                        .Include(t => t.CaseDetail.EntityLawyer)
                        .Include(t => t.CaseDetail.RelationshipType)
                        .Include(t => t.Payments)
                        .Include(t => t.Payments.Select(p => p.Status));
                        //.Include(t => t.Concept);
                        //.Include(t => t.TransactionType);
        }
    }
}
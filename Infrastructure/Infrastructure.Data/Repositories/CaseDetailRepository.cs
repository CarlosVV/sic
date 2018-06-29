namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using System.Data.SqlClient;
    using Infrastructure.Core.Helpers;
    #endregion

    public sealed class CaseDetailRepository : EfRepository<CaseDetail>, ICaseDetailRepository
    {
        public CaseDetail GetById(int caseDetailId)
        {
            return this.FindOne(c => c.CaseDetailId == caseDetailId,
                                c => c.Entity,
                                c => c.Case,
                                c => c.Case.Region,
                                c => c.Case.Clinic,
                                c => c.Case.Concept,
                                c => c.Entity.Addresses,
                                c => c.Entity.CivilStatus,
                                c => c.Entity.ParticipantType,
                                c => c.Entity.ParticipantStatus,
                                c => c.Case.EmployerStatus,
                                c => c.Transactions,
                                c => c.ThirdPartySchedules,
                                c => c.RelationshipType);
        }

        public Task<CaseDetail> GetByIdAsync(int caseDetailId)
        {
            return this.FindOneAsync(c => c.CaseDetailId == caseDetailId,
                                     c => c.Entity,
                                     c => c.Case,
                                     c => c.Case.Region,
                                     c => c.Case.Clinic,
                                     c => c.Case.Concept,
                                     c => c.Entity.Addresses,
                                     c => c.Entity.CivilStatus,
                                     c => c.Entity.ParticipantType,
                                     c => c.Entity.ParticipantStatus,
                                     c => c.Case.EmployerStatus,
                                     c => c.Transactions,
                                     c => c.ThirdPartySchedules,
                                     c => c.RelationshipType);
        }

        public Task<List<CaseDetail>> SearchCaseDetailsAsync(string caseNumber, string caseKey, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt)
        {
            var query = Entities.AsQueryable();

            if (!string.IsNullOrEmpty(caseNumber))
            {
                query = query.Where(c => c.CaseNumber == caseNumber);
            }

            if (!string.IsNullOrEmpty(caseKey))
            {
                query = query.Where(c => c.CaseKey == caseKey);
            }

            if (!string.IsNullOrEmpty(injuredName))
            {
                query = query.Where(c => c.Entity.FullName.Contains(injuredName));
            }

            if (!string.IsNullOrEmpty(ssn))
            {
                query = query.Where(c => c.Entity.SSN == ssn);
            }

            if (dateOfBirth.HasValue)
            {
                query = query.Where(c => c.Entity.BirthDate == dateOfBirth);
            }

            if (caseDate.HasValue)
            {
                query = query.Where(c => c.Case.CaseDate == caseDate);
            }

            if (regionId.HasValue)
            {
                query = query.Where(c => c.Case.RegionId == regionId);
            }

            if (clinicId.HasValue)
            {
                query = query.Where(c => c.Case.ClinicId == clinicId);
            }

            if (!string.IsNullOrEmpty(accountEbt))
            {
                query = query.Where(c => c.EBTAccount == accountEbt);
            }

            return query.AsNoTracking()
                        .Include(c => c.Entity)
                        .Include(c => c.Case)
                        .Include(c => c.Case.Region)
                        .Include(c => c.Case.Clinic)
                        .Include(c => c.Case.EmployerStatus)
                        .Include(c => c.Case.Concept)
                        .Include(c => c.Entity.Addresses)
                        .Include(c => c.Transactions)
                        .Include(c => c.RelationshipType)
                        .Include(c => c.EntityLawyer)
                        .Include(c => c.EntityLegalGuardian)
                        .ToListAsync();
        }

        public IEnumerable<CaseDetail> SearchCaseDetails(string caseNumber, string caseKey, string injuredName, string ssn, DateTime? dateOfBirth, DateTime? caseDate, int? regionId, int? clinicId, string accountEbt)
        {
            if (!String.IsNullOrEmpty(ssn)) {
                if (ssn.IndexOf("-") > 0) {
                    ssn = ssn.Replace("-", "");
                }
            }

            var query = Entities.AsQueryable();

            if (!string.IsNullOrEmpty(caseNumber))
            {
                query = query.Where(c => c.CaseNumber == caseNumber);
            }

            if (!string.IsNullOrEmpty(caseKey))
            {
                query = query.Where(c => c.CaseKey == caseKey);
            }

            if (!string.IsNullOrEmpty(injuredName))
            {
                query = query.Where(c => c.Entity.FullName.Contains(injuredName));
            }

            if (!string.IsNullOrEmpty(ssn))
            {
                query = query.Where(c => c.Entity.SSN == ssn);
            }

            if (dateOfBirth.HasValue)
            {
                query = query.Where(c => c.Entity.BirthDate == dateOfBirth);
            }

            if (caseDate.HasValue)
            {
                query = query.Where(c => c.Case.CaseDate == caseDate);
            }

            if (regionId.HasValue)
            {
                query = query.Where(c => c.Case.RegionId == regionId);
            }

            if (clinicId.HasValue)
            {
                query = query.Where(c => c.Case.ClinicId == clinicId);
            }

            if (!string.IsNullOrEmpty(accountEbt))
            {
                query = query.Where(c => c.EBTAccount == accountEbt);
            }

            //query.AsNoTracking().Select(newModel => new {
            //    CaseId = newModel.CaseId,
            //    CaseDetailId = newModel.CaseDetailId,
            //    CaseNumber = newModel.CaseNumber,
            //    CaseKey = newModel.CaseKey,
            //    CaseDate = newModel.Case.CaseDate,
            //    AccidentDate = newModel.Case.AccidentDate,
            //    Region1 = newModel.Case.Region == null ? string.Empty : newModel.Case.Region.Region1,
            //    Clinic1 = newModel.Case.Clinic == null ? string.Empty : newModel.Case.Clinic.Clinic1,
            //    IsMinor = newModel.Case.IsMinor.GetValueOrDefault(false),
            //    DeathFlag = newModel.Case.DeathFlag.GetValueOrDefault(false),
            //    Concept1 = newModel.Case.Concept == null ? string.Empty : newModel.Case.Concept.Concept1,
            //    TienePerentorio = newModel.Transactions.Any(t => t.TransactionTypeId == 4),
            //    TieneInversionMenor3 = newModel.Transactions.Any(t => t.TransactionTypeId == 3 && (t.TransactionDate.HasValue && t.TransactionDate.Value.AddYears(3) > now)),
            //    FechaAccidenteMenor1984 = newModel.Case.AccidentDate.HasValue ? (newModel.Case.AccidentDate.Value < new DateTime(1984, 5, 30)) : false,
            //    TienePagoTercerosVigente = newModel.ThirdPartySchedules.Any(t => !t.TerminationDate.HasValue || (t .TerminationDate.Value < now)),
            //    DiasSemana = newModel.Case.DaysWeek.GetValueOrDefault(0),
            //    CompSemanal = newModel.Case.WeeklyComp.GetValueOrDefault(decimal.Zero),
            //    CompSemanalInca = newModel.Case.WeeklyCompDisability.GetValueOrDefault(decimal.Zero),
            //    FechaSuspension = newModel.CancellationDate.HasValue ? newModel.CancellationDate.Value.ToShortDateString() : string.Empty,
            //    RazonSuspension = newModel.Cancellation.IsNull() ? string.Empty : newModel.Cancellation.Cancellation1,
            //    FechaReanudacion = newModel.RestartDate.HasValue ? newModel.RestartDate.Value.ToShortDateString() : string.Empty,
            //    FromCase = newModel.CaseFolderId.HasValue ? true : false,
            //    CancellationDate = newModel.CancellationDate,
            //    Lesionado =
            //});

            return query.AsNoTracking()
                        .Include(c => c.Entity)
                        .Include(c => c.Case)
                        .Include(c => c.Case.Region)
                        .Include(c => c.Case.Clinic)
                        .Include(c => c.Case.EmployerStatus)
                        .Include(c => c.Case.Concept)
                        .Include(c => c.Entity.Addresses)
                        .Include(c => c.Transactions)
                        .Include(c => c.RelationshipType)
                        .Include(c => c.EntityLawyer)
                        .Include(c => c.EntityLegalGuardian)
                        .Include(c => c.EntitySic)
                        .Include(c => c.EntitySif);
        }

        public IEnumerable<CaseDetail> FindRelatives(int caseId)
        {
            return this.Find(c => c.CaseId == caseId && c.RelationshipTypeId != null,
                             c => c.Entity,
                             c => c.RelationshipType);
        }

        public CaseDetail FindCaseDetailByIdAndKey(int caseId, string key) {
            return this.Find(c => c.CaseId == caseId && c.CaseKey == key,
                             c => c.Entity,
                             c => c.RelationshipType,
                             c => c.EntitySif,
                             c => c.EntitySic).FirstOrDefault();
        }

        public IEnumerable<CaseDetail> FindCasesDormantExpunged(){
            var query = Entities.AsQueryable();

            //Filter to the the cases that are on Expunge or Dormant
            query = query.Where(c => c.EBTStatus == "Dormant" || c.EBTStatus == "Expunged");
            //query = query.Where(c => c.CaseDetailId == 101);

            //Bring the query home
            return query.Include(c => c.Case)
                        .Include(c => c.Case.Concept)
                        .Include(c => c.Case.Region)
                        .Include(c => c.Case.Clinic)
                        .Include(c => c.Entity);
        }

        public CaseDetail GetCaseByNumber(string caseNumber)
        {
            return FindOne(c => c.CaseNumber == caseNumber,
                           c => c.Entity, c => c.Case, c => c.Case.Region, c => c.Case.Clinic, c => c.Case.EmployerStatus, c => c.Entity.Addresses, c => c.Case.Concept, c => c.Entity.Addresses.Select(x => x.City), c => c.Entity.Addresses.Select(x => x.State));
        }

        public CaseDetail GetCaseByNumber(string caseNumber, string caseKey) {
            return FindOne(c => c.CaseNumber == caseNumber && c.CaseKey == c.CaseKey,
                           c => c.Entity, c => c.Case, c => c.Case.Region, c => c.Case.Clinic, c => c.Case.EmployerStatus, c => c.Entity.Addresses, c => c.Case.Concept, c => c.Entity.Addresses.Select(x => x.City), c => c.Entity.Addresses.Select(x => x.State));
        }

        public IEnumerable<CaseDetail> GetCaseBeneficiariesByCaseNumber(string caseNumber)
        {
            var query = Entities.AsQueryable();

            query = query.Where(c => c.CaseNumber == caseNumber);
            query = query.Where(c => c.Entity.ParticipantTypeId == 4);

            return query.Include(c => c.Entity)
                        .Include(c => c.Entity.ParticipantType)
                        .Include(c => c.RelationshipType);

        }

        public IEnumerable<CaseDetail> GetRelatedCasesDetailByCaseNumber(string caseNumber, string caseKey = "00")
        {

            List<string> casenumbers = this.Context.SqlQuery<string>(
                                                                    "EXEC [WebApp].[GetRelatedCasesByCaseNumber] @CaseNumber",
                                                                    new SqlParameter("CaseNumber", caseNumber)
                                                                 ).ToList();

            List<CaseDetail> cases = new List<CaseDetail>();
            foreach (string c in casenumbers)
            {
                cases.Add(GetCaseByNumber(c, caseKey));
            }

            return cases;
        }

        public IEnumerable<CaseDetail> GetOtherRelatedCasesDetailByCaseNumber(string caseNumber, string caseKey)
        {
            List<string> casenumbers = this.Context.SqlQuery<string>(
                                                                    "EXEC [WebApp].[GetOtherRelatedCasesByCaseNumber] @CaseNumber",
                                                                    new SqlParameter("CaseNumber", caseNumber)
                                                                 ).ToList();

            List<CaseDetail> cases = new List<CaseDetail>();
            foreach (string c in casenumbers)
            {

                cases.Add(GetCaseByNumber(c, caseKey));
            }

            return cases;
        }

        public CaseDetailDemographic GetEntityPriorityDetail(int caseDetailId)
        {
            CaseDetailDemographic detail = new CaseDetailDemographic();

            var caseDetailIdParameter = new SqlParameter()
            {
                ParameterName = "caseDetailId",
                Value = caseDetailId
            };

            var c = this.Context.SqlQuery<CaseDetailDemographic>("SELECT CaseDetailId, EntityId, EntityId_LegalGuardian, EntityId_Lawyer, RelationshipTypeId, RelationshipType, OtherRelationshipType, Source, Priority FROM [SiC DB].SiC.vCaseDetailDemographic WHERE CaseDetailId = @caseDetailId", caseDetailIdParameter).FirstOrDefault();

            return c;
        }

        public CaseDetail GetInjuredDetail(int caseDetailId, int entityId)
        {
            return this.FindOne(c => c.CaseDetailId == caseDetailId && c.EntityId == entityId,
                                c => c.Entity,
                                c => c.EntityLegalGuardian,
                                c => c.EntityLawyer,
                                c => c.Entity.Addresses,
                                c => c.Entity.Addresses.Select(d => d.AddressType),                                                                
                                c => c.Entity.CivilStatus,
                                c => c.RelationshipType,
                                c => c.EntitySic,
                                c => c.EntitySif,
                                c => c.Cancellation);
        }
    }
}
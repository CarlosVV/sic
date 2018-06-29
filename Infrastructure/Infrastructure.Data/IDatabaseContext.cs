namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Domain.Core.Model;

    #endregion

    public interface IDatabaseContext : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry Entry(object entity);

        IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters);

        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        IDbSet<AddressType> AddressTypes { get; set; }

        IDbSet<ApplyTo> ApplyToes { get; set; }

        IDbSet<CivilStatus> CivilStatus { get; set; }

        IDbSet<Entity> Entities { get; set; }

        IDbSet<Gender> Genders { get; set; }
        

        IDbSet<Master> Masters { get; set; }

        IDbSet<ParticipantStatus> ParticipantStatus { get; set; }

        IDbSet<ParticipantType> ParticipantType { get; set; }
        
        
        IDbSet<RelationshipCategory> RelationshipCategories { get; set; }

        IDbSet<RelationshipType> RelationshipTypes { get; set; }

        //IDbSet<ActiveIdent> ActiveIdents { get; set; }

        //IDbSet<ActiveOnOff> ActiveOnOffs { get; set; }

        IDbSet<Cancellation> Cancellations { get; set; }

        IDbSet<City> Cities { get; set; }

        IDbSet<Clinic> Clinics { get; set; }

        IDbSet<Country> Countries { get; set; }

        //IDbSet<Court> Courts { get; set; }

        IDbSet<Region> Regions { get; set; }

        IDbSet<State> States { get; set; }

        IDbSet<Class> Classes { get; set; }

        IDbSet<Concept> Concepts { get; set; }

        IDbSet<MonthlyConcept> MonthlyConcepts { get; set; }

        IDbSet<Payment> Payments { get; set; }

        IDbSet<Status> Status { get; set; }

        IDbSet<ThirdPartySchedule> ThirdPartySchedules { get; set; }

        IDbSet<TransferType> TransferTypes { get; set; }

        IDbSet<Case> Cases { get; set; }

        IDbSet<Compensation> Compensations { get; set; }

        IDbSet<CaseDetail> CaseDetails { get; set; }

        IDbSet<EmployerStatus> EmployerStatus { get; set; }

        IDbSet<KeyRiskIndicator> KeyRiskIndicators { get; set; }

        IDbSet<Transaction> Transactions { get; set; }

        IDbSet<TransactionType> TransactionTypes { get; set; }

        IDbSet<AccessControlLevel> AccessControlLevels { get; set; }

        IDbSet<ActivityLog> ActivityLogs { get; set; }

        IDbSet<ActivityLogType> ActivityLogTypes { get; set; }

        IDbSet<Functionality> Functionalities { get; set; }

        IDbSet<Menu> Menus { get; set; }

        IDbSet<ObjectType> ObjectTypes { get; set; }

        IDbSet<Profile> Profiles { get; set; }

        IDbSet<ResourcesString> ResourcesStrings { get; set; }

        IDbSet<Setting> Settings { get; set; }

        IDbSet<Address> Addresses { get; set; }        
        

        IDbSet<Log4Net_Log> Log4Net_Log { get; set; }

        IDbSet<SimeraBeneficiary> SimeraBeneficiary { get; set; }

        IDbSet<SimeraTransaction> SimeraTransaction { get; set; }

        IDbSet<MessageTemplate> MessageTemplate { get; set; }

        IDbSet<Occupation> Occupation{ get; set; }

        IDbSet<ModifiedReason> ModifiedReason { get; set; }

        //IDbSet<ParticipantType> ParticipantType { get; set; }

        IDbSet<SapTransaction> SapTransaction { get; set; }

        int SaveChanges();
    }
}
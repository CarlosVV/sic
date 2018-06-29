namespace Nagnoi.SiC.Infrastructure.Data
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Infrastructure.Interception;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Diagnostics;
    using System.Linq;
    using Configuration;
    using Core.Helpers;
    using Domain.Core.Model;
    using System.Data.Entity.Validation;

    #endregion

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        #region Constructors

        static DatabaseContext()
        {
            Database.SetInitializer<DatabaseContext>(null);
        }

        public DatabaseContext() : base("name=SicModel")
        {
            var dependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            SetConfigurationOptions();
            SetConfigurationInterceptors();
        }

        #endregion

        #region Properties

        public IDbSet<AddressType> AddressTypes { get; set; }
        public IDbSet<ApplyTo> ApplyToes { get; set; }
        public IDbSet<CivilStatus> CivilStatus { get; set; }
        public IDbSet<Entity> Entities { get; set; }
        public IDbSet<Gender> Genders { get; set; }        
        public IDbSet<Master> Masters { get; set; }
        public IDbSet<ParticipantStatus> ParticipantStatus { get; set; }
        public IDbSet<ParticipantType> ParticipantType { get; set; }        
        public IDbSet<RelationshipCategory> RelationshipCategories { get; set; }
        public IDbSet<RelationshipType> RelationshipTypes { get; set; }
        //public IDbSet<ActiveIdent> ActiveIdents { get; set; }
        //public IDbSet<ActiveOnOff> ActiveOnOffs { get; set; }
        public IDbSet<Cancellation> Cancellations { get; set; }
        public IDbSet<City> Cities { get; set; }
        public IDbSet<Clinic> Clinics { get; set; }
        public IDbSet<Country> Countries { get; set; }
        //public IDbSet<Court> Courts { get; set; }
        public IDbSet<Region> Regions { get; set; }
        public IDbSet<State> States { get; set; }
        public IDbSet<Class> Classes { get; set; }
        public IDbSet<Concept> Concepts { get; set; }
        public IDbSet<MonthlyConcept> MonthlyConcepts { get; set; }
        public IDbSet<Payment> Payments { get; set; }
        public IDbSet<Status> Status { get; set; }
        public IDbSet<ThirdPartySchedule> ThirdPartySchedules { get; set; }
        public IDbSet<TransferType> TransferTypes { get; set; }
        public IDbSet<Case> Cases { get; set; }
        public IDbSet<Compensation> Compensations { get; set; }
        public IDbSet<CaseDetail> CaseDetails { get; set; }
        public IDbSet<EmployerStatus> EmployerStatus { get; set; }
        public IDbSet<KeyRiskIndicator> KeyRiskIndicators { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }
        public IDbSet<TransactionType> TransactionTypes { get; set; }
        public IDbSet<AccessControlLevel> AccessControlLevels { get; set; }
        public IDbSet<ActivityLog> ActivityLogs { get; set; }
        public IDbSet<ActivityLogType> ActivityLogTypes { get; set; }
        public IDbSet<Functionality> Functionalities { get; set; }
        public IDbSet<Menu> Menus { get; set; }
        public IDbSet<ObjectType> ObjectTypes { get; set; }
        public IDbSet<Profile> Profiles { get; set; }
        public IDbSet<ResourcesString> ResourcesStrings { get; set; }
        public IDbSet<Setting> Settings { get; set; }
        public IDbSet<Address> Addresses { get; set; }                
        public IDbSet<Log4Net_Log> Log4Net_Log { get; set; }
        public IDbSet<SimeraBeneficiary> SimeraBeneficiary { get; set; }
        public IDbSet<SimeraTransaction> SimeraTransaction { get; set; }
        public IDbSet<Occupation> Occupation { get; set; }
        public IDbSet<ModifiedReason> ModifiedReason { get; set; }
        //public IDbSet<ParticipantType> ParticipantType { get; set; }

        public IDbSet<MessageTemplate> MessageTemplate { get; set; }
        public IDbSet<SapTransaction> SapTransaction { get; set; }

        #endregion

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public override int SaveChanges()
        {            
            var auditableEntities = GetAuditableEntities();
            
            if (auditableEntities.Any()) {
                SetAuditableProperties(auditableEntities);
            }

            return base.SaveChanges();                        
        }
        
        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TEntity>(sql, parameters);
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = default(int?), params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }
            
            return result;
        }

        #region Private Methods
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {    
            #region Entity Mapping

            modelBuilder.Entity<Entity>()
                .HasMany(e => e.Addresses)
                .WithRequired(e => e.Entity)
                .WillCascadeOnDelete(false);            
            
            modelBuilder.Entity<Entity>()
                .HasMany(e => e.PaymentRemitters)
                .WithOptional(e => e.Remitter)
                .HasForeignKey(e => e.EntityId_RemitTo);            
            
            modelBuilder.Entity<Entity>()
                .HasMany(e => e.ThirdPartyScheduleRemitters)
                .WithOptional(e => e.Remitter)
                .HasForeignKey(e => e.EntityId_RemitTo);

            modelBuilder.Entity<Entity>()
                .HasOptional(e => e.ModifiedReason)
                .WithMany(e => e.Entity)
                .HasForeignKey(e => e.ModifiedReasonId);

            modelBuilder.Entity<Entity>()
                .HasOptional(e => e.Occupation)
                .WithMany(e => e.Entity)
                .HasForeignKey(e => e.OccupationId);

            modelBuilder.Entity<Entity>()
                .HasOptional(e => e.ParticipantType)
                .WithMany(e => e.Entities)
                .HasForeignKey(e => e.ParticipantTypeId);
            
            #endregion

            #region Relationship Type

            modelBuilder.Entity<RelationshipType>()
                .HasMany(e => e.CaseDetailEntities)
                .WithOptional(e => e.RelationshipType)
                .HasForeignKey(e => e.RelationshipTypeId);

            #endregion

            #region Clinic Mapping

            modelBuilder.Entity<Clinic>()
                .HasMany(e => e.CaseClinics)
                .WithOptional(e => e.Clinic)
                .HasForeignKey(e => e.ClinicId);

            modelBuilder.Entity<Clinic>()
                .HasMany(e => e.CaseClinicServices)
                .WithOptional(e => e.ClinicService)
                .HasForeignKey(e => e.ClinicId_Service);

            modelBuilder.Entity<Clinic>()
                .HasMany(e => e.Payments)
                .WithOptional(e => e.Clinic)
                .HasForeignKey(e => e.ClinicId);

            modelBuilder.Entity<Clinic>()
                .HasOptional(e => e.Region)
                .WithMany(e => e.Clinics)
                .HasForeignKey(e => e.RegionId);

            #endregion

            #region Court Mapping

            //modelBuilder.Entity<Court>()
            //    .Property(e => e.S_ROWID)
            //    .HasPrecision(18, 0);
            
            #endregion

            #region Region Mapping

            modelBuilder.Entity<Region>()
                .HasMany(e => e.CaseRegions)
                .WithOptional(e => e.Region)
                .HasForeignKey(e => e.RegionId);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.CaseRegionServices)
                .WithOptional(e => e.RegionService)
                .HasForeignKey(e => e.RegionId_Service);

            modelBuilder.Entity<Region>()
                .HasMany(e => e.Clinics)
                .WithOptional(e => e.Region)
                .HasForeignKey(e => e.RegionId);

            #endregion

            #region Concept Mapping

            modelBuilder.Entity<Concept>()
                .HasMany(e => e.MonthlyConcepts)
                .WithRequired(e => e.Concept)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Concept>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.Concept);

            #endregion

            #region Payment Mapping

            modelBuilder.Entity<Payment>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Payment>()
                .Property(e => e.BaseAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Payment>()
                .Property(e => e.Discount)
                .HasPrecision(19, 4);

            //modelBuilder.Entity<Payment>()
            //    .HasOptional(e => e.ThirdPartySchedule)
            //    .WithMany(e => e.Payments)
            //    .HasForeignKey(e => e.ThirdPartySchedule);

            #endregion

            #region Third Party Schedule Mapping

            modelBuilder.Entity<ThirdPartySchedule>()
                .Property(e => e.SinglePaymentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ThirdPartySchedule>()
                .Property(e => e.FirstInstallmentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ThirdPartySchedule>()
                .Property(e => e.SecondInstallmentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ThirdPartySchedule>()
                .Property(e => e.OrderAmount)
                .HasPrecision(19, 4);

            #endregion

            #region Case Mapping
            
            modelBuilder.Entity<Case>()
                .Property(e => e.DailyWage)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Case>()
                .Property(e => e.WeeklyComp)
                .HasPrecision(19, 4);         
            
            modelBuilder.Entity<Case>()
                .HasOptional(e => e.EmployerStatus)
                .WithMany(e => e.Cases)
                .HasForeignKey(e => e.EmployerStatusId);

            modelBuilder.Entity<Case>()
                .HasMany(e => e.CaseDetails)
                .WithOptional(e => e.Case)
                .HasForeignKey(e => e.CaseId);

            modelBuilder.Entity<Case>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.CaseReference)
                .HasForeignKey(e => e.CaseId_Reference);

            #endregion

            #region Case Detail Mapping

            modelBuilder.Entity<CaseDetail>()
                .Property(e => e.Reserve)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CaseDetail>()
                .Property(e => e.DeductedMonthly)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CaseDetail>()
                .Property(e => e.LegacyAmountPaid_Inca)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CaseDetail>()
                .Property(e => e.LegacyAmountPaid_Diet)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.Entity)
                .WithMany(e => e.CaseDetailEntities)
                .HasForeignKey(e => e.EntityId);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.EntityCheck)
                .WithMany(e => e.CaseDetailEntityChecks)
                .HasForeignKey(e => e.EntityId_Check);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.EntityInca)
                .WithMany(e => e.CaseDetailEntityIncas)
                .HasForeignKey(e => e.EntityId_Inca);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.EntityDiet)
                .WithMany(e => e.CaseDetailEntityDiets)
                .HasForeignKey(e => e.EntityId_Diet);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.EntityLawyer)
                .WithMany(e => e.CaseDetailEntityLawyers)
                .HasForeignKey(e => e.EntityId_Lawyer);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.EntityLegalGuardian)
                .WithMany(e => e.CaseDetailEntityLegalGuardians)
                .HasForeignKey(e => e.EntityId_LegalGuardian);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.EntitySif)
                .WithMany(e => e.CaseDetailEntitySif)
                .HasForeignKey(e => e.EntityId_Sif);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.EntitySic)
                .WithMany(e => e.CaseDetailEntitySic)
                .HasForeignKey(e => e.EntityId_Sic);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.TransferType)
                .WithMany(e => e.CaseDetails)
                .HasForeignKey(e => e.TransferTypeId);

            modelBuilder.Entity<CaseDetail>()
                .HasOptional(e => e.Cancellation)
                .WithMany(e => e.CaseDetails)
                .HasForeignKey(e => e.CancellationId);

            modelBuilder.Entity<CaseDetail>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.CaseDetail)
                .HasForeignKey(e => e.CaseDetailId);

            modelBuilder.Entity<CaseDetail>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.CaseDetail)
                .HasForeignKey(e => e.CaseDetailId);

            modelBuilder.Entity<CaseDetail>()
                .HasMany(e => e.ThirdPartySchedules)
                .WithRequired(e => e.CaseDetail)
                .HasForeignKey(e => e.CaseDetailId);

            #endregion

            #region Transaction Mapping

            modelBuilder.Entity<Transaction>()
                .Map(t => t.Requires("Hidden").HasValue(false))
                .Ignore(t => t.Hidden);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.TransactionAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.MonthlyInstallment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.NumberOfWeeks)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Transaction>()
                .HasMany(e => e.TransactionDetails)
                .WithOptional(e => e.Transaction)
                .HasForeignKey(e => e.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasMany(e => e.Payments)
                .WithOptional(e => e.Transaction)
                .HasForeignKey(e => e.TransactionId);
            
            #endregion

            #region Transaction Type Mapping

            modelBuilder.Entity<TransactionType>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.TransactionType)
                .HasForeignKey(e => e.TransactionTypeId);

            #endregion

            #region Activity Log Type Mapping

            modelBuilder.Entity<ActivityLogType>()
                .HasMany(e => e.ActivityLogs)
                .WithRequired(e => e.ActivityLogType)
                .WillCascadeOnDelete(false);

            #endregion

            #region Functionality Mapping

            modelBuilder.Entity<Functionality>()
                .HasMany(e => e.AccessControlLevels)
                .WithRequired(e => e.Functionality)
                .WillCascadeOnDelete(false);

            #endregion

            #region Menu Mapping

            modelBuilder.Entity<Menu>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.ParentMenu)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Menu>()
                .HasOptional(e => e.Functionality)
                .WithMany(e => e.Menus)
                .HasForeignKey(e => e.FunctionalityId);

            #endregion

            #region Object Type Mapping

            modelBuilder.Entity<ObjectType>()
                .HasMany(e => e.ActivityLogs)
                .WithRequired(e => e.ObjectType)
                .HasForeignKey(e => e.ObjectTypeId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Profile Mapping

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.AccessControlLevels)
                .WithRequired(e => e.Profile)
                .WillCascadeOnDelete(false);

            #endregion

            #region Transaction Detail Mapping

            modelBuilder.Entity<TransactionDetail>()
                .Map(t => t.Requires("Hidden").HasValue(false))
                .Ignore(t => t.Hidden);

            modelBuilder.Entity<TransactionDetail>()
               .Property(e => e.Percent)
               .HasPrecision(3, 2);

            modelBuilder.Entity<TransactionDetail>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TransactionDetail>()
                .HasOptional(e => e.CompensationRegion)
                .WithMany(e => e.TransactionDetails)
                .HasForeignKey(e => e.CompensationRegionId);

            #endregion

            #region City Mapping

            modelBuilder.Entity<City>()
                .HasOptional(c => c.State)
                .WithMany(c => c.Cities)
                .HasForeignKey(c => c.StateId);

            #endregion

            #region State Mapping

            modelBuilder.Entity<State>()
                .HasMany(s => s.Cities)
                .WithOptional(s => s.State)
                .HasForeignKey(s => s.StateId);

            #endregion

            #region Compensation Mapping

            modelBuilder.Entity<Compensation>()
                .HasMany(s => s.Cases)
                .WithOptional(s => s.Compensation)
                .HasForeignKey(s => s.CompensationId);

            #endregion

            #region Adjustment Status Mapping

            modelBuilder.Entity<AdjustmentStatus>()
                .HasMany(e => e.Payments)
                .WithOptional(e => e.AdjustmentStatus)
                .HasForeignKey(e => e.AdjustmentStatusId)
                .WillCascadeOnDelete(false);

            #endregion

            #region Adjustment Reason Mapping

            modelBuilder.Entity<AdjustmentReason>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.AdjustmentReason)
                .HasForeignKey(e => e.AdjustmentReasonId);

            #endregion

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Types<ISoftDeletable>().Configure(e => e.Ignore(t => t.Hidden));

            base.OnModelCreating(modelBuilder);
        }

        private IEnumerable<DbEntityEntry<IAuditableEntity>> GetAuditableEntities()
        {
            return ChangeTracker.Entries<IAuditableEntity>()
                                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified));
        }

        private void SetAuditableProperties(IEnumerable<DbEntityEntry<IAuditableEntity>> auditableEntities)
        {
            DateTime now = DateTime.Now;

            foreach (var entry in auditableEntities)
            {
                string userName = WebHelper.GetUserName();

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = userName;
                    entry.Entity.CreatedDateTime = now;
                }
                else
                {
                    Entry(entry.Entity).Property(x => x.CreatedBy).IsModified = false;
                    Entry(entry.Entity).Property(x => x.CreatedDateTime).IsModified = false;
                }

                entry.Entity.ModifiedBy = userName;
                entry.Entity.ModifiedDateTime = now;
            }
        }

        private void SetConfigurationOptions()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        private void SetConfigurationInterceptors()
        {
            Database.CommandTimeout = 180;
            Database.Log = message => Debug.WriteLine(message);

            DbInterception.Add(new SoftDeleteInterceptor());
        }

        #endregion
    }
}
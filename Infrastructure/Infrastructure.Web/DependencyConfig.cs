namespace Nagnoi.SiC.Infrastructure.Web
{
    #region References

    using Application;
    using Application.Audit;
    using Application.Core;
    using Core.Data;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using FrontEnd.Application.Common;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Log;
    using Infrastructure.Data;

    #endregion

    public static class DependencyConfig
    {
        public static void Register()
        {
            IoC.Register<ICacheManager, PerRequestCacheManager>("sic_cache_por_request");
            IoC.Register<ICacheManager, LongTermCacheManager>();

            IoC.Register<IDatabaseContext, DatabaseContext>();
            IoC.Register<IUnitOfWork, EfUnitOfWork>();

            IoC.Register<IAddressRepository, AddressRepository>();
            IoC.Register<IEntityRepository, EntityRepository>();
            IoC.Register<IThirdPartyScheduleRepository, ThirdPartyScheduleRepository>();
            IoC.Register<IAddressTypeRepository, AddressTypeRepository>();            
            IoC.Register<IRelationshipTypeRepository, RelationshipTypeRepository>();
            IoC.Register<ITransferTypeRepository, TransferTypeRepository>();
            IoC.Register<ITransactionTypeRepository, TransactionTypeRepository>();
            IoC.Register<IGenderRepository, GenderRepository>();
            IoC.Register<IClassRepository, ClassRepository>();
            IoC.Register<ICaseRepository, CaseRepository>();
            IoC.Register<IApplyToRepository, ApplyToRepository>();
            IoC.Register<IBeneficiaryRepository, BeneficiaryRepository>();
            IoC.Register<ICivilStatusRepository, CivilStatusRepository>();
            IoC.Register<IConceptRepository, ConceptRepository>();
            IoC.Register<IResourceRepository, ResourceRepository>();
            IoC.Register<ISettingRepository, SettingRepository>();
            IoC.Register<IActivityLogRepository, ActivityLogRepository>();
            IoC.Register<ILogRepository, LogRepository>();
            IoC.Register<IRelationshipCategoryRepository, RelationshipCategoryRepository>();
            IoC.Register<IActivityLogRepository, ActivityLogRepository>();
            IoC.Register<IActivityLogTypeRepository, ActivityLogTypeRepository>();
            IoC.Register<IRepository<Menu>, EfRepository<Menu>>();
            IoC.Register<IAccessControlLevelRepository, AccessControlLevelRepository>();
            IoC.Register<IProfileRepository, ProfileRepository>();
            IoC.Register<IFunctionalityRepository, FunctionalityRepository>();
            IoC.Register<IRepository<CompensationRegion>, EfRepository<CompensationRegion>>();
            IoC.Register<IRepository<Status>, EfRepository<Status>>();
            IoC.Register<IRepository<Region>, EfRepository<Region>>();
            IoC.Register<IRepository<Clinic>, EfRepository<Clinic>>();
            //IoC.Register<IRepository<Court>, EfRepository<Court>>();
            IoC.Register<IRepository<City>, EfRepository<City>>();
            IoC.Register<IRepository<State>, EfRepository<State>>();
            IoC.Register<IRepository<Country>, EfRepository<Country>>();
            IoC.Register<ITransactionRepository, TransactionRepository>();
            IoC.Register<IPaymentRepository, PaymentRepository>();
            IoC.Register<ICaseDetailRepository, CaseDetailRepository>();
            IoC.Register<IRepository<MonthlyConcept>, EfRepository<MonthlyConcept>>();
            IoC.Register<IRepository<AdjustmentStatus>, EfRepository<AdjustmentStatus>>();
            IoC.Register<IRepository<Cancellation>, EfRepository<Cancellation>>();
            IoC.Register<IRepository<CaseDetail>, EfRepository<CaseDetail>>();
            IoC.Register<IRepository<SimeraTransaction>, EfRepository<SimeraTransaction>>();
            IoC.Register<ISimeraBeneficiaryRepository, SimeraBeneficiaryRepository>();
            IoC.Register<IRepository<AdjustmentReason>, EfRepository<AdjustmentReason>>();
            IoC.Register<IRepository<MessageTemplate>, EfRepository<MessageTemplate>>();
            IoC.Register<IRepository<Transaction>, EfRepository<Transaction>>();
            IoC.Register<IRepository<Occupation>, EfRepository<Occupation>>();
            IoC.Register<IRepository<ModifiedReason>, EfRepository<ModifiedReason>>();
            IoC.Register<IRepository<ParticipantType>, EfRepository<ParticipantType>>();
            IoC.Register<IRepository<SapTransaction>, EfRepository<SapTransaction>>();

            IoC.Register<IActivityLogService, ActivityLogService>();
            IoC.Register<ILogger, Log4NetProvider>();
            IoC.Register<IAccessControlLevelService, AccessControlLevelService>();
            IoC.Register<IAddressTypeService, AddressTypeService>();            
            IoC.Register<IRelationshipTypeService, RelationshipTypeService>();
            IoC.Register<ITransferTypeService, TransferTypeService>();
            IoC.Register<ITransactionTypeService, TransactionTypeService>();
            IoC.Register<IGenderService, GenderService>();
            IoC.Register<IClassService, ClassService>();
            IoC.Register<ICaseService, CaseService>();
            IoC.Register<IBeneficiaryService, BeneficiaryService>();
            IoC.Register<ICivilStatusService, CivilStatusService>();
            IoC.Register<IApplyToService, ApplyToService>();
            IoC.Register<IResourceService, ResourceService>();
            IoC.Register<ISettingService, SettingService>();
            IoC.Register<IMenuService, MenuService>();
            IoC.Register<IRelationshipCategoryService, RelationshipCategoryService>();
            IoC.Register<IConceptService, ConceptService>();
            IoC.Register<ITransactionService, TransactionService>();
            IoC.Register<IPaymentService, PaymentService>();
            IoC.Register<IEntityService, EntityService>();
            IoC.Register<ICompensationRegionService, CompensationRegionService>();
            IoC.Register<ILocationService, LocationService>();
            IoC.Register<ISimeraBeneficiaryService, SimeraBeneficiaryService>();
            IoC.Register<ISimeraTransactionService, SimeraTransactionService>();
            IoC.Register<IEmailSenderService, EmailSenderService>();
            IoC.Register<IMessageTemplateService, MessageTemplateService>();
            IoC.Register<IOccupationService, OccupationService>();
            IoC.Register<IModifiedReasonService, ModifiedReasonService>();
            IoC.Register<IParticipantTypeService, ParticipantTypeService>();

            IoC.Register<ISapTransactionService, SapTransactionService>();
        }
    }
}
namespace SIC.SapInterfaceConsole.Modules {

    #region References

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Log;
    using Nagnoi.SiC.Domain.Core.Services;
    using System.ServiceProcess;

    #endregion

    public class BaseService {

        #region Services

        public IAddressTypeService AddressTypeService {
            get { return IoC.Resolve<IAddressTypeService>(); }
        }        

        public ITransferTypeService TransferTypeService {
            get { return IoC.Resolve<ITransferTypeService>(); }
        }

        public IRelationshipTypeService RelationshipTypeService {
            get { return IoC.Resolve<IRelationshipTypeService>(); }
        }

        public ITransactionTypeService TransactionTypeService {
            get { return IoC.Resolve<ITransactionTypeService>(); }
        }

        public IGenderService GenderService {
            get { return IoC.Resolve<IGenderService>(); }
        }

        public ICaseService CaseService {
            get { return IoC.Resolve<ICaseService>(); }
        }

        public IClassService ClassService {
            get { return IoC.Resolve<IClassService>(); }
        }

        public IApplyToService ApplyToService {
            get { return IoC.Resolve<IApplyToService>(); }
        }

        public IBeneficiaryService BeneficiaryService {
            get { return IoC.Resolve<IBeneficiaryService>(); }
        }

        public ICivilStatusService CivilStatusService {
            get { return IoC.Resolve<ICivilStatusService>(); }
        }

        public IResourceService ResourceService {
            get { return IoC.Resolve<IResourceService>(); }
        }

        public IRelationshipCategoryService RelationshipCategoryService {
            get { return IoC.Resolve<IRelationshipCategoryService>(); }
        }

        public ISettingService SettingService {
            get { return IoC.Resolve<ISettingService>(); }
        }

        public IActivityLogService ActivityLogService {
            get { return IoC.Resolve<IActivityLogService>(); }
        }

        public ILogger Logger {
            get { return IoC.Resolve<ILogger>(); }
        }

        public IAccessControlLevelService PermissionService {
            get { return IoC.Resolve<IAccessControlLevelService>(); }
        }

        public IConceptService ConceptService {
            get { return IoC.Resolve<IConceptService>(); }
        }

        public ITransactionService TransactionService {
            get { return IoC.Resolve<ITransactionService>(); }
        }

        public IEntityService EntityService {
            get { return IoC.Resolve<IEntityService>(); }
        }

        public IPaymentService PaymentService {
            get { return IoC.Resolve<IPaymentService>(); }
        }

        public ICompensationRegionService CompensationRegionService {
            get { return IoC.Resolve<ICompensationRegionService>(); }
        }

        public ILocationService LocationService {
            get { return IoC.Resolve<ILocationService>(); }
        }

        public ISimeraBeneficiaryService SimeraBeneficiaryService {
            get { return IoC.Resolve<ISimeraBeneficiaryService>(); }
        }

        public ISimeraTransactionService SimeraTransactionService {
            get { return IoC.Resolve<ISimeraTransactionService>(); }
        }

        public IMessageTemplateService MessageTemplateService {
            get { return IoC.Resolve<IMessageTemplateService>(); }
        }

        public IEmailSenderService EmailSenderService {
            get { return IoC.Resolve<IEmailSenderService>(); }
        }

        public ISapTransactionService SapTransactionService {
            get { return IoC.Resolve<ISapTransactionService>(); }
        }

        #endregion

    }
}

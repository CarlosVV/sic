namespace Nagnoi.SiC.Infrastructure.Web.Services {
    
    #region References

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Domain.Core.Services;
    using Core.Dependencies;
    using Core.Log;
    using UI;
    using System.Text;
    using System.ServiceProcess;

    #endregion

    public class BaseService : ServiceBase {
        #region Services

        public IAddressTypeService AddressTypeService {
            get { return IoC.Resolve<IAddressTypeService>(); }
        }

        public IInternetTypeService InternetTypeService {
            get { return IoC.Resolve<IInternetTypeService>(); }
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

        #endregion

        #region Public Methods       

        protected void LogException(Exception ex) {
            IoC.Resolve<ILogger>().Error(ex.Message, ex);
        }

        protected virtual void SuccessNotification(string message) {
            SuccessNotification(message, true);
        }

        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest) {
            //AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotification(string message) {
            ErrorNotification(message, true);
        }

        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest) {
            //AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotification(Exception ex, bool persistForTheNextRequest, bool logException) {
            if (logException) {
                LogException(ex);
            }

            //AddNotification(NotifyType.Error, ex.Message, persistForTheNextRequest);
        }        

        #endregion
    }
}

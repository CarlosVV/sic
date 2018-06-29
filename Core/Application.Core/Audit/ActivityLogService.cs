namespace Nagnoi.SiC.Application.Audit
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using Domain.Core.Model;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Caching;
    using Infrastructure.Core.Dependencies;
    using Infrastructure.Core.Helpers;
    using Infrastructure.Core.Log;
    using Infrastructure.Core.Validations;
    using Nagnoi.SiC.Infrastructure.Core.Data;

    #endregion

    public sealed class ActivityLogService : IActivityLogService
    {
        #region Constants

        private readonly string ActivityLogTypesAllCacheDependencyKey = string.Format("Nagnoi.SiC.ActivityLogTypes.All");

        #endregion

        #region Private Members

        private readonly IActivityLogRepository activityLogRepository = null;

        private readonly IActivityLogTypeRepository activityLogTypeRepository = null;

        private readonly ICacheManager cacheManager = null;

        private readonly ISettingService settingService = null;

        private readonly ILogger logger = null;

        private readonly IUnitOfWork unitOfWork = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogService"/> class
        /// </summary>
        public ActivityLogService() : this(
            IoC.Resolve<IActivityLogRepository>(),
            IoC.Resolve<IActivityLogTypeRepository>(),
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<ISettingService>(),
            IoC.Resolve<IUnitOfWork>(),
            IoC.Resolve<ILogger>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogService"/> class
        /// </summary>
        /// <param name="logRepository">the Activity Log Repository layer reference</param>
        internal ActivityLogService(
            IActivityLogRepository activityLogRepository, 
            IActivityLogTypeRepository activityLogTypeRepository,
            ICacheManager cacheManager,            
            ISettingService settingFacade,
            IUnitOfWork unitOfWork,
            ILogger logger)
        {
            this.activityLogRepository = activityLogRepository;

            this.activityLogTypeRepository = activityLogTypeRepository;

            this.cacheManager = cacheManager;

            this.settingService = settingFacade;

            this.unitOfWork = unitOfWork;

            this.logger = logger;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a item indicating whether cache is enabled
        /// </summary>
        public bool CacheEnabled
        {
            get
            {
                return IoC.Resolve<ISettingService>().GetSettingValueBoolean("Cache.ActivityLogFacade.CacheEnabled");
            }
        }

        #endregion

        #region Public Methods

        public void CreateActivityLog(ActivityLog activityLog, string systemKeyword)
        {
            if (activityLog.IsNull())
            {
                return;
            }
            
            ActivityLogType activityType = this.FindActivityLogTypeBySystemKeyword(systemKeyword);
            if (activityType.IsNull())
            {
                return;
            }

            activityLog.CreatedBy = WebHelper.GetUserName();
            activityLog.CreatedDateTime = DateTime.UtcNow;
            activityLog.ActivityLogTypeId = activityType.ActivityLogTypeId;
            activityLog.ObjectTypeId = Validate.EnsureNotNull(activityLog.ObjectTypeId, (int)AuditObjectType.Generic);           
            activityLog.Comment = Validate.EnsureNotNull(activityLog.Comment);
            activityLog.Comment = Validate.EnsureMaximumLength(activityLog.Comment, 4000);
            this.activityLogRepository.Insert(activityLog);

            unitOfWork.SaveChanges();
        }

        public IEnumerable<ActivityLogType> GetAllActivityTypes()
        {
            IEnumerable<ActivityLogType> result;

            if (this.CacheEnabled && this.cacheManager.IsAdded(this.ActivityLogTypesAllCacheDependencyKey)) {
                Debug.WriteLine("Get Activity Log Types from Cache");

                result = this.cacheManager.Get(this.ActivityLogTypesAllCacheDependencyKey) as IEnumerable<ActivityLogType>;

                return result.Clone<ActivityLogType>();
            }

            result = this.activityLogTypeRepository.GetAll().OrderBy(item => item.Name);

            if (this.CacheEnabled) {
                Debug.WriteLine("Insert Activity Log Types on Cache");

                this.cacheManager.Add(this.ActivityLogTypesAllCacheDependencyKey, result);
            }

            return result.Clone<ActivityLogType>();           
        }

        public ActivityLogType FindActivityTypeById(int activityLogTypeId)
        {
            if (activityLogTypeId == 0)
            {
                return null;
            }

            IEnumerable<ActivityLogType> activityTypes = this.GetAllActivityTypes();

            var query = from activityType in activityTypes
                        where activityType.ActivityLogTypeId == activityLogTypeId
                        select activityType;

            return query.Any() ? query.FirstOrDefault() : null;
        }

        public ActivityLogType FindActivityLogTypeBySystemKeyword(string systemKeyword)
        {
            IEnumerable<ActivityLogType> activityTypes = this.GetAllActivityTypes();

            var query = from activityType in activityTypes
                        where activityType.SystemKeyword.Equals(systemKeyword, StringComparison.OrdinalIgnoreCase)
                        select activityType;

            return query.Any() ? query.FirstOrDefault() : null;
        }

        public IEnumerable<ActivityLog> SelectActivityLogsByUserId(int userId, DateTime from, DateTime to)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("userId");
            }

            return this.activityLogRepository.SelectActivityLogByUser(userId, from, to);
        }

        public void SaveActivityLogToCache(ActivityLog activityLog)
        {
            string cacheKey = string.Format("UserSessionActivity.{0}.{1}", WebHelper.GetUserName(), HttpContext.Current.Session.SessionID);

            Dictionary<string, object> cachedObject = this.cacheManager.Get(cacheKey) as Dictionary<string, object>;

            if (cachedObject.IsNull()) {
                this.logger.Error("The session summary data could not be found from cache");

                return;
            }

            List<ActivityLog> activityLogs = new List<ActivityLog>();

            if (cachedObject.ContainsKey("ActivityLogs")) {
                activityLogs = cachedObject["ActivityLogs"] as List<ActivityLog>;

                if (activityLogs.IsNull()) {
                    this.logger.Error("The activity logs could not be found from cached session summary");

                    return;
                }

                activityLog.ActivityLogType = this.FindActivityTypeById(activityLog.ActivityLogTypeId);
                activityLogs.Add(activityLog);
            }
        }

        public IEnumerable<ActivityLog> SearchActivityLogs(Hashtable filters)
        {
            if (filters.IsNull())
            {
                filters = new Hashtable();
            }

            return this.activityLogRepository.Search(filters);
        }

        public IEnumerable<ActivityLog> SelectActivityLogPaged(int startPage, int pageSize, bool sortAscending, out int totalRows, Hashtable filters)
        {
            if (filters.IsNull())
            {
                filters = new Hashtable();
            }
            else
            {
                if (filters.ContainsKey("USERID"))
                {
                    if (filters["USERID"].IsNull())
                    {
                        filters.Remove("USERID");
                    }
                    else
                    {
                        int userId;
                        if (!int.TryParse(filters["USERID"].ToString(), out userId))
                        {
                            filters.Remove("USERID");
                        }
                        else
                        {
                            if (userId == 0)
                            {
                                filters.Remove("USERID");
                            }
                        }
                    }
                }

                if (filters.ContainsKey("ACTIVITYLOGTYPEID"))
                {
                    if (filters["ACTIVITYLOGTYPEID"].IsNull())
                    {
                        filters.Remove("ACTIVITYLOGTYPEID");
                    }
                    else
                    {
                        int activityLogTypeId;
                        if (!int.TryParse(filters["ACTIVITYLOGTYPEID"].ToString(), out activityLogTypeId))
                        {
                            filters.Remove("ACTIVITYLOGTYPEID");
                        }
                        else
                        {
                            if (activityLogTypeId == 0)
                            {
                                filters.Remove("ACTIVITYLOGTYPEID");
                            }
                        }
                    }
                }
            }

            IEnumerable<ActivityLog> activityLog = this.activityLogRepository.SelectPaged();
            totalRows = 10; 
            return activityLog;

        }

        /// <summary>
        /// Searches and selects paginated ActivityLogType entries, based on the specified searching filters and pagination data
        /// </summary>
        /// <param name="startPage">the page number to be returned</param>
        /// <param name="pageSize">the size (number of records) of the page to be returned</param>
        /// <param name="sortAscending">a item indicating whether or not the sorting is in ascending order</param>
        /// <param name="totalRows">an out parameter indicating the total number of rows that match the filtering criteria</param>
        /// <param name="filters">a hashtable containing the filtering criteria</param>
        /// <returns>Returns a paginated list of activity log type entries</returns>
        public IEnumerable<ActivityLogType> SelectActivityLogTypesPaged(int startPage, int pageSize, bool sortAscending, out int totalRows, Hashtable filters)
        {
            if (filters.IsNull())
            {
                filters = new Hashtable();
            }

            IEnumerable<ActivityLogType> activityLogTypes = this.GetAllActivityTypes();

            if (filters.ContainsKey("SYSTEMKEYWORD") && !filters["SYSTEMKEYWORD"].IsNull())
            {
                string systemKeyword = filters["SYSTEMKEYWORD"].ToString();
                activityLogTypes = activityLogTypes.Where(item => item.SystemKeyword.IndexOf(systemKeyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            bool? isEnabled = null;
            if (filters.ContainsKey("ISENABLED") && !filters["ISENABLED"].IsNull())
            {
                bool testBoolean;
                if (bool.TryParse(filters["ISENABLED"].ToString(), out testBoolean))
                {
                    isEnabled = testBoolean;
                }
                else
                {
                    if (filters["ISENABLED"].ToString() == "0")
                    {
                        isEnabled = false;
                    }
                    else if (filters["ISENABLED"].ToString() == "1")
                    {
                        isEnabled = true;
                    }
                    else
                    {
                        isEnabled = null;
                    }
                }
            }

            if (isEnabled.HasValue)
            {
                activityLogTypes = activityLogTypes.Where(item => item.IsEnabled == isEnabled.Value);
            }

            totalRows = activityLogTypes.Count();

            if (sortAscending)
            {
                activityLogTypes = activityLogTypes.OrderBy(item => item.SystemKeyword);
            }
            else
            {
                activityLogTypes = activityLogTypes.OrderByDescending(item => item.SystemKeyword);
            }

            return activityLogTypes.Skip((startPage - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Saves changes in a list of activity log types
        /// </summary>
        /// <param name="activityLogTypes">a list of ActivityLogType objects</param>
        public void SaveActivityLogTypes(IList<ActivityLogType> activityLogTypes)
        {
            if (activityLogTypes.IsNullOrEmpty()) {
                throw new ArgumentNullException("activityLogTypes");
            }

            IEnumerable<ActivityLogType> allActivityLogTypes = this.GetAllActivityTypes();

            ActivityLogType changedActivityLogType;
            ActivityLogType matchingActivityLogType;
            for (int index = activityLogTypes.Count() - 1; index >= 0; index--) {
                changedActivityLogType = activityLogTypes.ElementAt(index);
                matchingActivityLogType = allActivityLogTypes.Where(item => item.ActivityLogTypeId == changedActivityLogType.ActivityLogTypeId).FirstOrDefault();

                if (matchingActivityLogType.IsNull() || changedActivityLogType.IsEnabled == matchingActivityLogType.IsEnabled) {
                    activityLogTypes.RemoveAt(index);
                }
            }

            string activityLogTypesXml = activityLogTypes.BuildXml();

            this.activityLogTypeRepository.SaveAll(activityLogTypesXml, WebHelper.GetServerName(), DateTime.UtcNow);

            if (this.CacheEnabled) {
                string activityLogTypesAllCacheKey = "Nagnoi.SiC.ActivityLogTypes.All";

                this.cacheManager.Remove(activityLogTypesAllCacheKey);
            }

            ActivityLog activityLog = new ActivityLog() {
                ObjectTypeId = (int)AuditObjectType.ActivityLogType,
                ObjectId = null,
                Comment = activityLogTypesXml
            };

            this.CreateActivityLog(activityLog, "EnableDisableActivityLogTypes");
            
        }

        public void CreateDictionaryForThirdParty(ref Dictionary<string, object> dicThirdParty, ThirdPartySchedule objThirdPartySchedule) {

            if (objThirdPartySchedule.IsNull())
                throw new ArgumentNullException();

            if (dicThirdParty.IsNull())
                dicThirdParty = new Dictionary<string, object>();

            dicThirdParty.Add("IdEntidad", !objThirdPartySchedule.EntityId_RemitTo.IsNull() ? 
                objThirdPartySchedule.EntityId_RemitTo.Value : -1);
            dicThirdParty.Add("NumeroCasoOrden", objThirdPartySchedule.TerminationOrderNumber);
            dicThirdParty.Add("NumeroParticipante", objThirdPartySchedule.OrderIdentifier);
            dicThirdParty.Add("TerminationFlag", objThirdPartySchedule.TerminationFlag.HasValue ? 
                objThirdPartySchedule.TerminationFlag.Value: false);
            dicThirdParty.Add("SinglePayment", objThirdPartySchedule.SinglePaymentAmount.Value);
            dicThirdParty.Add("FirstInstallmentAmount",!objThirdPartySchedule.FirstInstallmentAmount.IsNull() ?
                objThirdPartySchedule.FirstInstallmentAmount.Value : 0);
            dicThirdParty.Add("SecondInstallmentAmount", !objThirdPartySchedule.SecondInstallmentAmount.IsNull() ?
                objThirdPartySchedule.SecondInstallmentAmount.Value : 0);
            dicThirdParty.Add("FechaTerminacion", objThirdPartySchedule.TerminationDate.HasValue ? 
                objThirdPartySchedule.TerminationDate.Value.ToLongDateString() : string.Empty);
        }

        public void CreateDictionaryForRegistroAnticipo(ref Dictionary<string, object> dicThirdParty, Payment objPayment) { 
        }
        public void CreateDictionaryForRegistroInversión(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForRegistroDietas(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForRegistroIPP(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForPagosaTerceros(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForHonorarios(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForAjustesaIncapacidades(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForCertificarCantidades(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForAprobación(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForTransacción(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForAjustesEBT(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForDocumentarDébito(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForTablasdeReferencia(ref Dictionary<string, object> dictValues, Payment objPayment) { }
        public void CreateDictionaryForCertificarPreExistentes(ref Dictionary<string, object> Payment, Payment objPayment) { }
        public void CreateDictionaryForActualizarDemográficos(ref Dictionary<string, object> Payment, Payment objPayment) { }

        //public void CreateDictionaryForPayment(ref Dictionary<string, object> dicPayment, Payment objPayment, Transaction objTransaction, TransactionTypeEnum type) {

        //    if (objPayment.IsNull())
        //        throw new ArgumentNullException();

        //    if (dicPayment.IsNull())
        //        dicPayment = new Dictionary<string, object>();            

        //    switch (type) {

        //        case TransactionTypeEnum.Anticipos:
        //            dicPayment.Add("Cantidad", objPayment.Amount);
        //            dicPayment.Add("Beneficiario", objPayment.EntityId_RemitTo.Value);
        //            dicPayment.Add("Fecha", objPayment.IssueDate.Value);                    
        //            break;
        //        case TransactionTypeEnum.Inversion:
        //            dicPayment.Add("Cantidad", objPayment.Amount);
        //            dicPayment.Add("Peticionario", objPayment.EntityId_RemitTo.Value);
        //            dicPayment.Add("Fecha", objPayment.IssueDate.Value);
        //            break;
        //        case TransactionTypeEnum.AdjudicacionAdicional:
        //            dicPayment.Add("Cantidad", objPayment.Amount);
        //            dicPayment.Add("Fecha", objPayment.IssueDate);
        //            dicPayment.Add("Desglose", objPayment.IssueDate.Value);
        //            break;
        //        case TransactionTypeEnum.HonorarioAbogados:
        //            dicPayment.Add("Abogado", objTransaction.CaseDetail.Entity.EntityId);
        //            dicPayment.Add("FechaDecision", objTransaction.DecisionDate);
        //            dicPayment.Add("FechaVista", objTransaction.HearingDateIC.IsNull()? string.Empty: objTransaction.HearingDateIC.Value.ToLongDateString());
        //            dicPayment.Add("FechaNotificacion", objTransaction.NotificationDateIC.IsNull()? string.Empty: objTransaction.NotificationDateIC.Value.ToLongDateString());
        //            dicPayment.Add("NumeroCasoCI", objPayment.IssueDate.Value);
        //            dicPayment.Add("Monto", objPayment.IssueDate.Value);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #endregion
        
    }
}
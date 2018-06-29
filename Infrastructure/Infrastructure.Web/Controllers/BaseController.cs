namespace Nagnoi.SiC.Infrastructure.Web.Controllers
{
    #region References

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Core.Dependencies;
    using Core.Log;
    using Domain.Core.Services;
    using Newtonsoft.Json;
    using UI;

    #endregion

    public class BaseController : Controller
    {
        #region Services

        public IAddressTypeService AddressTypeService {
            get { return IoC.Resolve<IAddressTypeService>(); }
        }

        public ITransferTypeService TransferTypeService
        {
            get { return IoC.Resolve<ITransferTypeService>(); }
        }

        public IRelationshipTypeService RelationshipTypeService
        {
            get { return IoC.Resolve<IRelationshipTypeService>(); }
        }

        public ITransactionTypeService TransactionTypeService
        {
            get { return IoC.Resolve<ITransactionTypeService>(); }
        }

        public IGenderService GenderService
        {
            get { return IoC.Resolve<IGenderService>(); }
        }

        public ICaseService CaseService
        {
            get { return IoC.Resolve<ICaseService>(); }
        }

        public IClassService ClassService
        {
            get { return IoC.Resolve<IClassService>(); }
        }

        public IApplyToService ApplyToService
        {
            get { return IoC.Resolve<IApplyToService>(); }
        }

        public IBeneficiaryService BeneficiaryService
        {
            get { return IoC.Resolve<IBeneficiaryService>(); }
        }

        public ICivilStatusService CivilStatusService
        {
            get { return IoC.Resolve<ICivilStatusService>(); }
        }

        public IResourceService ResourceService {
            get { return IoC.Resolve<IResourceService>(); }
        }

        public IRelationshipCategoryService RelationshipCategoryService
        {
            get { return IoC.Resolve<IRelationshipCategoryService>(); }
        }

        public ISettingService SettingService {
            get { return IoC.Resolve<ISettingService>(); }
        }

        public IActivityLogService ActivityLogService {
            get { return IoC.Resolve<IActivityLogService>(); }
        }

        public ILogger Logger
        {
            get { return IoC.Resolve<ILogger>(); }
        }

        public IAccessControlLevelService PermissionService {
            get { return IoC.Resolve<IAccessControlLevelService>(); }
        }

        public IConceptService ConceptService {
            get { return IoC.Resolve<IConceptService>(); }
        }

        public ITransactionService TransactionService
        {
            get { return IoC.Resolve<ITransactionService>(); }
        }

        public IEntityService EntityService
        {
            get { return IoC.Resolve<IEntityService>(); }
        }

        public IPaymentService PaymentService
        {
            get { return IoC.Resolve<IPaymentService>(); }
        }

        public ICompensationRegionService CompensationRegionService
        {
            get { return IoC.Resolve<ICompensationRegionService>(); }
        }

        public ILocationService LocationService
        {
            get { return IoC.Resolve<ILocationService>(); }
        }

        public ISimeraBeneficiaryService SimeraBeneficiaryService
        {
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

        public IOccupationService OccupationService
        {
            get { return IoC.Resolve<IOccupationService>(); }
        }
        #endregion

        #region Public Methods

        protected void AddModelErrors(params string[] errors)
        {
            foreach (string error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        protected void AddModelErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        protected void LogException(Exception ex)
        {
            IoC.Resolve<ILogger>().Error(ex.Message, ex);
        }

        protected virtual void SuccessNotification(string message)
        {
            SuccessNotification(message, true);
        }

        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotification(string message)
        {
            ErrorNotification(message, true);
        }

        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotification(Exception ex, bool persistForTheNextRequest, bool logException)
        {
            if (logException)
            {
                LogException(ex);
            }

            AddNotification(NotifyType.Error, ex.Message, persistForTheNextRequest);
        }

        protected virtual void AddNotification(NotifyType notifyType, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("sic.notifications.{0}", notifyType);

            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }
        
        protected virtual JsonResult JsonDataTable(object data)
        {
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        #endregion
    }

    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings SerializerSettings { get; set; }

        public Formatting Formatting { get; set; }
        
        public JsonNetResult()
        {
            MaxJsonLength = int.MaxValue;

            SerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                context.HttpContext.Request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("JSON GET is not allowed");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data == null)
            {
                return;
            }

            JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
            JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
            serializer.Serialize(writer, Data);

            writer.Flush();
        }
    }
}
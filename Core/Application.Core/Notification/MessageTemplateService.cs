namespace Nagnoi.SiC.Application.Core {

    #region References

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Core.Model;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Helpers;
    using Domain.Core.Repositories;
    using Domain.Core.Services;
    using Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Data;
    using Nagnoi.SiC.Infrastructure.Core.Caching;
    using Nagnoi.SiC.Infrastructure.Core.Configuration;
    using System.Diagnostics;

    #endregion

    public sealed class MessageTemplateService : IMessageTemplateService
    {
        #region Constants

        private const string MessageTemplateAllCacheDependencyKey = "Nagnoi.SiC.Settings.All-{0}";

        #endregion

        #region Private Members

        private readonly IRepository<MessageTemplate> messageTemplateRepository = null;
        private readonly ISettingRepository settingRepository = null;
        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructors
        public MessageTemplateService() : this(
            IoC.Resolve<ISettingRepository>(),
            IoC.Resolve<ICacheManager>(),
            IoC.Resolve<IRepository<MessageTemplate>>()
            )
        {
        }

        internal MessageTemplateService(
            ISettingRepository settingRepository,           
            ICacheManager cacheManager,
            IRepository<MessageTemplate> messageTemplateRepository
           )
        {
            this.settingRepository = settingRepository;
            this.cacheManager = cacheManager;
            this.messageTemplateRepository = messageTemplateRepository;
        }

        #endregion

        #region Properties

        public bool CacheEnabled {
            get {
                return true;
            }
        }
        #endregion

        #region Public Methods

        public IEnumerable<MessageTemplate> GetAllSettings() {
            IEnumerable<MessageTemplate> result = null;

            string key = MessageTemplateAllCacheDependencyKey.FormatString(SystemSettings.ApplicationId);

            if (this.CacheEnabled && this.cacheManager.IsAdded(key)) {
                Debug.WriteLine("Get Message Template from Cache");

                result = this.cacheManager.Get(key) as IEnumerable<MessageTemplate>;

                return result.Clone();
            }

            result = this.messageTemplateRepository.GetAll().ToList();

            if (this.CacheEnabled) {
                Debug.WriteLine("Insert Message Template on Cache");

                this.cacheManager.Add(key, result);
            }

            return result.Clone();
        }

        public MessageTemplate GetMessageTemplate(string keyword) {
            return this.GetAllSettings().Where(p => p.MessageTemplateKeyword == keyword).FirstOrDefault();            
        }

        #endregion
    }
}

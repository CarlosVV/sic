namespace Nagnoi.SiC.FrontEnd.Application.Common
{
    #region Imports
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Nagnoi.SiC.Domain.Core.Model;
    using Nagnoi.SiC.Domain.Core.Repositories;
    using Nagnoi.SiC.Domain.Core.Services;
    using Nagnoi.SiC.Infrastructure.Core.Caching;
    using Nagnoi.SiC.Infrastructure.Core.Configuration;
    using Nagnoi.SiC.Infrastructure.Core.Dependencies;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    using Nagnoi.SiC.Infrastructure.Core.Validations;
    #endregion

    public sealed class SettingService : ISettingService
    {
        #region Constants

        private const string SettingsAllCacheDependencyKey = "Nagnoi.SiC.Settings.All-{0}";

        #endregion

        #region Private Members

        private readonly ISettingRepository settingRepository = null;
        private readonly ICacheManager cacheManager = null;

        #endregion

        #region Constructors
        public SettingService() : this(
            IoC.Resolve<ISettingRepository>(),
            IoC.Resolve<ICacheManager>()
            )
        {
        }

        internal SettingService(
            ISettingRepository settingRepository,           
            ICacheManager cacheManager
           )
        {
            this.settingRepository = settingRepository;
            this.cacheManager = cacheManager;
        }

        #endregion

        #region Properties

        public bool CacheEnabled
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Public Methods

        public bool GetSettingValueBoolean(string name)
        {
            return this.GetSettingValueBoolean(name, false, false);
        }

        public bool GetSettingValueBoolean(string name, bool defaultValue)
        {
            return this.GetSettingValueBoolean(name, defaultValue, false);
        }

        public bool GetSettingValueBoolean(string name, bool defaultValue, bool includeApplicationName)
        {
            string value = this.GetSettingValueByName(name, includeApplicationName);
            if (!value.IsNullOrEmpty())
            {
                return bool.Parse(value);
            }

            return defaultValue;
        }

        public int GetSettingValueInteger(string name)
        {
            return this.GetSettingValueInteger(name, 0, false);
        }

        public int GetSettingValueInteger(string name, int defaultValue)
        {
            return this.GetSettingValueInteger(name, defaultValue, false);
        }

        public int GetSettingValueInteger(string name, int defaultValue, bool includeApplicationName)
        {
            string value = this.GetSettingValueByName(name, includeApplicationName);

            if (!value.IsNullOrEmpty())
            {
                return value.ToInt();
            }

            return defaultValue;
        }

        public string GetSettingValueByName(string name)
        {
            return this.GetSettingValueByName(name, false);
        }

        public string GetSettingValueByName(string name, bool includeApplicationName)
        {
            var setting = this.GetSettingByName(name, includeApplicationName);
            if (!setting.IsNull()) {
                return setting.Value;
            }

            return string.Empty;
        }

        public Setting GetSettingByName(string name, bool includeApplicationName)
        {
            if (name.IsNullOrEmpty())
            {
                return null;
            }

            name = name.Trim().ToLowerInvariant();

            var settings = this.GetAllSettings();

            var query = from setting in settings
                        where setting.Name.Trim().ToLowerInvariant().Equals(name, StringComparison.OrdinalIgnoreCase)
                        select setting;
            if (query.Any())
            {
                return query.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Setting> GetAllSettings()
        {
            IEnumerable<Setting> result = null;

            string key = SettingsAllCacheDependencyKey.FormatString(SystemSettings.ApplicationId);

            if (this.CacheEnabled && this.cacheManager.IsAdded(key)) {
                Debug.WriteLine("Get Settings from Cache");

                // it seems result is NULL
                // result = this.cacheManager.Get(key) as IEnumerable<Setting>;
                result = (IEnumerable<Setting>) this.cacheManager.Get(key);
                return result.Clone();
            }

            result = this.settingRepository.GetAll().ToList();

            if (this.CacheEnabled) {
                Debug.WriteLine("Insert Settings on Cache");

                this.cacheManager.Add(key, result);
            }

            return result.Clone();            
        }

        public Setting SetParam(string name, string value)
        {
            return SetParam(name, value, string.Empty);
        }

        public Setting SetParam(string name, string value, string description)
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentNullException("setting name");
            }

            Setting setting = this.GetSettingByName(name, false);
            setting.Name = Validate.EnsureNotNull(setting.Name);
            setting.Name = Validate.EnsureMaximumLength(setting.Name, 200);
            setting.Value = Validate.EnsureNotNull(value);
            setting.Value = Validate.EnsureMaximumLength(setting.Value, 2000);

            if (description.IsNullOrEmpty())
            {
                description = setting.Description;
            }
            setting.Description = Validate.EnsureNotNull(description);

            this.settingRepository.Update(setting);           

            this.CreateActivityLogSetParam(setting.SettingId, setting.Name, setting.Value);

            return setting;
        }

        public Setting InsertSetting(Setting setting)
        {
            if (setting.IsNull())
            {
                throw new ArgumentNullException("setting");
            }

            if (setting.Name.IsNullOrEmpty())
            {
                throw new ArgumentNullException("setting name");
            }

            setting.Name = Validate.EnsureNotNull(setting.Name);
            setting.Name = Validate.EnsureMaximumLength(setting.Name, 200);
            setting.Value = Validate.EnsureNotNull(setting.Value);
            setting.Value = Validate.EnsureMaximumLength(setting.Value, 2000);
            setting.Description = Validate.EnsureNotNull(setting.Description);

            this.settingRepository.Insert(setting);
            
            if (this.CacheEnabled)
            {
                string key = SettingsAllCacheDependencyKey.FormatString(SystemSettings.ApplicationId);
                this.cacheManager.RemoveByPattern(key);
            }

            return setting;
        }

        #endregion

        #region Private Methods

        private void CreateActivityLogSetParam(int settingId, string settingName, string settingValue)
        {
            string comment = "Save setting with Id {0}, name {1}, value{2}".FormatString(settingId, settingName, settingValue);

            ActivityLog activityLog = new ActivityLog()
            {
                ObjectTypeId = (int)AuditObjectType.Setting,
                ObjectId = settingName,
                Comment = comment
            };

            IoC.Resolve<IActivityLogService>().CreateActivityLog(activityLog, "EditSetting");
        }

        #endregion
    }
}
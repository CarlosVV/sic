namespace Nagnoi.SiC.Infrastructure.Core.Configuration
{
    #region Imports

    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    #endregion

    /// <summary>
    /// Static class for system settings
    /// </summary>
    public static class SystemSettings
    {
        #region Constants

        /// <summary>
        /// Key name for Use SSL
        /// </summary>
        private const string UseSslAppSettingKey = "UseSSL";

        /// <summary>
        /// Key name for Server Host Name
        /// </summary>
        private const string StatusPageNameAppSettingKey = "StatusPageName";

        /// <summary>
        /// Key name for Application Name
        /// </summary>
        private const string ApplicationIdAppSettingKey = "AppId";             

        /// <summary>
        /// Key name for External Site Url
        /// </summary>
        private const string ExternalSiteUrlAppSettingKey = "ExtenalSiteUrl";        

        /// <summary>
        /// Key name for Cache File Path
        /// </summary>
        private const string CacheFilePathAppSettingKey = "CacheFilePath";

        /// <summary>
        /// Key name for Maintenance Mode
        /// </summary>
        private const string MaintenanceModeAppSettingKey = "MaintenanceMode";

        /// <summary>
        /// Key name for By Pass URL
        /// </summary>
        private const string ByPassUrlAppSettingKey = "ByPassUrl";

        /// <summary>
        /// Key name for Dependency Container Type
        /// </summary>
        private const string DependencyContainerTypeNameSettingKey = "DependencyResolverTypeName";      

        #endregion

        #region Properties

        /// <summary>
        /// Gets a item indicating whether the application uses SSL protocol
        /// </summary>
        public static bool UseSsl
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[UseSslAppSettingKey]))
                {
                    return false;
                }
                else
                {
                    bool result;

                    if (bool.TryParse(ConfigurationManager.AppSettings[UseSslAppSettingKey], out result))
                    {
                        return result;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

      

        /// <summary>
        /// Gets the server host name to bring this server out of the LB Comment the line bellow so that
        /// the status page returns a blank page when polled to write a comment put an apostrophe at the beginning of the line
        /// </summary>
        public static string StatusPageName
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[StatusPageNameAppSettingKey]))
                {
                    return string.Empty;
                }
                else
                {
                    return ConfigurationManager.AppSettings[StatusPageNameAppSettingKey];
                }
            }
        }

        /// <summary>
        /// Gets the application name
        /// </summary>
        public static int ApplicationId
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[ApplicationIdAppSettingKey]))
                {
                    throw new InvalidOperationException("Application identifier not found");
                }

                return ConfigurationManager.AppSettings[ApplicationIdAppSettingKey].ToInt(0);
            }
        }
               

        /// <summary>
        /// Gets the URL external site (for GTS applications)
        /// </summary>
        public static string ExternalSiteUrl
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[ExternalSiteUrlAppSettingKey]))
                {
                    throw new InvalidOperationException("URL External not found");
                }

                return ConfigurationManager.AppSettings[ExternalSiteUrlAppSettingKey];
            }
        }              

        /// <summary>
        /// Gets the cache file path
        /// </summary>
        public static string CacheFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[CacheFilePathAppSettingKey]))
                {
                    return string.Empty;
                }
                else
                {
                    return ConfigurationManager.AppSettings[CacheFilePathAppSettingKey];
                }
            }
        }

        /// <summary>
        /// Gets a item indicating whether the application is under maintenance mode
        /// </summary>
        public static bool MaintenanceMode
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[MaintenanceModeAppSettingKey]))
                {
                    return false;
                }
                else
                {
                    bool result;

                    if (bool.TryParse(ConfigurationManager.AppSettings[MaintenanceModeAppSettingKey], out result))
                    {
                        return result;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of allowed IPs when the application is under maintenance mode
        /// </summary>
        public static string ByPassUrl
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[ByPassUrlAppSettingKey]))
                {
                    return string.Empty;
                }
                else
                {
                    return ConfigurationManager.AppSettings[ByPassUrlAppSettingKey];
                }
            }
        }     

        /// <summary>
        /// Gets the list of allowed IPs when the application is under maintenance mode
        /// </summary>
        public static string DependencyContainerType
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[DependencyContainerTypeNameSettingKey]))
                {
                    throw new InvalidOperationException("Dependency Container not found");
                }

                return ConfigurationManager.AppSettings[DependencyContainerTypeNameSettingKey];
            }
        }
      
        #endregion
    }
}
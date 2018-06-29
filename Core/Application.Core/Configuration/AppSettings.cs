namespace Nagnoi.SiC.Application.Core.Configuration {
    using Nagnoi.SiC.Infrastructure.Core.Exceptions;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Linq;
    public static class AppSettings {
        public static T Get<T>(string key) {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(appSetting)) throw new AppSettingNotFoundException(key);

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }

        public static T GetValue<T>(this  NameValueCollection nameValuePairs, string configKey, T defaultValue) where T : IConvertible {
            T retValue = default(T);

            if (nameValuePairs.AllKeys.Contains(configKey)) {
                string tmpValue = nameValuePairs[configKey];

                retValue = (T)Convert.ChangeType(tmpValue, typeof(T));
            }
            else {
                return defaultValue;
            }

            return retValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagnoi.SiC.Infrastructure.Core.Configuration {
    #region Imports

    using System;
    using System.Configuration;

    #endregion
    /// <summary>
    /// Representa las configuraciones del sistema
    /// </summary>
    public class Settings {
        #region Miembros Privados

        /// <summary>
        /// Key name for Dependency Container Type
        /// </summary>
        private const string DependencyContainerTypeNameSettingKey = "DependencyResolverTypeName";

        private const string RepositorioReportesSettingKey = "RepositorioReportes";

        #endregion

        #region Propiedades

        public static string DependencyContainerType {
            get {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[DependencyContainerTypeNameSettingKey])) {
                    throw new InvalidOperationException("Dependency Container no encontrado");
                }
                else {
                    return ConfigurationManager.AppSettings[DependencyContainerTypeNameSettingKey];
                }
            }
        }      

        #endregion
    }
}

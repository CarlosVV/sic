using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagnoi.SiC.Infrastructure.Core.Exceptions {
    public class AppSettingNotFoundException : Exception {
        #region Fields

        /// <summary>
        /// The setting name
        /// </summary>
        private readonly string _settingName;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="AppSettingNotFoundException"/>
        /// </summary>
        /// <param name="settingName"></param>
        public AppSettingNotFoundException(string settingName) {
            _settingName = settingName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the message for the exception
        /// </summary>
        //public override string Message {
        //    get { return string.Format(Resources.AppSettingMissingMessageFormat, _settingName); }
        //}

        #endregion
    }
}

namespace Nagnoi.SiC.Infrastructure.Core.Log
{
    #region Imports

    using System;
    using System.Web;
    using Nagnoi.SiC.Infrastructure.Core.Configuration;
    using Nagnoi.SiC.Infrastructure.Core.Helpers;
    using log4net;
    using log4net.Config;

    #endregion

    /// <summary>
    /// Represents the implementation of ILogger for Log4Net
    /// </summary>
    public sealed class Log4NetProvider : ILogger, ILogProvider
    {
        #region Private Members

        /// <summary>
        /// Log interface of Log4Net
        /// </summary>
        private readonly ILog logInstance;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetProvider"/> class.
        /// </summary>
        public Log4NetProvider()
        {
            this.logInstance = LogManager.GetLogger(this.GetType());
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Configures the Log4NetProvider
        /// </summary>
        public static void Configure()
        {
            XmlConfigurator.Configure();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Inserts a debug message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public void Debug(string message, Exception exception = null)
        {
            if (!this.logInstance.IsDebugEnabled)
            {
                return;
            }

            ThreadContext.Properties["AppId"] = SystemSettings.ApplicationId;

            if (exception.IsNull())
            {
                this.logInstance.Debug(message);
            }
            else
            {
                this.logInstance.Debug(message, exception);
            }
        }

        /// <summary>
        /// Inserts an information message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public void Information(string message, Exception exception = null)
        {
            if (!this.logInstance.IsInfoEnabled)
            {
                return;
            }

            ThreadContext.Properties["AppId"] = SystemSettings.ApplicationId;

            if (exception.IsNull())
            {
                this.logInstance.Info(message);
            }
            else
            {
                this.logInstance.Info(message, exception);
            }
        }

        /// <summary>
        /// Inserts a warning message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public void Warning(string message, Exception exception = null)
        {
            if (!this.logInstance.IsWarnEnabled)
            {
                return;
            }

            ThreadContext.Properties["AppId"] = SystemSettings.ApplicationId;

            if (exception.IsNull())
            {
                this.logInstance.Warn(message);
            }
            else
            {
                this.logInstance.Warn(message, exception);
            }
        }

        /// <summary>
        /// Inserts an error message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public void Error(string message, Exception exception = null)
        {
            if (!this.logInstance.IsErrorEnabled)
            {
                return;
            }

            ThreadContext.Properties["AppId"] = SystemSettings.ApplicationId;

            if (!HttpContext.Current.User.IsNull() && 
                HttpContext.Current.User.Identity.IsAuthenticated)
            {
                MDC.Set("user", HttpContext.Current.User.Identity.Name);
            }

            MDC.Set("ipaddress", WebHelper.GetUserHostAddress());
            MDC.Set("pageurl", WebHelper.GetThisPageUrl(true));
            MDC.Set("referrerurl", WebHelper.GetUrlReferrer());

            if (exception.IsNull())
            {
                this.logInstance.Error(message);
            }
            else
            {
                this.logInstance.Error(message, exception);
            }
        }

        /// <summary>
        /// Inserts a fatal message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public void Fatal(string message, Exception exception = null)
        {
            if (!this.logInstance.IsFatalEnabled)
            {
                return;
            }

            ThreadContext.Properties["AppId"] = SystemSettings.ApplicationId;
            if (!HttpContext.Current.User.IsNull() && 
                HttpContext.Current.User.Identity.IsAuthenticated)
            {
                MDC.Set("user", HttpContext.Current.User.Identity.Name);
            }

            MDC.Set("ipaddress", WebHelper.GetUserHostAddress());
            MDC.Set("pageurl", WebHelper.GetThisPageUrl(true));
            MDC.Set("referrerurl", WebHelper.GetUrlReferrer());

            if (exception.IsNull())
            {
                this.logInstance.Fatal(message);
            }
            else
            {
                this.logInstance.Fatal(message, exception);
            }
        }
        
        #endregion
    }
}
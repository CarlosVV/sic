namespace Nagnoi.SiC.Infrastructure.Core.Log
{
    #region Imports

    using System;

    #endregion

    /// <summary>
    /// Provides a set of extension methods for ILogger interface
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Inserts a debug message
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public static void Debug(this ILogger logger, string message, Exception exception = null)
        {
            ((ILogProvider)logger).Debug(message, exception);
        }

        /// <summary>
        /// Inserts an information message
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public static void Information(this ILogger logger, string message, Exception exception = null)
        {
            ((ILogProvider)logger).Information(message, exception);
        }

        /// <summary>
        /// Inserts a warning message
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public static void Warning(this ILogger logger, string message, Exception exception = null)
        {
            ((ILogProvider)logger).Warning(message, exception);
        }

        /// <summary>
        /// Inserts an error message
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public static void Error(this ILogger logger, string message, Exception exception = null)
        {
            ((ILogProvider)logger).Error(message, exception);
        }

        /// <summary>
        /// Inserts a fatal message
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        public static void Fatal(this ILogger logger, string message, Exception exception = null)
        {
            ((ILogProvider)logger).Fatal(message, exception);
        }
    }
}
namespace Nagnoi.SiC.Infrastructure.Core.Log
{
    #region Imports

    using System;

    #endregion

    /// <summary>
    /// Represents the interface of Log Provider
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// Inserts a debug message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        void Debug(string message, Exception exception = null);

        /// <summary>
        /// Inserts an information message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        void Information(string message, Exception exception = null);

        /// <summary>
        /// Inserts a warning message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        void Warning(string message, Exception exception = null);

        /// <summary>
        /// Inserts an error message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        void Error(string message, Exception exception = null);

        /// <summary>
        /// Inserts a fatal message
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception instance</param>
        void Fatal(string message, Exception exception = null);
    }
}
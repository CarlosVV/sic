namespace Nagnoi.SiC.Domain.Core.Model
{
    #region Imports

    using System;

    #endregion

    /// <summary>
    /// Log entry interface
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// Gets or sets the identifier
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the LogProvider (ELMAH, Log4Net, etc.)
        /// </summary>
        LogProviderType LogProvider { get; set; }

        /// <summary>
        /// Gets or sets the UTC datetime of the log entry
        /// </summary>
        DateTime LoggingDate { get; set; }

        /// <summary>
        /// Gets or sets the information about where the error occurred
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// Gets or sets the name of the machine where the error occurred
        /// </summary>
        string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the name of the class that logged the error
        /// </summary>
        string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the level of the log entry (information, Warning, Error, etc.)
        /// </summary>
        LogLevelType LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the application identifier
        /// </summary>
        int ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the message that was logged
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the number of occurrences that the same message was logged
        /// </summary>
        long Occurrences { get; set; }

        /// <summary>
        /// Gets or sets the Application name
        /// Used only for Elmah when filtering entries that point to an equivalent Elmah details page or not
        /// </summary>
        string Application { get; set; }
    }
}

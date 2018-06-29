namespace Nagnoi.SiC.Domain.Core.Model
{
    #region Imports
        
    using System;
    using System.Collections.Generic;
    using System.Data.Common;

    #endregion

    public class Log4NetLogEntry :  ILogEntry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogEntry"/> class.
        /// </summary>
        public Log4NetLogEntry()
        {
            this.LogProvider = LogProviderType.Log4Net;
            this.Occurrences = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogEntry"/> class.
        /// from data reader row
        /// </summary>
        /// <param name="reader">Data Reader instance</param>
        public Log4NetLogEntry(DbDataReader dr)
        {
            this.LogProvider = LogProviderType.Log4Net;
            this.Occurrences = 1;

            IDictionary<string, int> hDict = null;

            Log4NetLogEntry current = this;

            this.setFromDataReader(dr, ref current, ref hDict);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLogEntry"/> class.
        /// from data reader row and dictionary instance
        /// </summary>
        /// <param name="reader">Data Reader instance</param>
        /// <param name="hDict">Dictionary instance with key values</param>
        public Log4NetLogEntry(DbDataReader dr, IDictionary<string, int> hDict)
        {
            this.LogProvider = LogProviderType.Log4Net;
            this.Occurrences = 1;

            Log4NetLogEntry current = this;

            this.setFromDataReader(dr, ref current, ref hDict);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the UTC datetime of the log event
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Thread
        /// </summary>
        public string Thread { get; set; }

        /// <summary>
        /// Gets or sets the Level
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the Logger
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// Gets or sets the User name
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the Exception
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// Gets or sets the Ip Address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the Page Url
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the Referrer Url
        /// </summary>
        public string ReferrerUrl { get; set; }

        #endregion

        #region ILogEntry Implementation

        /// <summary>
        /// Gets a LogProviderType enum item indicating the provider of the log entry
        /// </summary>
        public LogProviderType LogProvider { get; set; }

        /// <summary>
        /// Gets or sets the logging date
        /// Explicitely implements the LoggingDate property of ILogEntry
        /// Refers to Date
        /// </summary>
        public DateTime LoggingDate
        {
            get
            {
                return this.Date;
            }
            set
            {
                this.Date = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the class that raised the log entry
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the Source of the log entry
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the Machine Name
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the Log Level (Information, Warning, Error, etc.)
        /// Explicitely implements the LogLevel property of ILogEntry
        /// Refers to the Levl property and converts enum values to string and viceversa
        /// </summary>
        public LogLevelType LogLevel
        {
            get
            {
                if (this.Level == "INFO")
                {
                    return LogLevelType.Information;
                }
                else if (this.Level == "ERROR")
                {
                    return LogLevelType.Error;
                }
                else
                {
                    return LogLevelType.None;
                }
            }
            set
            {
                if (value == LogLevelType.Information)
                {
                    this.Level = "INFO";
                }
                else if (value == LogLevelType.Error)
                {
                    this.Level = "ERROR";
                }
                else
                {
                    this.Level = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Application Identifier
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the logged Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the number of occurrences that the same message was logged
        /// </summary>
        public long Occurrences { get; set; }

        /// <summary>
        /// Gets or sets the Application name
        /// </summary>
        public string Application { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets a new instance of the Log
        /// from data reader row
        /// </summary>
        /// <param name="reader">Data Reader instance</param>
        /// <param name="prevObject">Previous instance of Log</param>
        /// <param name="hDict">Dictionary instance with key values</param>
        public void setFromDataReader(DbDataReader dr, ref Log4NetLogEntry prevObject, ref IDictionary<string, int> hDict)
        {
        }

        #endregion

        public string Id { get; set; }
    }
}
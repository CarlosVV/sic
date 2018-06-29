namespace Nagnoi.SiC.Domain.Core.Model
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Data.Common;

    #endregion

    public class ElmahLogEntry :  ILogEntry
    {
        #region Constructors

        public ElmahLogEntry() 
        {
            this.LogProvider = LogProviderType.Elmah;
            this.LogLevel = LogLevelType.Error;
            this.Occurrences = 1;
        }

        public ElmahLogEntry(DbDataReader dr)
        {
            this.LogProvider = LogProviderType.Elmah;
            this.LogLevel = LogLevelType.Error;
            this.Occurrences = 1;

            IDictionary<string, int> hDict = null;
            ElmahLogEntry current = this;
            this.setFromDataReader(dr, ref current, ref hDict);
        }

        public ElmahLogEntry(DbDataReader dr, IDictionary<string, int> hDict)
        {
            this.LogProvider = LogProviderType.Elmah;
            this.LogLevel = LogLevelType.Error;
            this.Occurrences = 1;
            ElmahLogEntry current = this;
            this.setFromDataReader(dr, ref current, ref hDict);
        }

        #endregion

        #region Properties

        public Guid ErrorId { get; set; }
        public string Host { get; set; }
        public string Type { get; set; }
        public string User { get; set; }
        public int StatusCode { get; set; }
        public DateTime TimeUtc { get; set; }
        public int Sequence { get; set; }
        public string AllXml { get; set; }

        #endregion

        #region ILogEntry Implementation

        public LogProviderType LogProvider { get; set; }
        
        /// <summary>
        /// Gets or sets the logging date
        /// Explicitely implements the LoggingDate property of ILogEntry
        /// Refers to TimeUtc
        /// </summary>
        public DateTime LoggingDate
        {
            get
            {
                return this.TimeUtc;
            }
            set
            {
                this.TimeUtc = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the class that raised the log entry
        /// Explicitely implements the ClassName property of ILogEntry
        /// Refers to Type
        /// </summary>
        public string ClassName
        {
            get
            {
                return this.Type;
            }
            set
            {
                this.Type = value;
            }
        }

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
        /// </summary>
        public LogLevelType LogLevel { get; set; }

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
        public void setFromDataReader(DbDataReader dr, ref ElmahLogEntry prevObject, ref IDictionary<string, int> hDict)
        {
        }

        #endregion

        public string Id { get; set; }
    }
}
namespace Nagnoi.SiC.Domain.Core.Model
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Data.Common;    

    #endregion

    public class AspNetLogEntry: ILogEntry
    {
        #region Constructors

        public AspNetLogEntry() 
        {
            this.LogProvider = LogProviderType.AspNet;
        }

        public AspNetLogEntry(DbDataReader dr)
        {
            this.LogProvider = LogProviderType.AspNet;

            IDictionary<string, int> hDict = null;

            AspNetLogEntry current = this;

            this.setFromDataReader(dr, ref current, ref hDict);
        }

        public AspNetLogEntry(DbDataReader dr, IDictionary<string, int> hDict)
        {
            this.LogProvider = LogProviderType.AspNet;

            AspNetLogEntry current = this;

            this.setFromDataReader(dr, ref current, ref hDict);
        }

        #endregion

        #region Properties
        public string EventId { get; set; }
        public DateTime EventTimeUtc { get; set; }
        public DateTime EventTime { get; set; }
        public string EventType { get; set; }
        public decimal EventSequence { get; set; }
        public decimal EventOccurrence { get; set; }
        public int EventCode { get; set; }
        public int EventDetailCode { get; set; }
        public string ApplicationPath { get; set; }
        public string ApplicationVirtualPath { get; set; }
        public string RequestUrl { get; set; }
        public string ExceptionType { get; set; }
        public string Details { get; set; }
        #endregion

        #region ILogEntry Implementation
        public LogProviderType LogProvider { get; set; }      

        public DateTime LoggingDate
        {
            get
            {
                return this.EventTimeUtc;
            }
            set
            {
                this.EventTimeUtc = value;
            }
        }

        public string ClassName
        {
            get
            {
                return this.EventType;
            }
            set
            {
                this.EventType = value;
            }
        }

        public string Source { get; set; }
        public string MachineName { get; set; }
        public LogLevelType LogLevel
        {
            get
            {
                if (this.ExceptionType == null)
                {
                    return LogLevelType.Information;
                }
                else
                {
                    return LogLevelType.Error;
                }
            }
            set
            {
                if (value != LogLevelType.Error)
                {
                    this.ExceptionType = null;
                }
            }
        }

        public int ApplicationId { get; set; }

        public string Message { get; set; }

        public long Occurrences
        {
            get
            {
                return long.Parse(decimal.Round(this.EventOccurrence).ToString());
            }
            set
            {
                this.EventOccurrence = decimal.Parse(value.ToString());
            }
        }
        public string Application { get; set; }

        #endregion

        #region Public Methods

        public void setFromDataReader(DbDataReader dr, ref AspNetLogEntry prevObject, ref IDictionary<string, int> hDict)
        {
        }

        #endregion

        public string Id { get; set; }
    }
}
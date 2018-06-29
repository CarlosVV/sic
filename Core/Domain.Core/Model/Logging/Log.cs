namespace Nagnoi.SiC.Domain.Core.Model
{
    #region Imports

 
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Globalization;

    #endregion

    /// <summary>
    /// Represents the Log class
    /// </summary>
    [Serializable]
    public class Log 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log() 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// from data reader row
        /// </summary>
        /// <param name="reader">Data Reader instance</param>
        public Log(DbDataReader dr)
        {
            IDictionary<string, int> hDict = null;

            Log current = this;

            this.setFromDataReader(dr, ref current, ref hDict);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// from data reader row and dictionary instance
        /// </summary>
        /// <param name="reader">Data Reader instance</param>
        /// <param name="hDict">Dictionary instance with key values</param>
        public Log(DbDataReader dr, IDictionary<string, int> hDict)
        {
            Log current = this;

            this.setFromDataReader(dr, ref current, ref hDict);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the log identifier
        /// </summary>
        public int LogId { get; set; }

        /// <summary>
        /// Gets or sets the log type providerId
        /// </summary>
        public int LogTypeId { get; set; }

        /// <summary>
        /// Gets or sets the log type
        /// </summary>
        public LogLevelType LogType
        {
            get { return (LogLevelType)this.LogTypeId; }
            set { this.LogTypeId = Convert.ToInt32(value, NumberFormatInfo.InvariantInfo); }
        }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string ShortMessage { get; set; }

        /// <summary>
        /// Gets or sets the exception
        /// </summary>
        public string FullMessage { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the customer providerId
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the page url
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the referrer url
        /// </summary>
        public string ReferrerUrl { get; set; }

        /// <summary>
        /// Gets or sets the created on
        /// </summary>
        public DateTime CreatedOn { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets a new instance of the Log
        /// from data reader row
        /// </summary>
        /// <param name="reader">Data Reader instance</param>
        /// <param name="prevObject">Previous instance of Log</param>
        /// <param name="hDict">Dictionary instance with key values</param>
        public void setFromDataReader(DbDataReader dr, ref Log prevObject, ref IDictionary<string, int> hDict)
        {
        }

        #endregion
    }
}
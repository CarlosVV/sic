namespace Nagnoi.SiC.Infrastructure.Core.Log
{
    #region Imports

    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using Nagnoi.SiC.Infrastructure.Core.Configuration;
    using Elmah;

    #endregion

    /// <summary>
    /// Contains logic to log ELMAH error to database including a custo field ApplicationId
    /// </summary>
    public class ElmahSqlErrorLog : SqlErrorLog
    {
        #region Constants

        /// <summary>
        /// Maximun length of Application Name
        /// </summary>
        private const int _maxAppNameLength = 60;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ElmahSqlErrorLog"/> class
        /// </summary>
        /// <param name="config">a dictionary containing configurations</param>
        public ElmahSqlErrorLog(IDictionary config) : base(config)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElmahSqlErrorLog"/> class
        /// </summary>
        /// <param name="connectionString">the connection string to the SQL database where the errors will be logged</param>
        public ElmahSqlErrorLog(string connectionString)
            : base(connectionString)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="error">the error data</param>
        /// <returns>the new ErrorId created in the table</returns>
        public override string Log(Error error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            string errorXml = ErrorXml.EncodeString(error);
            Guid id = Guid.NewGuid();

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                using (SqlCommand command = CommandsWithApplicationId.LogError(
                                                                                id, 
                                                                                this.ApplicationName,
                                                                                error.HostName, 
                                                                                error.Type, 
                                                                                error.Source, 
                                                                                error.Message, 
                                                                                error.User,
                                                                                error.StatusCode, 
                                                                                error.Time.ToUniversalTime(), 
                                                                                errorXml)
                                                                              )
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();

                    return id.ToString();
                }
            }
        }

        #endregion

        /// <summary>
        /// Replaces and customizes the Commands class (which is private and sealed in the base implementation)
        /// Add the ApplicationId parameter and item
        /// </summary>
        private sealed class CommandsWithApplicationId
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="CommandsWithApplicationId"/> class
            /// </summary>
            private CommandsWithApplicationId() { }

            #endregion

            #region Public Methods

            /// <summary>
            /// Generates the SqlCommand used to log a new record into the ELMAH_Error table
            /// </summary>
            /// <param name="id">the error identifier</param>
            /// <param name="appName">the application name</param>
            /// <param name="hostName">the host name</param>
            /// <param name="typeName">the type name</param>
            /// <param name="source">the source</param>
            /// <param name="message">the message</param>
            /// <param name="user">the user</param>
            /// <param name="statusCode">the status code</param>
            /// <param name="time">the time</param>
            /// <param name="xml">the xml</param>
            /// <returns>a SqlCommand object</returns>
            public static SqlCommand LogError(
                                                Guid id,
                                                string appName,
                                                string hostName,
                                                string typeName,
                                                string source,
                                                string message,
                                                string user,
                                                int statusCode,
                                                DateTime time,
                                                string xml
                                            )
            {
                SqlCommand command = new SqlCommand("ELMAH_LogError");
                command.CommandType = CommandType.StoredProcedure;

                SqlParameterCollection parameters = command.Parameters;

                parameters.Add("@ErrorId", SqlDbType.UniqueIdentifier).Value = id;
                parameters.Add("@Application", SqlDbType.NVarChar, _maxAppNameLength).Value = appName;
                parameters.Add("@Host", SqlDbType.NVarChar, 30).Value = hostName;
                parameters.Add("@Type", SqlDbType.NVarChar, 100).Value = typeName;
                parameters.Add("@Source", SqlDbType.NVarChar, 60).Value = source;
                parameters.Add("@Message", SqlDbType.NVarChar, 500).Value = message;
                parameters.Add("@User", SqlDbType.NVarChar, 50).Value = user;
                parameters.Add("@AllXml", SqlDbType.NText).Value = xml;
                parameters.Add("@StatusCode", SqlDbType.Int).Value = statusCode;
                parameters.Add("@TimeUtc", SqlDbType.DateTime).Value = time;
                
                /* Custom Code */
                int applicationId = SystemSettings.ApplicationId;
                parameters.Add("@ApplicationId", SqlDbType.Int).Value = applicationId;

                return command;
            }

            /// <summary>
            /// Returns the SqlCommand used to retrieve a specific error's AllXml data
            /// </summary>
            /// <param name="appName">the application name</param>
            /// <param name="id">the error identifier</param>
            /// <returns>a SqlCommand object</returns>
            public static SqlCommand GetErrorXml(string appName, Guid id)
            {
                SqlCommand command = new SqlCommand("ELMAH_GetErrorXml");
                command.CommandType = CommandType.StoredProcedure;

                SqlParameterCollection parameters = command.Parameters;
                parameters.Add("@Application", SqlDbType.NVarChar, _maxAppNameLength).Value = appName;
                parameters.Add("@ErrorId", SqlDbType.UniqueIdentifier).Value = id;

                return command;
            }

            /// <summary>
            /// Returns the SqlCommand used to retrieve a paginate list of Errors, as Xml
            /// </summary>
            /// <param name="appName">the application name</param>
            /// <param name="pageIndex">the page index</param>
            /// <param name="pageSize">the page size</param>
            /// <returns>a SqlCommand object</returns>
            public static SqlCommand GetErrorsXml(string appName, int pageIndex, int pageSize)
            {
                SqlCommand command = new SqlCommand("ELMAH_GetErrorsXml");
                command.CommandType = CommandType.StoredProcedure;

                SqlParameterCollection parameters = command.Parameters;

                parameters.Add("@Application", SqlDbType.NVarChar, _maxAppNameLength).Value = appName;
                parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
                parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                parameters.Add("@TotalCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                return command;
            }

            public static void GetErrorsXmlOutputs(SqlCommand command, out int totalCount)
            {
                Debug.Assert(command != null);

                totalCount = (int)command.Parameters["@TotalCount"].Value;
            }

            #endregion
        }
    }
}
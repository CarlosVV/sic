namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Text;

    #endregion

    /// <summary>
    /// Provides a set of methods to create excel documents
    /// </summary>
    public class ExcelFreeAutomationHelper : IDisposable
    {
        #region Private Members

        /// <summary>
        /// Excel object string
        /// </summary>
        private string excelObject = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=\"Excel {3};HDR={4};IMEX={5}\"";

        /// <summary>
        /// File path
        /// </summary>
        private string filePath = string.Empty;

        /// <summary>
        /// Excel HDR
        /// </summary>
        private string hdr = "No";

        /// <summary>
        /// Excel IMEX
        /// </summary>
        private string imex = "1";

        /// <summary>
        /// OLEDB connection instance
        /// </summary>
        private OleDbConnection connection;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFreeAutomationHelper"/> class
        /// </summary>
        /// <param name="filePath">File path</param>
        public ExcelFreeAutomationHelper(string filePath)
        {
            this.filePath = filePath;
        }

        #endregion

        #region Destructor

        ~ExcelFreeAutomationHelper()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the connection string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                string result = string.Empty;

                if (string.IsNullOrEmpty(this.filePath))
                {
                    return result;
                }

                // Check the file format
                FileInfo fileInfo = new FileInfo(this.filePath);
                if (fileInfo.Extension.Equals(".xls"))
                {
                    result = string.Format(this.excelObject, "Jet", "4.0", this.filePath, "8.0", this.hdr, this.imex);
                }
                else if (fileInfo.Extension.Equals(".xlsx"))
                {
                    result = string.Format(this.excelObject, "Ace", "12.0", this.filePath, "12.0", this.hdr, this.imex);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the connection instance
        /// </summary>
        public OleDbConnection Connection
        {
            get
            {
                if (this.connection == null)
                {
                    this.connection = new OleDbConnection(this.ConnectionString);
                }

                return this.connection;
            }
        }

        /// <summary>
        /// Gets or sets a HDR
        /// </summary>
        public string Hdr
        {
            get
            {
                return this.hdr;
            }
            set
            {
                this.hdr = value;
            }
        }

        /// <summary>
        /// Gets or sets an IMEX
        /// </summary>
        public string Imex
        {
            get
            {
                return this.imex;
            }
            set
            {
                this.imex = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a schema
        /// </summary>
        /// <returns>Returns the data table schema</returns>
        public DataTable GetSchema()
        {
            DataTable schema = null;

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            schema = this.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            return schema;
        }

        /// <summary>
        /// Read all table rows
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>Returns the table</returns>
        public DataTable ReadTable(string tableName)
        {
            return this.ReadTable(tableName, ExcelHelperReadTableMode.ReadFromWorkSheet);
        }

        /// <summary>
        /// Read table
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="mode">Read mode</param>
        /// <returns>Returns the table</returns>
        public DataTable ReadTable(string tableName, ExcelHelperReadTableMode mode)
        {
            return this.ReadTable(tableName, mode, string.Empty);
        }

        /// <summary>
        /// Read table
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="mode">Read mode</param>
        /// <param name="criteria">Criteria search</param>
        /// <returns>Returns the table</returns>
        public DataTable ReadTable(string tableName, ExcelHelperReadTableMode mode, string criteria)
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            string commandText = "Select * from [{0}]";
            if (!string.IsNullOrEmpty(criteria))
            {
                commandText = " Where " + criteria;
            }

            string tableNameSuffix = string.Empty;
            if (mode == ExcelHelperReadTableMode.ReadFromWorkSheet)
            {
                tableNameSuffix = "$";
            }

            OleDbCommand command = new OleDbCommand(string.Format(commandText, tableName + tableNameSuffix));

            command.Connection = this.Connection;

            OleDbDataAdapter adapter = new OleDbDataAdapter(command);

            DataSet result = new DataSet();

            adapter.Fill(result, tableName);

            if (result.Tables.Count >= 1)
            {
                return result.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Drop table
        /// </summary>
        /// <param name="tableName">Table name</param>
        public void DropTable(string tableName)
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            string commandText = "Drop Table [{0}]";
            using (OleDbCommand command = new OleDbCommand(string.Format(commandText, tableName), this.Connection))
            {
                command.ExecuteNonQuery();
            }

            this.Connection.Close();
        }

        /// <summary>
        /// Write table
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="tableDefinition">Table definition</param>
        public void WriteTable(string tableName, Dictionary<string, string> tableDefinition)
        {
            using (OleDbCommand command = new OleDbCommand(this.GenerateCreateTable(tableName, tableDefinition), this.Connection))
            {
                if (this.Connection.State != ConnectionState.Open)
                {
                    this.Connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Set new row
        /// </summary>
        /// <param name="row">Date row</param>
        public void AddNewRow(DataRow row)
        {
            string commandText = this.GenerateInsertStatement(row);

            this.ExecuteCommand(commandText);
        }

        /// <summary>
        /// Execute any command
        /// </summary>
        /// <param name="commandText">Command text</param>
        public void ExecuteCommand(string commandText)
        {
            using (OleDbCommand command = new OleDbCommand(commandText, this.Connection))
            {
                if (this.Connection.State != ConnectionState.Open)
                {
                    this.Connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Disposes the class
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates create table script
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="tableDefinition">Table definition</param>
        /// <returns>Returns the create table script</returns>
        private string GenerateCreateTable(string tableName, Dictionary<string, string> tableDefinition)
        {
            StringBuilder statement = new StringBuilder();
            bool isFirstColumn = true;

            statement.AppendFormat("CREATE TABLE [{0}] (", tableName);

            isFirstColumn = true;

            foreach (KeyValuePair<string, string> keyValue in tableDefinition)
            {
                if (!isFirstColumn)
                {
                    statement.Append(",");
                }

                isFirstColumn = false;

                statement.AppendFormat("{0} {1}", keyValue.Key, keyValue.Value);
            }

            statement.Append(")");

            return statement.ToString();
        }

        /// <summary>
        /// Generates insert statement script
        /// </summary>
        /// <param name="row">Data row</param>
        /// <returns>Returns the insert statement</returns>
        private string GenerateInsertStatement(DataRow row)
        {
            StringBuilder statement = new StringBuilder();
            bool isFirstColumn = true;

            statement.AppendFormat("INSERT INTO [{0}](", row.Table.TableName);

            foreach (DataColumn column in row.Table.Columns)
            {
                if (!isFirstColumn)
                {
                    statement.Append(",");
                }

                isFirstColumn = false;

                statement.Append(column.Caption);
            }

            statement.Append(") VALUES(");

            isFirstColumn = true;

            for (int index = 0; index < row.Table.Columns.Count - 1; index++)
            {
                if (!object.ReferenceEquals(row.Table.Columns[index].DataType, typeof(int)))
                {
                    statement.Append("'");
                    statement.Append(row[index].ToString().Replace("'", "''"));
                    statement.Append("'");
                }
                else
                {
                    statement.Append(row[index].ToString().Replace("'", "''"));
                }

                if (index != row.Table.Columns.Count - 1)
                {
                    statement.Append("'");
                }
            }

            statement.Append("'");

            return statement.ToString();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (this.connection != null && this.connection.State == ConnectionState.Open)
                {
                    this.connection.Close();
                }

                if (this.connection != null)
                {
                    this.connection.Dispose();
                }

                this.connection = null;
                this.filePath = string.Empty;
            }
        }

        #endregion
    }
}
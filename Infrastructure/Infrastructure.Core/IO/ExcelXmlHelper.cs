namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using System;
    using System.Data;
    using System.IO;
    using System.Text;

    #endregion

    /// <summary>
    /// Provides a set of methods to export an excel file
    /// using the XML Spreadsheet
    /// </summary>
    public static class ExcelXmlHelper
    {
        #region Private Members

        /// <summary>
        /// Maximum limit of rows
        /// </summary>
        private const int RowLimit = 65000;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the XML definition of workbook template
        /// </summary>
        /// <returns>Returns the XML namespaces</returns>
        public static string GetWorkbookTemplate()
        {
            StringBuilder sb = new StringBuilder(818);

            sb.AppendFormat(@"<?xml version=""1.0""?>{0}", Environment.NewLine);

            sb.AppendFormat(@"<?mso-application progid=""Excel.Sheet""?>{0}", Environment.NewLine);

            sb.AppendFormat(@"<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);

            sb.AppendFormat(@" xmlns:o=""urn:schemas-microsoft-com:office:office""{0}", Environment.NewLine);

            sb.AppendFormat(@" xmlns:x=""urn:schemas-microsoft-com:office:excel""{0}", Environment.NewLine);

            sb.AppendFormat(@" xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""{0}", Environment.NewLine);

            sb.AppendFormat(@" xmlns:html=""http://www.w3.org/TR/REC-html40"">{0}", Environment.NewLine);

            sb.AppendFormat(@" <Styles>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Style ss:ID=""Default"" ss:Name=""Normal"">{0}", Environment.NewLine);

            sb.AppendFormat(@" <Alignment ss:Vertical=""Bottom""/>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Borders/>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""/>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Interior/>{0}", Environment.NewLine);

            sb.AppendFormat(@" <NumberFormat/>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Protection/>{0}", Environment.NewLine);

            sb.AppendFormat(@" </Style>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Style ss:ID=""BoldColumn"">{0}", Environment.NewLine);

            sb.AppendFormat(@" <Font ss:FontName=""Calibri"" x:Family=""Swiss"" ss:Size=""11"" ss:Color=""#000000""{0}", Environment.NewLine);

            sb.AppendFormat(@" ss:Bold=""1""/>{0}", Environment.NewLine);

            sb.AppendFormat(@" </Style>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Style ss:ID=""DateLiteral"">{0}", Environment.NewLine);

            sb.AppendFormat(@" <NumberFormat ss:Format=""Short Date""/>{0}", Environment.NewLine);

            sb.AppendFormat(@" </Style>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Style ss:ID=""Decimal"">{0}", Environment.NewLine);

            sb.AppendFormat(@" <NumberFormat ss:Format=""#,##0.00""/>{0}", Environment.NewLine);

            sb.AppendFormat(@" </Style>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Style ss:ID=""StringLiteral"">{0}", Environment.NewLine);

            sb.AppendFormat(@" <NumberFormat ss:Format=""@""/>{0}", Environment.NewLine);

            sb.AppendFormat(@" </Style>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Style ss:ID=""Integer"">{0}", Environment.NewLine);

            sb.AppendFormat(@" <NumberFormat ss:Format=""0""/>{0}", Environment.NewLine);

            sb.AppendFormat(@" </Style>{0}", Environment.NewLine);

            sb.AppendFormat(@" <Style ss:ID=""ErrorLiteral"">{0}", Environment.NewLine);

            sb.AppendFormat(@" <Interior ss:Color=""#FF0000"" ss:Pattern=""Solid""/>{0}", Environment.NewLine);

            sb.AppendFormat(@" </Style>{0}", Environment.NewLine);

            sb.AppendFormat(@" </Styles>{0}", Environment.NewLine);

            sb.Append(@"{0}\r\n</Workbook>");

            return sb.ToString();
        }

        /// <summary>
        /// Gets the XML excel from data table
        /// </summary>
        /// <param name="input">DataTable instance</param>
        /// <returns>Returns the xml string</returns>
        public static string GetExcelXml(DataTable input)
        {
            string excelTemplate = GetWorkbookTemplate();

            DataSet ds = new DataSet();

            ds.Tables.Add(input.Copy());

            string worksheets = GetWorksheets(ds);

            string excelXml = string.Format(excelTemplate, worksheets);

            return excelXml;
        }

        /// <summary>
        /// Gets the XML excel from dataset
        /// </summary>
        /// <param name="input">DataTable instance</param>
        /// <returns>Returns the xml string</returns>
        public static string GetExcelXml(DataSet input)
        {
            string excelTemplate = GetWorkbookTemplate();

            string worksheets = GetWorksheets(input);

            string excelXml = string.Format(excelTemplate, worksheets);

            return excelXml;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Replaces the strange characters
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Returns the clean string</returns>
        private static string ReplaceXmlChar(string input)
        {
            input = input.Replace("&", "&");

            input = input.Replace("<", "<");

            input = input.Replace(">", ">");

            input = input.Replace("\"", "'");

            input = input.Replace("'", "'");

            return input;
        }

        /// <summary>
        /// Gets the xml cell
        /// </summary>
        /// <param name="type">Data type</param>
        /// <param name="cellData">Cell content</param>
        /// <returns>Returns the xml string</returns>
        private static string GetCell(Type type, object cellData)
        {
            object data = (cellData is DBNull) ? string.Empty : cellData;

            if (type.Name.Contains("Int") && data.ToString() != string.Empty)
            {
                return string.Format("<Cell ss:StyleID=\"Integer\"><Data ss:Type=\"Number\">{0}</Data></Cell>", data);
            }

            if (type.Name.Contains("Double") || type.Name.Contains("Decimal"))
            {
                return string.Format("<Cell ss:StyleID=\"Decimal\"><Data ss:Type=\"Number\">{0}</Data></Cell>", data);
            }

            if (type.Name.Contains("Date") && data.ToString() != string.Empty)
            {
                return string.Format("<Cell ss:StyleID=\"DateLiteral\"><Data ss:Type=\"DateTime\">{0}</Data></Cell>", Convert.ToDateTime(data).ToString("yyyy-MM-dd"));
            }

            return string.Format("<Cell ss:StyleID=\"StringLiteral\"><Data ss:Type=\"String\">{0}</Data></Cell>", ReplaceXmlChar(data.ToString()));
        }

        /// <summary>
        /// Gets the error cell
        /// </summary>
        /// <param name="type">Data type</param>
        /// <param name="cellData">Cell content</param>
        /// <returns>Returns the xml string</returns>
        private static string GetErrorCell(Type type, object cellData)
        {
            object data = (cellData is DBNull) ? string.Empty : cellData;

            if (type.Name.Contains("Int"))
            {
                return string.Format("<Cell ss:StyleID=\"Integer\"><Data ss:Type=\"Number\">{0}</Data></Cell>", data);
            }

            if (type.Name.Contains("Double") || type.Name.Contains("Decimal"))
            {
                return string.Format("<Cell ss:StyleID=\"Decimal\"><Data ss:Type=\"Number\">{0}</Data></Cell>", data);
            }

            if (type.Name.Contains("Date") && data.ToString() != string.Empty)
            {
                return string.Format("<Cell ss:StyleID=\"DateLiteral\"><Data ss:Type=\"DateTime\">{0}</Data></Cell>", Convert.ToDateTime(data).ToString("yyyy-MM-dd"));
            }

            return string.Format("<Cell ss:StyleID=\"ErrorLiteral\"><Data ss:Type=\"String\">{0}</Data></Cell>", ReplaceXmlChar(data.ToString()));
        }

        /// <summary>
        /// Gets the definition of worksheets
        /// </summary>
        /// <param name="source">Source data</param>
        /// <returns>Returns the worksheets string</returns>
        private static string GetWorksheets(DataSet source)
        {
            StringWriter writer = new StringWriter();

            if (source == null || source.Tables.Count == 0)
            {
                writer.Write("<Worksheet ss:Name=\"Sheet1\">\r\n<Table>\r\n<Row><Cell><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");

                return writer.ToString();
            }

            foreach (DataTable dt in source.Tables)
            {
                if (dt.Rows.Count == 0)
                {
                    writer.Write("<Worksheet ss:Name=\"" + ReplaceXmlChar(dt.TableName) +
                        "\">\r\n<Table>\r\n<Row><Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\"></Data></Cell></Row>\r\n</Table>\r\n</Worksheet>");
                }
                else
                {
                    // write each row data 
                    int sheetCount = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if ((i % RowLimit) == 0)
                        {
                            // add close tags for previous sheet of the same data table 
                            if ((i / RowLimit) > sheetCount)
                            {
                                writer.Write("\r\n </Table> \r\n</Worksheet>");

                                sheetCount = i / RowLimit;
                            }

                            writer.Write("\r\n<Worksheet ss:Name=\"" + ReplaceXmlChar(dt.TableName) + (((i / RowLimit) == 0) ? string.Empty : Convert.ToString(i / RowLimit)) + "\">\r\n<Table>");
                            foreach (DataColumn dc in dt.Columns)
                            {
                                writer.Write("<Column  ss:AutoFitWidth=\"1\" ss:Width=\"90\"/>");
                            }

                            // write column name row 
                            writer.Write("\r\n<Row>");

                            foreach (DataColumn dc in dt.Columns)
                            {
                                writer.Write(string.Format("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">{0}</Data></Cell>", ReplaceXmlChar(dc.Caption)));
                            }

                            writer.Write("</Row>");
                        }

                        writer.Write("\r\n<Row>");

                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dt.Rows[i].GetColumnError(dc.ColumnName).Equals("MUPException"))
                            {
                                writer.Write(GetErrorCell(dc.DataType, dt.Rows[i][dc.ColumnName]));
                            }
                            else
                            {
                                writer.Write(GetCell(dc.DataType, dt.Rows[i][dc.ColumnName]));
                            }
                        }

                        writer.Write("</Row>");
                    }

                    writer.Write("\r\n</Table>\r\n</Worksheet>");
                }
            }

            return writer.ToString();
        }

        #endregion
    }
}
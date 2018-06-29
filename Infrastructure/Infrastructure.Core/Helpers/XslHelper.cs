namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;

    #endregion

    /// <summary>
    /// Represents XSL Helper
    /// </summary>
    public static class XslHelper
    {
        #region Public Methods

        /// <summary>
        /// Transform an xml object to XSLT Format
        /// </summary>
        /// <param name="xml">XML string object</param>
        /// <param name="xsltFilePath">XSLT file path</param>
        /// <returns>Returns the applicationHost format of XSLT setting</returns>
        public static string TransformFromXml(string xml, string xsltFilePath)
        {
            MemoryStream stream = null;
            StreamReader reader = null;

            try
            {
                // Encode all xml format string to bytes
                byte[] bytes = Encoding.ASCII.GetBytes(xml);

                stream = new MemoryStream(bytes);

                XmlReader xmlReader = XmlReader.Create(stream);

                stream = null;

                // Load XML
                XPathDocument document = new XPathDocument(xmlReader);

                XslCompiledTransform xsltFormat = new XslCompiledTransform();

                // Load the style sheet
                xsltFormat.Load(xsltFilePath);

                stream = new MemoryStream();

                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.ASCII);

                // Apply transformation from xml format to html format and save it in xmltextwriter
                xsltFormat.Transform(document, writer);

                writer = null;

                reader = new StreamReader(stream);

                stream.Position = 0;

                return reader.ReadToEnd();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }

                if (reader != null)
                {
                    reader.Dispose();
                }
            }
        }

        #endregion
    }
}
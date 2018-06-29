namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    /// Xml Helper
    /// </summary>
    public static class XmlHelper
    {
        #region Public Methods

        /// <summary>
        /// XML Encode
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncode(string str)
        {
            if (str == null)
            {
                return null;
            }

            str = Regex.Replace(str, @"[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", string.Empty, RegexOptions.Compiled);

            return XmlEncodeAsIs(str);
        }

        /// <summary>
        /// XML Encode as is
        /// </summary>
        /// <param name="input">String instance</param>
        /// <returns>Encoded string</returns>
        public static string XmlEncodeAsIs(string input)
        {
            if (input == null)
            {
                return null;
            }

            StringWriter stringWriter = null;
            XmlTextWriter textWriter = null;

            try
            {
                stringWriter = new StringWriter();
                textWriter = new XmlTextWriter(stringWriter);

                textWriter.WriteString(input);

                return stringWriter.ToString();
            }
            finally
            {
                if (stringWriter != null)
                {
                    stringWriter.Dispose();
                }
            }
        }

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <typeparam name="T">a Class</typeparam>
        /// <param name="objectoToSerialize">An object</param>
        /// <returns>Xml string</returns>
        public static string SerializeObject<T>(object objectoToSerialize) where T : class, new()
        {
            XmlSerializerNamespaces namespaceSerializer = new XmlSerializerNamespaces();
            namespaceSerializer.Add(string.Empty, string.Empty);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings writerSettings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Encoding = new UTF8Encoding(false)
            };

            StringBuilder builder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(builder, writerSettings))
            {
                serializer.Serialize(writer, objectoToSerialize, namespaceSerializer);

                writer.Flush();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Deserialize an object
        /// </summary>
        /// <typeparam name="T">a Class</typeparam>
        /// <param name="xmlToDeserialize">An xml string</param>
        /// <returns>Object instance</returns>
        public static T DeserializeObject<T>(string xmlToDeserialize) where T : class, new()
        {
            if (xmlToDeserialize == null)
            {
                throw new ArgumentNullException("xmlToDeserialize");
            }

            T result = default(T);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xmlToDeserialize))
            {
                XmlReader resultReader = XmlReader.Create(reader);

                result = serializer.Deserialize(resultReader) as T;
            }

            return result;
        }

        /// <summary>
        /// Serializes a list of identifiers to XML string
        /// </summary>
        /// <param name="rootName">XML root name</param>
        /// <param name="values">List of values</param>
        /// <returns>Returns a XML</returns>
        public static string BuildXmlStringIds(string rootName, string[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();

            result.AppendFormat("<{0}>", rootName);
            for (int index = 0; index < values.Length; index++)
            {
                result.AppendFormat("<Id>{0}</Id>", values[index]);
            }
            result.AppendFormat("</{0}>", rootName);

            return result.ToString();
        }

        /// <summary>
        /// Serializes a list of codes to XML string
        /// </summary>
        /// <param name="rootName">XML root name</param>
        /// <param name="values">List of values</param>
        /// <returns>Returns a XML</returns>
        public static string BuildXmlStringCodes(string rootName, string[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();

            result.AppendFormat("<{0}>", rootName);
            for (int index = 0; index < values.Length; index++)
            {
                result.AppendFormat("<Code>{0}</Code>", values[index]);
            }
            result.AppendFormat("</{0}>", rootName);

            return result.ToString();
        }

        /// <summary>
        /// Serializes a dictionary to XML string
        /// </summary>
        /// <typeparam name="TKey">Any object</typeparam>
        /// <typeparam name="TValue">Any object</typeparam>
        /// <param name="rootName">XML root name</param>
        /// <param name="values">List of ke item pairs</param>
        /// <returns>Returns a XML</returns>
        public static string SerializeKeyValuePairs<TKey, TValue>(string rootName, IDictionary<TKey, TValue> values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();

            result.AppendFormat("<{0}>", rootName);
            foreach (var item in values)
            {
                result.Append("<KeyValue>");

                result.AppendFormat("<Key>{0}</Key>", item.Key);
                result.AppendFormat("<Value>{0}</Value>", item.Value);

                result.Append("</KeyValue>");
            }
            result.AppendFormat("</{0}>", rootName);

            return result.ToString();
        }

        /// <summary>
        /// Serializes a lookup to XML string
        /// </summary>
        /// <typeparam name="TKey">Any object</typeparam>
        /// <typeparam name="TValue">Any object</typeparam>
        /// <param name="rootName">XML root name</param>
        /// <param name="values">List of the item pairs</param>
        /// <returns></returns>

        public static string SerializeKeyValuePairsFromLookup<TKey, TValue>(string rootName, ILookup<TKey, TValue> values)
        {
            if(values.IsNullOrEmpty())
            {
                return null;
            }

            StringBuilder result = new StringBuilder();

            result.AppendFormat("<{0}>", rootName);

            foreach (IGrouping<string, string> itemValue in values)
            {
                foreach(var item in itemValue)
                {
                    result.Append("<KeyValue>");

                    result.AppendFormat("<Key>{0}</Key>", itemValue.Key);
                    result.AppendFormat("<Value>{0}</Value>", item);

                    result.Append("</KeyValue>");
                }                
            }

            result.AppendFormat("</{0}>", rootName);

            return result.ToString();
        }

        #endregion
    }
}
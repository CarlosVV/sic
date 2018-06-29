namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using Nagnoi.SiC.Infrastructure.Core.IO;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Web.Script.Serialization;
    using System.Xml;
    using System.Xml.Serialization;
    

    #endregion

    /// <summary>
    /// Extension class
    /// </summary>
    public static class BasicExtensions
    {
        /// <summary>
        /// Tries to cast on object to specific type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="o">Object instance</param>
        /// <param name="applicationHost">Converted type</param>
        /// <returns>Returns true or false</returns>
        public static bool TryCast<T>(this object o, out T result)
        {
            if (o is T)
            {
                result = (T)o;
                return true;
            }

            result = default(T);
            return false;
        }

        /// <summary>
        /// Cast from any object to T generic
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="o">Object instance</param>
        /// <param name="t">T generic</param>
        /// <returns>T instance</returns>
        public static T CastTo<T>(this object o, T t)
        {
            return (T)o;
        }

        /// <summary>
        /// Converts an object to a string representation of it in JSON format
        /// </summary>
        /// <param name="obj">Any object</param>
        /// <returns>Returns a JSON string</returns>
        public static string ToJson(this object obj)
        {
            return ToJson(obj, 100);
        }

        /// <summary>
        /// Converts an object to a string representation of it in JSON format
        /// </summary>
        /// <param name="obj">Any object</param>
        /// <param name="recursionDepth">Recursion Depth</param>
        /// <returns>Returns a JSON string</returns>
        public static string ToJson(this object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            if (recursionDepth > 0)
            {
                serializer.RecursionLimit = recursionDepth;
            }

            return serializer.Serialize(obj);
        }

        /// <summary>
        /// Checks if object is null
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Returns true or false</returns>
        public static bool IsNull(this object source)
        {
            return source == null;
        }

        /// <summary>
        /// Throw an argument null exception if object is null
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">Object instance</param>
        /// <param name="parameterName">Parameter name</param>
        public static void ThrowIfNull<T>(this T obj, string parameterName) where T : class
        {
            obj.ThrowIfNull(parameterName, string.Empty);
        }

        /// <summary>
        /// Throw an argument null exception if object is null
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">Object instance</param>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="message">Exception message</param>
        public static void ThrowIfNull<T>(this T obj, string parameterName, string message) where T : class
        {
            if (obj == null)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new ArgumentNullException(parameterName);
                }
                else
                {
                    throw new ArgumentNullException(parameterName, message);
                }
            }
        }

        /// <summary>
        /// Disposes an object if is null
        /// </summary>
        /// <param name="source">Source object</param>
        public static void DisposeIfNotNull(this IDisposable source)
        {
            if (source != null)
            {
                source.Dispose();
            }
        }

        /// <summary>
        /// Convert from enumeration type to list of codes
        /// </summary>
        /// <param name="enumType">Enumeration type</param>
        /// <returns>List of key values</returns>
        public static List<KeyValuePair<string, string>> ToListCodeName(this Type enumType)
        {
            Array arrValues = Enum.GetValues(enumType);
            string[] arrNames = Enum.GetNames(enumType);

            List<KeyValuePair<string, string>> result = arrNames.Select((name, index) =>
                                                                new KeyValuePair<string, string>(
                                                                    arrValues.GetValue(index).GetHashCode().ToString(),
                                                                    name))
                                                        .ToList();

            return result;
        }

        /// <summary>
        /// Converts to file size format
        /// </summary>
        /// <param name="item">Long item</param>
        /// <returns>Returns the format size</returns>
        public static string ToFileSize(this long value)
        {
            return string.Format(new FileSizeFormatProvider(), "{0:fs}", value);
        }

        /// <summary>
        /// Raises an event
        /// </summary>
        /// <param name="eventHandler">Event handler</param>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event arguments</param>
        public static void Raise(this EventHandler eventHandler, object sender, EventArgs e)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        /// <summary>
        /// Raises an event
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="eventHandler">Event handler</param>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event arguments</param>
        public static void Raise<T>(this EventHandler<T> eventHandler, object sender, T e) where T : EventArgs
        {
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        /// <summary>
        /// In clause of SQL query
        /// </summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="values">List element</param>
        /// <returns>Returns true or false</returns>
        public static bool In<T>(this T source, params T[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return values.Contains(source);
        }

        /// <summary>
        /// Serializes an object of type T in to an xml string
        /// </summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="source">Object to serialize</param>
        /// <returns>A string that represents Xml, empty otherwise</returns>
        public static string XmlSerializer<T>(this T source) where T : class, new()
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            XmlSerializerNamespaces namespaceSerializer = new XmlSerializerNamespaces();
            namespaceSerializer.Add(string.Empty, string.Empty);

            XmlWriterSettings writerSettings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Indent = true,
                Encoding = new UTF8Encoding(false)
            };

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder, writerSettings))
            {
                serializer.Serialize(writer, source);

                writer.Flush();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Deserializes an xml string in to an object of Type T
        /// </summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="xml">Xml as string to deserialize from</param>
        /// <returns>A new object of type T is successful, null if failed</returns>
        public static T XmlDeserialize<T>(this string xml) where T : class, new()
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            T result = default(T);

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xml))
            {
                try
                {
                    result = serializer.Deserialize(reader) as T;
                }
                catch
                {
                    // Could not be deserialized to this type
                }
            }
            
            return result;
        }

        /// <summary>
        /// Serializes an object of type T in to an Base64 string
        /// </summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="source">Object to serialize</param>
        /// <returns>A string that represents Bass64 string, empty otherwise</returns>
        public static string Base64Serializer<T>(this T source) where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            string result = string.Empty;

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, source);

                byte[] content = stream.GetBuffer();

                result = Convert.ToBase64String(content, 0, content.Length, Base64FormattingOptions.None);
            }
                        
            return result;
        }

        /// <summary>
        /// Deserializes an xml string in to an object of Type T
        /// </summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="source">Base64 as string to deserialize from</param>
        /// <returns>A new object of type T is successful, null if failed</returns>
        public static T Base64Deserialize<T>(this string source) where T : class
        {
            T result = default(T);

            byte[] content = Convert.FromBase64String(source);
            int length = content.Length;

            using (MemoryStream stream = new MemoryStream(content, 0, length))
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    result = binaryFormatter.Deserialize(stream) as T;
                }
                catch
                {
                    // Could not be deserialized to this type
                }
            }

            return result;
        }
    }
}
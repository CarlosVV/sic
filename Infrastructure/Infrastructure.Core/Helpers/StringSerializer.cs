namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    #endregion
    public static class StringSerializer
    {
        private const string separator_replace_string = "-@-";

        /// <summary>
        /// Serializes a flat object's properties into a String
        /// separated by a separator character/string. Only
        /// top level properties are serialized.
        /// </summary>
        /// <remarks>
        /// Only serializes top level properties, with no nesting support
        /// and only simple properties or those with a type converter are
        /// 'serialized'. All other property types use ToString().
        /// </remarks>
        /// <param name="objectToSerialize">The object to serialize</param>
        /// <param name="separator">Optional separator character or string. Default is |</param>
        /// <returns></returns>
        public static string SerializeObject(object objectToSerialize, string separator = null)
        {
            if (separator == null)
                separator = "|";

            if (objectToSerialize == null)
                return "null";

            var properties =
                objectToSerialize.GetType()
                                 .GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //.OrderBy(prop => prop.Name.ToLower())
            //.ToArray();

            var values = new List<string>();

            for (int i = 0; i < properties.Length; i++)
            {
                var pi = properties[i];

                // don't store read/write-only data
                if (!pi.CanRead && !pi.CanWrite)
                    continue;

                object value = pi.GetValue(objectToSerialize, null);

                string stringValue = "null";
                if (value != null)
                {
                    if (value is string)
                    {
                        stringValue = (string)value;
                        if (stringValue.Contains(separator))
                            stringValue = stringValue.Replace(separator, separator_replace_string);
                    }
                    else
                        stringValue = ReflectionHelper.TypedValueToString(value, unsupportedReturn: "null");
                }

                values.Add(stringValue);
            }

            if (values.Count < 0)
                // empty object (no properties)
                return string.Empty;

            return string.Join(separator, values.ToArray());
        }

        /// <summary>
        /// Deserializes an object previously serialized by SerializeObject.
        /// </summary>
        /// <param name="serialized"></param>
        /// <param name="type"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static object DeserializeObject(string serialized, Type type, string separator = null)
        {
            if (serialized == "null")
                return null;

            if (separator == null)
                separator = "|";

            object inst = ReflectionHelper.CreateInstanceFromType(type);
            var properties = inst.GetType()
                                 .GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //.OrderBy(prop => prop.Name.ToLower())
            //.ToArray();

            string[] tokens = serialized.Split(new string[] { separator }, StringSplitOptions.None);
            if (tokens == null || tokens.Length < 1)
                return null;

            for (int i = 0; i < properties.Length; i++)
            {
                string token = tokens[i];
                var prop = properties[i];

                // don't store read/write-only data
                if (!prop.CanRead && !prop.CanWrite)
                    continue;

                token = token.Replace(separator_replace_string, separator);

                object value = null;
                if (token != null)
                {
                    try
                    {
                        value = ReflectionHelper.StringToTypedValue(token, prop.PropertyType);
                    }
                    catch (InvalidCastException)
                    {
                        // skip over unsupported types
                    }
                }

                prop.SetValue(inst, value, null);
            }


            return inst;
        }

        /// <summary>
        /// Deserializes an object serialized with SerializeObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serialized"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string serialized, string separator = null)
            where T : class, new()
        {
            return DeserializeObject(serialized, typeof(T), separator) as T;
        }
    }
}
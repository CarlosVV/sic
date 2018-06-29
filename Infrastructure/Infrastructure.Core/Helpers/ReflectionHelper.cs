namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    #endregion

    /// <summary>
    /// Collection of Reflection and type conversion related utility functions
    /// </summary>
    public static class ReflectionHelper
    {
        public const BindingFlags MemberAccess =
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;

        /// <summary>
        /// Creates an instance from a type by calling the parameterless constructor.
        ///
        /// Note this will not work with COM objects - continue to use the Activator.CreateInstance
        /// for COM objects.
        /// <seealso>Class wwUtils</seealso>
        /// </summary>
        /// <param name="typeToCreate">
        /// The type from which to create an instance.
        /// </param>
        /// <returns>object</returns>
        public static object CreateInstanceFromType(Type typeToCreate, params object[] args)
        {
            if (args == null)
            {
                Type[] Parms = Type.EmptyTypes;
                return typeToCreate.GetConstructor(Parms).Invoke(null);
            }

            return Activator.CreateInstance(typeToCreate, args);
        }

        /// <summary>
        /// Turns a string into a typed value generically.
        /// Explicitly assigns common types and falls back
        /// on using type converters for unhandled types.         
        /// 
        /// Common uses: 
        /// * UI -&gt; to data conversions
        /// * Parsers
        /// <seealso>Class ReflectionUtils</seealso>
        /// </summary>
        /// <param name="sourceString">
        /// The string to convert from
        /// </param>
        /// <param name="targetType">
        /// The type to convert to
        /// </param>
        /// <param name="culture">
        /// Culture used for numeric and datetime values.
        /// </param>
        /// <returns>object. Throws exception if it cannot be converted.</returns>
        public static object StringToTypedValue(string sourceString, Type targetType, CultureInfo culture = null)
        {
            object result = null;

            bool isEmpty = string.IsNullOrEmpty(sourceString);

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (targetType == typeof(string))
                result = sourceString;
            else if (targetType == typeof(Int32) || targetType == typeof(int))
            {
                if (isEmpty)
                    result = 0;
                else
                    result = Int32.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(Int64))
            {
                if (isEmpty)
                    result = (Int64)0;
                else
                    result = Int64.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(Int16))
            {
                if (isEmpty)
                    result = (Int16)0;
                else
                    result = Int16.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(decimal))
            {
                if (isEmpty)
                    result = 0M;
                else
                    result = decimal.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(DateTime))
            {
                if (isEmpty)
                    result = DateTime.MinValue;
                else
                    result = Convert.ToDateTime(sourceString, culture.DateTimeFormat);
            }
            else if (targetType == typeof(byte))
            {
                if (isEmpty)
                    result = 0;
                else
                    result = Convert.ToByte(sourceString);
            }
            else if (targetType == typeof(double))
            {
                if (isEmpty)
                    result = 0F;
                else
                    result = Double.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(Single))
            {
                if (isEmpty)
                    result = 0F;
                else
                    result = Single.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(bool))
            {
                if (!isEmpty &&
                    sourceString.ToLower() == "true" || sourceString.ToLower() == "on" || sourceString == "1")
                    result = true;
                else
                    result = false;
            }
            else if (targetType == typeof(Guid))
            {
                if (isEmpty)
                    result = Guid.Empty;
                else
                    result = new Guid(sourceString);
            }
            else if (targetType.IsEnum)
                result = Enum.Parse(targetType, sourceString);
            else if (targetType == typeof(byte[]))
            {
                // TODO: Convert HexBinary string to byte array
                result = null;
            }

            // Handle nullables explicitly since type converter won't handle conversions
            // properly for things like decimal separators currency formats etc.
            // Grab underlying type and pass value to that
            else if (targetType.Name.StartsWith("Nullable`"))
            {
                if (sourceString.ToLower() == "null" || sourceString == string.Empty)
                    result = null;
                else
                {
                    targetType = Nullable.GetUnderlyingType(targetType);
                    result = StringToTypedValue(sourceString, targetType);
                }
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null && converter.CanConvertFrom(typeof(string)))
                    result = converter.ConvertFromString(null, culture, sourceString);
                else
                {
                    Debug.Assert(false, string.Format("Type Conversion not handled in StringToTypedValue for {0} {1}",
                                                        targetType.Name, sourceString));
                    throw (new InvalidCastException(targetType.Name));
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a type to string if possible. This method supports an optional culture generically on any value.
        /// It calls the ToString() method on common types and uses a type converter on all other objects
        /// if available
        /// </summary>
        /// <param name="rawValue">The Value or Object to convert to a string</param>
        /// <param name="culture">Culture for numeric and DateTime values</param>
        /// <param name="unsupportedReturn">Return string for unsupported types</param>
        /// <returns>string</returns>
        public static string TypedValueToString(object rawValue, CultureInfo culture = null, string unsupportedReturn = null)
        {
            if (rawValue == null)
                return string.Empty;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            Type valueType = rawValue.GetType();
            string returnValue = null;

            if (valueType == typeof(string))
                returnValue = rawValue as string;
            else if (valueType == typeof(int) || valueType == typeof(decimal) ||
                valueType == typeof(double) || valueType == typeof(float) || valueType == typeof(Single))
                returnValue = string.Format(culture.NumberFormat, "{0}", rawValue);
            else if (valueType == typeof(DateTime))
                returnValue = string.Format(culture.DateTimeFormat, "{0}", rawValue);
            else if (valueType == typeof(bool) || valueType == typeof(Byte) || valueType.IsEnum)
                returnValue = rawValue.ToString();
            else if (valueType == typeof(Guid?))
            {
                if (rawValue == null)
                    returnValue = string.Empty;
                else
                    return rawValue.ToString();
            }
            else
            {
                // Any type that supports a type converter
                TypeConverter converter = TypeDescriptor.GetConverter(valueType);
                if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
                    returnValue = converter.ConvertToString(null, culture, rawValue);
                else
                {
                    // Last resort - just call ToString() on unknown type
                    if (!string.IsNullOrEmpty(unsupportedReturn))
                        returnValue = unsupportedReturn;
                    else
                        returnValue = rawValue.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Retrieve a dynamic 'non-typelib' property
        /// </summary>
        /// <param name="instance">Object to make the call on</param>
        /// <param name="property">Property to retrieve</param>
        /// <returns></returns>
        public static object GetPropertyCom(object instance, string property)
        {
            return instance.GetType().InvokeMember(property, ReflectionHelper.MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null,
                                                instance, null);
        }

        /// <summary>
        /// Returns a property or field value using a base object and sub members including . syntax.
        /// For example, you can access: oCustomer.oData.Company with (this,"oCustomer.oData.Company")
        /// </summary>
        /// <param name="parent">Parent object to 'start' parsing from.</param>
        /// <param name="property">The property to retrieve. Example: 'oBus.oData.Company'</param>
        /// <returns></returns>
        public static object GetPropertyExCom(object parent, string property)
        {
            Type Type = parent.GetType();

            int lnAt = property.IndexOf(".");
            if (lnAt < 0)
            {
                if (property == "this" || property == "me")
                    return parent;

                // Get the member
                return parent.GetType().InvokeMember(property, ReflectionHelper.MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null,
                    parent, null);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            string Main = property.Substring(0, lnAt);
            string Subs = property.Substring(lnAt + 1);

            object Sub = parent.GetType().InvokeMember(Main, ReflectionHelper.MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null,
                parent, null);

            // Recurse further into the sub-properties (Subs)
            return ReflectionHelper.GetPropertyExCom(Sub, Subs);
        }

        /// <summary>
        /// Calls a method on an object dynamically. This version requires explicit
        /// specification of the parameter type signatures.
        /// </summary>
        /// <param name="instance">Instance of object to call method on</param>
        /// <param name="method">The method to call as a stringToTypedValue</param>
        /// <param name="parameterTypes">Specify each of the types for each parameter passed.
        /// You can also pass null, but you may get errors for ambiguous methods signatures
        /// when null parameters are passed</param>
        /// <param name="parms">any variable number of parameters.</param>
        /// <returns>object</returns>
        public static object CallMethod(object instance, string method, Type[] parameterTypes, params object[] parms)
        {
            if (parameterTypes == null && parms.Length > 0)
                // Call without explicit parameter types - might cause problems with overloads
                // occurs when null parameters were passed and we couldn't figure out the parm type
                return instance.GetType().GetMethod(method, ReflectionHelper.MemberAccess | BindingFlags.InvokeMethod).Invoke(instance, parms);
            else
                // Call with parameter types - works only if no null values were passed
                return instance.GetType().GetMethod(method, ReflectionHelper.MemberAccess | BindingFlags.InvokeMethod, null, parameterTypes, null).Invoke(instance, parms);
        }

        /// <summary>
        /// Calls a method on an object dynamically.
        ///
        /// This version doesn't require specific parameter signatures to be passed.
        /// Instead parameter types are inferred based on types passed. Note that if
        /// you pass a null parameter, type inferrance cannot occur and if overloads
        /// exist the call may fail. if so use the more detailed overload of this method.
        /// </summary>
        /// <param name="instance">Instance of object to call method on</param>
        /// <param name="method">The method to call as a stringToTypedValue</param>
        /// <param name="parameterTypes">Specify each of the types for each parameter passed.
        /// You can also pass null, but you may get errors for ambiguous methods signatures
        /// when null parameters are passed</param>
        /// <param name="parms">any variable number of parameters.</param>
        /// <returns>object</returns>
        public static object CallMethod(object instance, string method, params object[] parms)
        {
            // Pick up parameter types so we can match the method properly
            Type[] parameterTypes = null;
            if (parms != null)
            {
                parameterTypes = new Type[parms.Length];
                for (int x = 0; x < parms.Length; x++)
                {
                    // if we have null parameters we can't determine parameter types - exit
                    if (parms[x] == null)
                    {
                        parameterTypes = null;  // clear out - don't use types
                        break;
                    }
                    parameterTypes[x] = parms[x].GetType();
                }
            }
            return CallMethod(instance, method, parameterTypes, parms);
        }

        /// <summary>
        /// Parses Properties and Fields including Array and Collection references.
        /// Used internally for the 'Ex' Reflection methods.
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        private static object GetPropertyInternal(object Parent, string Property)
        {
            if (Property == "this" || Property == "me")
                return Parent;

            object result = null;
            string pureProperty = Property;
            string indexes = null;
            bool isArrayOrCollection = false;

            // Deal with Array Property
            if (Property.IndexOf("[") > -1)
            {
                pureProperty = Property.Substring(0, Property.IndexOf("["));
                indexes = Property.Substring(Property.IndexOf("["));
                isArrayOrCollection = true;
            }

            // Get the member
            MemberInfo member = Parent.GetType().GetMember(pureProperty, ReflectionHelper.MemberAccess)[0];
            if (member.MemberType == MemberTypes.Property)
                result = ((PropertyInfo)member).GetValue(Parent, null);
            else
                result = ((FieldInfo)member).GetValue(Parent);

            if (isArrayOrCollection)
            {
                indexes = indexes.Replace("[", string.Empty).Replace("]", string.Empty);

                if (result is Array)
                {
                    int Index = -1;
                    int.TryParse(indexes, out Index);
                    result = CallMethod(result, "GetValue", Index);
                }
                else if (result is ICollection)
                {
                    if (indexes.StartsWith("\""))
                    {
                        // String Index
                        indexes = indexes.Trim('\"');
                        result = CallMethod(result, "get_Item", indexes);
                    }
                    else
                    {
                        // assume numeric index
                        int index = -1;
                        int.TryParse(indexes, out index);
                        result = CallMethod(result, "get_Item", index);
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// Returns a property or field value using a base object and sub members including . syntax.
        /// For example, you can access: oCustomer.oData.Company with (this,"oCustomer.oData.Company")
        /// This method also supports indexers in the Property value such as:
        /// Customer.DataSet.Tables["Customers"].Rows[0]
        /// </summary>
        /// <param name="Parent">Parent object to 'start' parsing from. Typically this will be the Page.</param>
        /// <param name="Property">The property to retrieve. Example: 'Customer.Entity.Company'</param>
        /// <returns></returns>
        public static object GetPropertyEx(object Parent, string Property)
        {
            Type type = Parent.GetType();

            int at = Property.IndexOf(".");
            if (at < 0)
            {
                // Complex parse of the property
                return GetPropertyInternal(Parent, Property);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            string main = Property.Substring(0, at);
            string subs = Property.Substring(at + 1);

            // Retrieve the next . section of the property
            object sub = GetPropertyInternal(Parent, main);

            // Now go parse the left over sections
            return GetPropertyEx(sub, subs);
        }

        /// <summary>
        /// Copies the content of one object to another. The target object 'pulls' properties of the first.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyObjectData(object source, Object target)
        {
            CopyObjectData(source, target, MemberAccess);
        }

        /// <summary>
        /// Copies the content of one object to another. The target object 'pulls' properties of the first.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="memberAccess"></param>
        public static void CopyObjectData(object source, Object target, BindingFlags memberAccess)
        {
            CopyObjectData(source, target, null, memberAccess);
        }

        /// <summary>
        /// Copies the content of one object to another. The target object 'pulls' properties of the first.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="excludedProperties"></param>
        public static void CopyObjectData(object source, Object target, string excludedProperties)
        {
            CopyObjectData(source, target, excludedProperties, MemberAccess);
        }

        /// <summary>
        /// Copies the data of one object to another. The target object 'pulls' properties of the first.
        /// This any matching properties are written to the target.
        ///
        /// The object copy is a shallow copy only. Any nested types will be copied as
        /// whole values rather than individual property assignments (ie. via assignment)
        /// </summary>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The object to copy to</param>
        /// <param name="excludedProperties">A comma delimited list of properties that should not be copied</param>
        /// <param name="memberAccess">Reflection binding access</param>
        public static void CopyObjectData(object source, object target, string excludedProperties = null, BindingFlags memberAccess = MemberAccess)
        {
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            MemberInfo[] miT = target.GetType().GetMembers(memberAccess);
            foreach (MemberInfo Field in miT)
            {
                string name = Field.Name;

                // Skip over any property exceptions
                if (!string.IsNullOrEmpty(excludedProperties) &&
                    excluded.Contains(name))
                    continue;

                if (Field.MemberType == MemberTypes.Field)
                {
                    FieldInfo SourceField = source.GetType().GetField(name);
                    if (SourceField == null)
                        continue;

                    object SourceValue = SourceField.GetValue(source);
                    ((FieldInfo)Field).SetValue(target, SourceValue);
                }
                else if (Field.MemberType == MemberTypes.Property)
                {
                    PropertyInfo piTarget = Field as PropertyInfo;
                    PropertyInfo SourceField = source.GetType().GetProperty(name, memberAccess);
                    if (SourceField == null)
                        continue;

                    if (piTarget.CanWrite && SourceField.CanRead)
                    {
                        object SourceValue = SourceField.GetValue(source, null);
                        piTarget.SetValue(target, SourceValue, null);
                    }
                }
            }
        }

    }
}
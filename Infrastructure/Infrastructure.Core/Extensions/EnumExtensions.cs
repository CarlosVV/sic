namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.ComponentModel;
    using System.Linq;

    #endregion

    /// <summary>
    /// Enumeration extension methods.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the translation key for enumeration
        /// </summary>
        /// <param name="item">Enumeration item</param>
        /// <returns>Returns the translation key</returns>
        public static string GetTranslationKey(this Enum value)
        {
            return string.Format("Enum.{0}.{1}", value.GetType().Name, value.ToString());
        }       

        /// <summary>
        /// Gets the attribute type
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type</typeparam>
        /// <param name="item">Enumeration item</param>
        /// <returns>Returns the attribute type</returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            return type.GetField(name)
                    .GetCustomAttributes(false)
                    .OfType<TAttribute>()
                    .SingleOrDefault();
        }

        /// <summary>
        /// Gets the enumeration description
        /// </summary>
        /// <param name="value">Enumeration item</param>
        /// <returns>Returns a friendly description</returns>
        public static string Description(this Enum value)
        {
            var memberInfo = value.GetType().GetMember(value.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return value.ToString();
        }

        /// <summary>
        /// Determines whether the enumeration has description
        /// </summary>
        /// <param name="value">Enumeration item</param>
        /// <returns>Returns true or false</returns>
        public static bool HasDescription(this Enum value)
        {
            return !string.IsNullOrWhiteSpace(value.Description());
        }

        /// <summary>
        /// Determines whether the enumeration has description
        /// </summary>
        /// <param name="value">Enumeration item</param>
        /// <param name="expectedDescription">Expected description</param>
        /// <returns>Returns true or false</returns>
        public static bool HasDescription(this Enum value, string expectedDescription)
        {
            return value.Description().Equals(expectedDescription);
        }

        /// <summary>
        /// Determines whether the enumeration has description
        /// </summary>
        /// <param name="value">Enumeration item</param>
        /// <param name="expectedDescription">Expected description</param>
        /// <param name="comparisionType">String comparison type</param>
        /// <returns>Returns true or false</returns>
        public static bool HasDescription(this Enum value, string expectedDescription, StringComparison comparisionType)
        {
            return value.Description().Equals(expectedDescription, comparisionType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="matchTo"></param>
        /// <returns></returns>
        public static bool IsSet(this Enum input, Enum matchTo)
        {
            return (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;
        }
    }
}
namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.Script.Serialization;
    using System.Linq;

    #endregion

    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        public const string TimeFormat = "dd/MM/yyyy HH:mm";

        /// <summary>
        /// Parse from string to Enumeration
        /// </summary>
        /// <typeparam name="TEnum">Enumeration type</typeparam>
        /// <param name="input">String instance</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Enumeration type instance</returns>
        public static TEnum ParseAsEnum<TEnum>(this string input, bool ignoreCase = true) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), input, ignoreCase);
        }

        /// <summary>
        /// Try to parse from string to Enumeration
        /// </summary>
        /// <typeparam name="TEnum">Enumeration type</typeparam>
        /// <param name="input">Input string</param>
        /// <param name="defaultValue">Default item</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>the Enumeration type</returns>
        public static TEnum TryParseAsEnum<TEnum>(this string input, TEnum defaultValue, bool ignoreCase = true) where TEnum : struct
        {
            TEnum tryParse;

            int parsedInteger;
            if (int.TryParse(input, out parsedInteger) && !Enum.IsDefined(typeof(TEnum), parsedInteger))
            {
                return defaultValue;
            }

            if (Enum.TryParse<TEnum>(input, ignoreCase, out tryParse))
            {
                return tryParse;
            }

            return defaultValue;
        }

        /// <summary>
        /// Compresses the specified string a byte array using the specified encoding.
        /// </summary>
        /// <param name="stringToCompress">The string to compress.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>bytes array with compressed string</returns>
        public static byte[] Compress(this string stringToCompress, Encoding encoding)
        {
            byte[] stringAsBytes = encoding.GetBytes(stringToCompress);

            MemoryStream memoryStream = null;
            GZipStream zipStream = null;

            try
            {
                memoryStream = new MemoryStream();

                zipStream = new GZipStream(memoryStream, CompressionMode.Compress);

                memoryStream = null;

                zipStream.Write(stringAsBytes, 0, stringAsBytes.Length);

                return memoryStream.ToArray();
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }

                if (zipStream != null)
                {
                    zipStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Compresses the specified string a byte array using default
        /// UTF8 encoding.
        /// </summary>
        /// <param name="stringToCompress">The string to compress.</param>
        /// <returns>bytes array with compressed string</returns>
        public static byte[] Compress(this string stringToCompress)
        {
            return Compress(stringToCompress, new UTF8Encoding());
        }

        /// <summary>
        /// Takes a phrase and turns it into CamelCase text.
        /// White Space, punctuation and separators are stripped
        /// </summary>
        /// <param name="phrase">Phrase to format</param>
        /// <returns>Returns the formatted string</returns>
        public static string ToCamelCase(this string phrase)
        {
            if (phrase == null)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder(phrase.Length);

            // First letter is always upper case
            bool nextUpper = true;

            foreach (char ch in phrase)
            {
                if (char.IsWhiteSpace(ch) || char.IsPunctuation(ch) || char.IsSeparator(ch))
                {
                    nextUpper = true;
                    continue;
                }

                if (nextUpper)
                {
                    builder.Append(char.ToUpper(ch));
                }
                else
                {
                    builder.Append(char.ToLower(ch));
                }

                nextUpper = false;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Tries to create a phrase string from CamelCase text.
        /// Will place spaces before capitalized letters.
        /// Note that this method may not work for round tripping 
        /// ToCamelCase calls, since ToCamelCase strips more characters
        /// than just spaces.
        /// </summary>
        /// <param name="camelCase">Input string</param>
        /// <returns>Returns a formatted string as camel case</returns>
        public static string FromCamelCase(this string camelCase)
        {
            if (camelCase == null)
            {
                throw new ArgumentException("Null is not allowed");
            }

            StringBuilder builder = new StringBuilder(camelCase.Length + 10);
            bool first = true;
            char lastChar = '\0';

            foreach (char ch in camelCase)
            {
                if (!first &&
                     (char.IsUpper(ch) ||
                       char.IsDigit(ch) && !char.IsDigit(lastChar)))
                {
                    builder.Append(' ');
                }

                builder.Append(ch);
                first = false;
                lastChar = ch;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Appends a formatted string resulting from a String.Format call
        /// </summary>
        /// <param name="builder">String builder instance</param>
        /// <param name="format">A string containing formatting information, to be used in the same way as within the String.Format method (e.g. "{0}, {1}")</param>
        /// <param name="args">Parameter array of arguments to be inserted in the format string's placeholders</param>
        public static void AppendLineFormat(this StringBuilder builder, string format, params object[] args)
        {
            builder.AppendFormat(format, args);
            builder.AppendLine();
        }

        /// <summary>
        /// Appends string based on condition
        /// </summary>
        /// <param name="builder">String builder instance</param>
        /// <param name="condition">Condition sentence</param>
        /// <param name="value">A string</param>
        /// <returns>Returns the used string builder instance</returns>
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string value)
        {
            if (condition)
            {
                builder.Append(value);
            }

            return builder;
        }

        /// <summary>
        /// Indicates whether the item matches against a pattern
        /// </summary>
        /// <param name="input">The string to search a match</param>
        /// <param name="pattern">The regular expression pattern to match</param>
        /// <returns>Returns true or false</returns>
        public static bool Match(this string input, string pattern)
        {
            Regex regex = new Regex(pattern);

            return regex.IsMatch(input);
        }

        /// <summary>
        /// Enables quick and more natural string.Format calls
        /// </summary>
        /// <param name="input">A composite format string</param>
        /// <param name="args">The object to format</param>
        /// <returns>Returns a formatted string</returns>
        public static string FormatString(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

        /// <summary>
        /// Encodes the input string as HTML (converts special characters to entities)
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>HTML-encoded string</returns>
        public static string ToHTMLEncoded(this string input)
        {
            return HttpUtility.HtmlEncode(input);
        }

        /// <summary>
        /// Encodes the input string as a URL (converts special characters to % codes)
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>URL-encoded string</returns>
        public static string ToURLEncoded(this string input)
        {
            return HttpUtility.UrlEncode(input);
        }

        /// <summary>
        /// Encodes the path contained inside the input string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>URL path encoded string</returns>
        public static string ToURLPathEncode(this string input)
        {
            return HttpUtility.UrlPathEncode(input);
        }

        /// <summary>
        /// Decodes any HTML entities in the input string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>HTML encoded string</returns>
        public static string FromHTMLEncoded(this string input)
        {
            return HttpUtility.HtmlDecode(input);
        }

        /// <summary>
        /// Decodes any URL codes (% codes) in the input string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>URL-encoded string</returns>
        public static string FromURLEncoded(this string input)
        {
            return HttpUtility.UrlDecode(input);
        }

        /// <summary>
        /// Removes any HTML tags from the input string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>HTML string</returns>
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, @"<(style|script)[^<>]*>.*?</\1>|</?[a-z][a-z0-9]*[^<>]*>|<!--.*?-->", string.Empty);
        }

        /// <summary>
        /// Deserialzes an input string to object
        /// </summary>
        /// <typeparam name="T">Any class</typeparam>
        /// <param name="input">JSON string</param>
        /// <returns>Returns object type</returns>
        public static T FromJson<T>(this string input) where T : class
        {
            JavaScriptSerializer serialzer = new JavaScriptSerializer();

            return serialzer.Deserialize<T>(input);
        }

        /// <summary>
        /// "a string".IsNullOrEmpty() beats string.IsNullOrEmpty("a string")
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Returns true or false</returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Takes the first X characters
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="count">Count characters</param>
        /// <param name="ellipsis">Indicate whether must apply ellipis</param>
        /// <returns>Returns the taken string</returns>
        public static string Take(this string input, int count, bool ellipsis = false)
        {
            int lengthToTake = Math.Min(count, input.Length);
            string cutDownString = input.Substring(0, lengthToTake);

            if (ellipsis && lengthToTake < input.Length)
            {
                cutDownString += "...";
            }

            return cutDownString;
        }

        /// <summary>
        /// Removes white space, not line end.
        /// Useful when parsing user input such phone,
        /// price int.Parse("1 000 000".RemoveSpaces(),.....
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Returns string without spaces</returns>
        public static string RemoveSpaces(this string input)
        {
            return input.Replace(" ", "");
        }

        /// <summary>
        /// Checks if is valid email address
        /// </summary>
        /// <param name="input">Email address to test</param>
        /// <returns>true, if is valid email address</returns>
        public static bool IsValidEmailAddress(this string input)
        {
            return input.Match(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$");
        }

        /// <summary>
        /// Checks if url is valid.
        /// </summary>
        /// <remarks>
        /// ?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)? - user@
        /// (([0-9]{1,3}\.){3}[0-9]{1,3} - IP 199.194.52.184
        /// | - allows either IP or domain
        /// ([0-9a-z_!~*'()-]+\.)* - tertiary domain(s) - www.
        /// ([0-9a-z][0-9a-z-]{0,61})?[0-9a-z] - second level domain
        /// (\.[a-z]{2,6})?) - first level domain- .com or .museum is optional
        /// (:[0-9]{1,5})? - port number - :80
        /// ((/?)| - a slash isn't required if there is no file name
        /// </remarks>
        /// <param name="input">URL to test</param>
        /// <returns>Returns true, if is valid URL</returns>
        public static bool IsValidUrl(this string input)
        {   
            return input.Match(@"^(https?://)?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?(([0-9]{1,3}\.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+\.)*([0-9a-z_!~*'()-]+\.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z](\.[a-z]{2,6})?)(:[0-9]{1,5})?((/?)|(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$");
        }

        /// <summary>
        /// Checks if the string can be parsed as Double respective Int32.
        /// Spaces are not considered.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="floatPoint">true, if Double is considered, otherwhise Int32 is considered.</param>
        /// <returns>Returns true, if the string contains only digits or float-point</returns>
        public static bool IsNumber(this string input, bool floatPoint)
        {
            string withoutWhiteSpace = input.RemoveSpaces();

            if (floatPoint)
            {
                double resultDouble;

                return double.TryParse(withoutWhiteSpace, NumberStyles.Any, Thread.CurrentThread.CurrentUICulture, out resultDouble);
            }
            else
            {
                int resultInteger;

                return int.TryParse(withoutWhiteSpace, out resultInteger);
            }
        }

        /// <summary>
        /// Checks if the string only contains numeric values.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Returns true if is valid</returns>
        public static bool IsNumberOnly(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            input = input.Trim();

            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Converts string to integer if the item is valid, otherwise 0.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Returns integer item</returns>
        public static int ToInt(this string input)
        {
            return input.ToInt(0);
        }

        /// <summary>
        /// Converts string to integer if the item is valid, otherwise the default item.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="defaultValue">Default item</param>
        /// <returns>Returns integer item</returns>
        public static int ToInt(this string input, int defaultValue)
        {
            int number = defaultValue;
            if (!int.TryParse(input, out number))
            {
                number = defaultValue;
            }

            return number;
        }

        /// <summary>
        /// Converts string to decimal if the item is valid, otherwise 0.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Returns decimal item</returns>
        public static decimal ToDecimal(this string input)
        {
            return input.ToDecimal(decimal.Zero);
        }

        /// <summary>
        /// Converts string to decimal if the item is valid, otherwise the default item.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="defaultValue">Default item</param>
        /// <returns>Returns decimal item</returns>
        public static decimal ToDecimal(this string input, decimal defaultValue)
        {
            decimal number = defaultValue;
            if (!decimal.TryParse(input, out number))
            {
                number = defaultValue;
            }

            return number;
        }

        /// <summary>
        /// Converts string to boolean if the item is valid, otherwise the default item
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="defaultValue">Default item</param>
        /// <returns>Returns boolean</returns>
        public static bool ToBool(this string input, bool defaultValue = false)
        {
            bool result = defaultValue;

            input = string.Empty + input;
            if (input.Length > 1)
            {
                input = input.Substring(0, 1);
            }

            input = input.ToLowerInvariant();
            if (input == "1" || input == "t" || input == "y")
            {
                result = true;
            }
            else if (input == "0" || input == "f" || input == "n")
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Pluralize an integer item
        /// </summary>
        /// <param name="count">Input number</param>
        /// <param name="singular">Singular word</param>
        /// <param name="plural">Plural word</param>
        /// <returns>Returns a pluralized word</returns>
        public static string Pluralize(this int count, string singular, string plural)
        {
            return count == 1 ? singular : plural;
        }

        /// <summary>
        /// returns empty string if start-index is out of range
        /// returns string from start to given length, if start-index is smaller than 0
        /// returns string from start-index to end of string, if length argument is beyond number of characters left
        /// returns appropriate string if arguments in valid range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SafeSubstring(this string value, int startIndex, int length)
        {
            return new string((value ?? string.Empty).Skip(startIndex).Take(length).ToArray());
        }

        /// <summary>
        /// Parses exactly a string to date time object
        /// </summary>
        /// <param name="date">Date string as dd/MM/yyyy</param>
        /// <returns>Date representation</returns>
        public static DateTime ToDate(this string date)
        {
            return ToDate(date, "00:00", null);
        }

        /// <summary>
        /// Parses exactly a string to date time object
        /// </summary>
        /// <param name="date">Date string as dd/MM/yyyy</param>
        /// <param name="format">Input format</param>
        /// <returns>Date representation</returns>
        public static DateTime ToDate(this string date, string format)
        {
            return ToDate(date, null, format);
        }

        /// <summary>
        /// Parses exactly a string to date time object
        /// </summary>
        /// <param name="date">Date string as dd/MM/yyyy</param>
        /// <param name="time">Time string as HH:MM</param>
        /// <param name="format">Input format</param>
        /// <returns>Date representation</returns>
        public static DateTime ToDate(this string date, string time = null, string format = null)
        {
            if (string.IsNullOrEmpty(date))
            {
                return DateTime.MinValue;
            }

            if (string.IsNullOrEmpty(format))
            {
                format = TimeFormat;
            }

            if (!string.IsNullOrEmpty(time))
            {
                date = string.Format("{0} {1}", date, time);
            }

            DateTime result = DateTime.MinValue;

            DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);

            return result;
        }

        public static string ToSSN(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            if (input.Length < 5) 
            {
                return string.Empty;
            }

            return input.Insert(5, "-").Insert(3, "-");
        }

        public static string ToEIN(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            if (input.Length < 2) 
            {
                return string.Empty;
            }

            return input.Insert(2, "-");
        }
    }
}
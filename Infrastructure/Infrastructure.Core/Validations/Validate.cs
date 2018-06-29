// -----------------------------------------------------------------------
// <copyright file="Validate.cs" company="Nagnoi">
// Copyright (c) Nagnoi. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Nagnoi.SiC.Infrastructure.Core.Validations
{
    #region Imports

    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion

    /// <summary>
    /// Validate class
    /// </summary>
    public static class Validate
    {
        #region Public Methods

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            bool result = false;

            if (string.IsNullOrEmpty(email))
            {
                return result;
            }

            email = email.Trim();

            result = Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$");

            return result;
        }

        /// <summary>
        /// Verifies that a string is in valid unique identifier
        /// </summary>
        /// <param name="input">GUID to verify</param>
        /// <returns>true if the string is a valid unique identifier and false if it's not</returns>
        public static bool IsValidGuid(string str)
        {
            Guid unique;

            bool result = false;

            if (string.IsNullOrEmpty(str))
            {
                return result;
            }

            str = str.Trim();

            result = Guid.TryParse(str, out unique);

            return result;
        }

        /// <summary>
        /// Verifies that a file is a valid image
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns>true if the string is a valid image extension and false if it's not</returns>
        public static bool IsValidImage(string fileName)
        {
            return IsValidImage(fileName, string.Empty);
        }

        /// <summary>
        /// Verifies that a string is in network path format
        /// </summary>
        /// <param name="filePath">File path to verify</param>
        /// <returns>True if the string is a valid network path and false if it's not</returns>
        public static bool IsValidFilePath(string filePath)
        {
            bool result = false;

            if (string.IsNullOrEmpty(filePath))
            {
                return result;
            }

            filePath = filePath.Trim();

            result = Regex.IsMatch(filePath, @"(?:(?:(?:\b[a-z]:|\\\\[a-z0-9_.$]+\\[a-z0-9_.$]+)\\|\\?[^\\/:*?""<>|\r\n]+\\?)(?:[^\\/:*?""<>|\r\n]+\\)*[^\\/:*?""<>|\r\n]*)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            return result;
        }

        /// <summary>
        /// Verifies that a file is a valid image
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="specificExtension">Specific Extension</param>
        /// <returns>true if the string is a valid image extension and false if it's not</returns>
        public static bool IsValidImage(string fileName, string specificExtension)
        {
            Regex regex;

            if (string.IsNullOrEmpty(specificExtension))
            {
                regex = new Regex(@"(.*?)\.(jpg|JPG|jpeg|JPEG|png|PNG|gif|GIF|bmp|BMP)$");
            }
            else
            {
                regex = new Regex(@"(.*?)\.(" + specificExtension.ToLowerInvariant() + "|" + specificExtension.ToUpperInvariant() + ")$");
            }

            return regex.IsMatch(fileName);
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <returns>Input string if its length is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length > maxLength)
            {
                return str.Substring(0, maxLength);
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "0";
            }

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (char.IsDigit(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();

            // Loop is faster than RegEx
            // return Regex.Replace(input, "\\D", "");
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="tryParse">Try to parse</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str, bool tryParse)
        {
            int number;

            bool isSucess = int.TryParse(str, out number);

            return isSucess ? str : string.Empty;
        }

        /// <summary>
        /// Ensures that a string is only integer
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>String parsed to integer, or 0 if not possible</returns>
        public static int EnsureInteger(string str)
        {
            int number;

            bool isSuccess = int.TryParse(str, out number);

            return isSuccess ? number : 0;
        }

        /// <summary>
        /// Ensures that a string is only decimal
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>String parsed to decimal, or 0 if not possible</returns>
        public static decimal EnsureDecimal(string str)
        {
            decimal number;

            bool isSuccess = decimal.TryParse(str, out number);

            return isSuccess ? number : decimal.Zero;
        }
        
        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>String parsed to not null</returns>
        public static string EnsureNotNull(string str)
        {
            return EnsureNotNull(str, false);
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="defaultValue">Default item or previous item</param>
        /// <returns>String parsed to not null</returns>
        public static string EnsureNotNull(string str, string defaultValue)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            return str;
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="useDefaultValue">Default item</param>
        /// <returns>String parsed to not null</returns>
        public static string EnsureNotNull(string str, bool useDefaultValue)
        {
            if (string.IsNullOrEmpty(str))
            {
                return useDefaultValue ? "Not Entered" : string.Empty;
            }

            return str;
        }

        /// <summary>
        /// Ensures that a date time is not null
        /// </summary>
        /// <param name="date">Input date time</param>
        /// <param name="defaultValue">Default item or previous item</param>
        /// <returns>DateTime parsed to not null</returns>
        public static DateTime EnsureNotNull(DateTime date, DateTime defaultValue)
        {
            if (date == DateTime.MinValue)
            {
                return defaultValue;
            }

            if (date == DateTime.MaxValue)
            {
                return defaultValue;
            }

            return date;
        }

        /// <summary>
        /// Ensures that a integer is not null
        /// </summary>
        /// <param name="input">Input integer</param>
        /// <param name="defaultValue">Default item or previous item</param>
        /// <returns>Integer parsed to not null</returns>
        public static int EnsureNotNull(int input, int defaultValue)
        {
            if (input == int.MinValue)
            {
                return defaultValue;
            }

            if (input == int.MaxValue)
            {
                return defaultValue;
            }

            if (input == 0)
            {
                return defaultValue;
            }

            return input;
        }

        /// <summary>
        /// Ensures that is a string is empty it is converted to null
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>String parsed to null</returns>
        public static string EmptyToNull(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Ensure that a string is not phone
        /// </summary>
        /// <param name="code">Country code</param>
        /// <param name="number">Phone number</param>
        /// <returns>String parsed to phone number format</returns>
        public static string EnsurePhoneFormat(string code, string number)
        {
            if (string.IsNullOrEmpty(code) &&
                string.IsNullOrEmpty(number))
            {
                return string.Empty;
            }

            return string.Concat("+", code, number);
        }

        /// <summary>
        /// Gets the phone code from entire phone
        /// </summary>
        /// <param name="entirePhone">Entire phone number</param>
        /// <returns>Returns the country code</returns>
        public static string GetPhoneCode(string entirePhone)
        {
            string resultPhoneCode;

            try
            {
                resultPhoneCode = entirePhone.Substring(1, 2);
            }
            catch
            {
                return string.Empty;
            }

            return resultPhoneCode;
        }

        /// <summary>
        /// Gets the phone number from entire phone
        /// </summary>
        /// <param name="entirePhone">Entire phone</param>
        /// <returns>Returns the phone number without country code</returns>
        public static string GetPhoneNumber(string entirePhone)
        {
            string resultPhoneNumber;

            try
            {
                resultPhoneNumber = entirePhone.Substring(3, entirePhone.Length - 3);
            }
            catch
            {
                return entirePhone;
            }

            return resultPhoneNumber;
        }

        #endregion
    }
}
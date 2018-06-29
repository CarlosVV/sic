namespace Nagnoi.SiC.Infrastructure.Core.IO
{
    #region Imports

    using System;

    #endregion

    /// <summary>
    /// Represents a file size format provider
    /// </summary>
    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        #region Constants

        /// <summary>
        /// File size format
        /// </summary>
        private const string FileSizeFormat = "fs";

        /// <summary>
        /// KB item
        /// </summary>
        private const decimal OneKiloByte = 1024M;

        /// <summary>
        /// MB item
        /// </summary>
        private const decimal OneMegaByte = OneKiloByte * 1024M;

        /// <summary>
        /// GB item
        /// </summary>
        private const decimal OneGigaByte = OneMegaByte * 1024M;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the format provider
        /// </summary>
        /// <param name="formatType">Format type</param>
        /// <returns>Return the custom formatter</returns>
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Returns the format
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="arg">Argument format</param>
        /// <param name="formatProvider">Format provider instance</param>
        /// <returns>Returns a formatted string</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null || !format.StartsWith(FileSizeFormat))
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            if (arg is string)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            decimal size;

            try
            {
                size = Convert.ToDecimal(arg);
            }
            catch (InvalidCastException)
            {
                return DefaultFormat(format, arg, formatProvider);
            }

            string suffix;
            if (size > OneGigaByte)
            {
                size /= OneGigaByte;
                suffix = " GB";
            }
            else if (size > OneMegaByte)
            {
                size /= OneMegaByte;
                suffix = " MB";
            }
            else if (size > OneKiloByte)
            {
                size /= OneKiloByte;
                suffix = " KB";
            }
            else
            {
                suffix = " bytes";
            }

            string precision = format.Substring(2);

            if (string.IsNullOrEmpty(precision))
            {
                precision = "2";
            }

            return string.Format("{0:N" + precision + "}{1}", size, suffix);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the default format
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="arg">Argument format</param>
        /// <param name="formatProvider">Format provider</param>
        /// <returns>Returns a formatted string</returns>
        private static string DefaultFormat(string format, object arg, IFormatProvider formatProvider)
        {
            IFormattable formattableArg = arg as IFormattable;
            if (formattableArg != null)
            {
                return formattableArg.ToString(format, formatProvider);
            }

            return arg.ToString();
        }

        #endregion
    }
}
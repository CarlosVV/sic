namespace Nagnoi.SiC.Infrastructure.Core.Helpers
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Globalization;

    #endregion

    /// <summary>
    /// Date Time extension methods.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns true if the date is between or equal to one of the two values.
        /// </summary>
        /// <param name="date">DateTime Base, from where the calculation will be preformed.</param>
        /// <param name="startDate">Start date to check for</param>
        /// <param name="endDate">End date to check for</param>
        /// <returns>boolean item indicating if the date is between or equal to one of the two values</returns>
        public static bool Between(this DateTime date, DateTime startDate, DateTime endDate)
        {
            var ticks = date.Ticks;
            return ticks >= startDate.Ticks && ticks <= endDate.Ticks;
        }

        /// <summary>
        /// Returns 12:59:59pm time for the date passed.
        /// Useful for date only search ranges end item
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <returns>Returns the end of day</returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Returns 12:00am time for the date passed.
        /// Useful for date only search ranges start item
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <returns>Returns the beginning of day</returns>
        public static DateTime BeginningOfDay(this DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Returns the very end of the given month (the last millisecond of the last hour for the given date)
        /// </summary>
        /// <param name="date">DateTime Base, from where the calculation will be preformed.</param>
        /// <returns>Returns the end of month</returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the Start of the given month (the fist millisecond of the given date)
        /// </summary>
        /// <param name="date">DateTime Base, from where the calculation will be preformed.</param>
        /// <returns>Returns the beginning of month</returns>
        public static DateTime BeginningOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// Returns all dates between a range dates
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Returns a list of dates</returns>
        public static IEnumerable<DateTime> Range(this DateTime startDate, DateTime endDate)
        {
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                yield return date;
            }
        }

        public static string ToFriendlyDateString(this DateTime date)
        {
            return date.ToString("dd MMM yyyy");
        }

        /// <summary>
        /// Format nullable date
        /// </summary>
        /// <param name="date">DateTime instance or null</param>
        /// <returns>Returns formatted date</returns>
        public static string ToString(this DateTime? date)
        {
            return date.ToString(null, DateTimeFormatInfo.CurrentInfo, string.Empty);
        }

        /// <summary>
        /// Format nullable date
        /// </summary>
        /// <param name="date">DateTime instance or null</param>
        /// <param name="format">DateTime format string</param>
        /// <returns>Returns formatted date</returns>
        public static string ToString(this DateTime? date, string format)
        {
            return date.ToString(format, DateTimeFormatInfo.CurrentInfo, string.Empty);
        }

        /// <summary>
        /// Format nullable date
        /// </summary>
        /// <param name="date">DateTime instance or null</param>
        /// <param name="provider">Format provider</param>
        /// <returns>Returns formatted date</returns>
        public static string ToString(this DateTime? date, IFormatProvider provider)
        {
            return date.ToString(null, provider, string.Empty);
        }

        /// <summary>
        /// Format nullable date
        /// </summary>
        /// <param name="date">DateTime instance or null</param>
        /// <param name="format">DateTime format string</param>
        /// <param name="provider">Format provider</param>
        /// <returns>Returns formatted date</returns>
        public static string ToString(this DateTime? date, string format, IFormatProvider provider, string returnIfNull)
        {
            if (date.HasValue)
            {
                return date.Value.ToString(format, provider);
            }
            else
            {
                return returnIfNull;
            }
        }

        /// <summary>
        /// Converts the value of the current System.DateTime object to its equivalent
        /// short date string representation.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>A string that contains the short date string representation of the current date time</returns>
        public static string ToShortDateString(this DateTime? input)
        {
            return input.ToShortDateString(string.Empty);
        }

        /// <summary>
        /// Converts the value of the current System.DateTime object to its equivalent
        /// short date string representation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="returnIfNull"></param>
        /// <returns>A string that contains the short date string representation of the current date time</returns>
        public static string ToShortDateString(this DateTime? input, string returnIfNull)
        {
            if (input.HasValue)
            {
                return input.Value.ToShortDateString();
            }

            return returnIfNull;
        }

        /// <summary>
        /// Return Number of Months
        /// </summary>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public static int PayableMonthsInDuration(DateTime StartDate, DateTime EndDate) {
            int sy = StartDate.Year; int sm = StartDate.Month; int count = 0;
            do {
                count++; if ((sy == EndDate.Year) && (sm >= EndDate.Month)) { break; }
                sm++; if (sm == 13) { sm = 1; sy++; }
            } while ((EndDate.Year >= sy) || (EndDate.Month >= sm));
            return (count);
        }
    }
}
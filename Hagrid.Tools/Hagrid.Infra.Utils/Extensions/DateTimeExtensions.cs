using System;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// Class datetime extensions
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Transform the datetime in string on format dd/MM/yyyy
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <returns>The datetime formated</returns>
        public static string ToInverseSimpleDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Transform the datetime in string on format dd/MM/yyyy
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <returns>The datetime formated</returns>
        public static string ToSimpleDate(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Transform the datetime in string on format dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <returns>The datetime formated</returns>
        public static string ToSingleDateTime(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Transform the datetime in string on format dd/MM/yyyy HH:mm:ss
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <returns>The datetime formated</returns>
        public static string ToFullDateTime(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Transform the datetime in string on format yyyy-MM-dd{T}HH:mm:ss T is optional
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <param name="includeT">The optional to include T on time</param>
        /// <returns>The datetime formated</returns>
        public static string ToInverseFullDateTime(this DateTime date, bool includeT = false)
        {
            return date.ToString(string.Format("yyyy-MM-dd{0}HH:mm:ss", includeT ? "T" : " "));
        }

        /// <summary>
        /// Transform the datetime in string on format yyyyMMddhhmmss
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <returns>The datetime formated</returns>
        public static string ToInverseFullDateTime(this DateTime date)
        {
            return date.ToString("yyyyMMddhhmmss");
        }

        /// <summary>
        /// Transform the datetime in string on format yyyyMMdd
        /// </summary>
        /// <param name="date">The datetime</param>
        /// <returns>The datetime formated</returns>
        public static string ToInverseSingleDateTime(this DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        /// <summary>
        /// To init date time set time to 00:00:00
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToInitDateTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// To end date time set time to 23:59:59
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToEndDateTime(this DateTime date)
        {
            var dataAux = date.AddDays(1);

            return new DateTime(dataAux.Year, dataAux.Month, dataAux.Day).AddSeconds(-1);
        }
    }
}
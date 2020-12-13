using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// Class enum extensions
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Transform the value string into Enum data type.
        /// </summary>
        /// <typeparam name="T">Enum data type</typeparam>
        /// <param name="value">The value in string to convert to type enum.</param>
        /// <param name="ignoreCase">To ignore case to convert enum.</param>
        /// <returns>The T value converted.</returns>
        public static T ToEnum<T>(this string value, bool? ignoreCase = null)
        {
            if (value.IsNullorEmpty()) return default(T);

            if (!ignoreCase.HasValue)
                return (T)Enum.Parse(typeof(T), value);

            return (T)Enum.Parse(typeof(T), value, ignoreCase.Value);
        }

        /// <summary>
        /// Transform the value string into Enum data type.
        /// </summary>
        /// <typeparam name="T">Enum data type</typeparam>
        /// <param name="value">The value in string to convert to type enum.</param>
        /// <param name="ignoreCase">To ignore case to convert enum.</param>
        /// <returns>The T value converted.</returns>
        public static T ToEnum<T>(this object value, bool? ignoreCase = null)
        {
            if (value.IsNull()) return default(T);

            return value.ToString().ToEnum<T>();
        }

        /// <summary>
        /// Transform the value string into enum data type to Int.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value in string to convert to type enum.</param>
        /// <returns>The int value converted.</returns>
        public static int ToValue<T>(this string value)
        {
            return Convert.ToInt32(Enum.Parse(typeof(T), value));
        }

        /// <summary>
        /// Get description attribute from enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The description attribute from enum</returns>
        public static string GetDescription(this Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Cast in string end put in lower case
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The lowercase from Lowerenum</returns>
        public static string ToLower(this Enum value)
        {
            return value.AsString().ToLower();
        }

        /// <summary>
        /// Gets a list of values for the enum type.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <returns>A list with all enum values</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            if (typeof(T).IsEnum)
                return Enum.GetValues(typeof(T)).Cast<T>();

            return default(IEnumerable<T>);
        }
    }
}
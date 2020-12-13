using System;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// Class number extensions
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Check int value is zero.
        /// </summary>
        /// <param name="value">The int value to be checked.</param>
        /// <returns>The boolean true or false.</returns>
        public static bool IsZero(this int value)
        {
            return value == default(int);
        }

        /// <summary>
        /// Check decimal value is zero.
        /// </summary>
        /// <param name="value">The decimal value to be checked.</param>
        /// <returns>The boolean true or false.</returns>
        public static bool IsZero(this decimal value)
        {
            return value == default(decimal);
        }

        /// <summary>
        /// Check float value is zero.
        /// </summary>
        /// <param name="value">The float value to be checked.</param>
        /// <returns>The boolean true or false.</returns>
        public static bool IsZero(this float value)
        {
            return value == default(float);
        }

        /// <summary>
        /// Check long value is zero.
        /// </summary>
        /// <param name="value">The long value to be checked.</param>
        /// <returns>The boolean true or false.</returns>
        public static bool IsZero(this long value)
        {
            return value == default(long);
        }

        /// <summary>
        /// Transform value into integer value Convert.ToInt32((value * 100)).
        /// </summary>
        /// <param name="value">The value to transform</param>
        /// <returns>The value to transformed</returns>
        public static int ToInteger(this decimal value)
        {
            if (value.IsZero()) return Convert.ToInt32(value);

            return Convert.ToInt32((value * 100));
        }

        /// <summary>
        /// Transform value into percent value (value / 100).
        /// </summary>
        /// <param name="value">The value to transform</param>
        /// <returns>The value to transformed</returns>
        public static decimal ToPercent(this decimal value)
        {
            return (value / 100);
        }

        /// <summary>
        /// Format item in a string to currency specified.
        /// </summary>
        /// <param name="value">The value to format</param>
        /// <param name="cultureName">An string culture-specific formatting information. (pt-BR, en-US)</param>
        /// <param name="format">A composite format string</param>
        /// <returns>The value formated</returns>
        public static string ToCurrency(this decimal value, string cultureName = "pt-BR", string format = "{0:C2}")
        {
            var numberFormat = System.Globalization.CultureInfo.GetCultureInfo(cultureName).NumberFormat;

            return string.Format(numberFormat, format, value);
        }

        /// <summary>
        /// Format item in a string to currency specified no sign.
        /// </summary>
        /// <param name="value">The value to format</param>        
        /// <returns>The value formated</returns>
        public static string ToCurrencyNoSign(this decimal value)
        {
            return value.ToCurrency("pt-BR", "{0:N2}");
        }

        /// <summary>
        /// Format item in a string to american currency specified.
        /// </summary>
        /// <param name="value">The value to format</param>        
        /// <returns>The value formated</returns>
        public static string ToUSCurrency(this decimal value)
        {
            return value.ToCurrency("en-US", "{0:N2}").TakeSpecialCharactersOff();
        }

        /// <summary>
        /// Divide decimal number into parts without losing decimal values
        /// </summary>
        /// <param name="value">The value to split</param>
        /// <param name="currentPart">Current part to be divided</param>
        /// <param name="qtyParts">Quantity of parts to be divided</param>
        /// <returns>Current part value</returns>
        public static decimal ToDivideIntoParts(this decimal value, int currentPart, int qtyParts)
        {
            //"acquireType" not used more because Cielo and Elavon are using same base Calc.

            Decimal valueInstallment = Decimal.Zero;
            Decimal difference = Decimal.Zero;
            Decimal inverseValue = Decimal.Zero;

            valueInstallment = (value / qtyParts).IgnoreRepeatingDecimals();

            if (qtyParts > 1 && currentPart == 1)
            {
                difference = Decimal.Zero;
                inverseValue = Decimal.Zero;

                valueInstallment = (value / qtyParts).IgnoreRepeatingDecimals();

                inverseValue = valueInstallment * qtyParts;

                difference = value - inverseValue;

                if (difference > 0)
                    valueInstallment += difference; //plus difference
            }

            return valueInstallment;
        }

        /// <summary>
        /// Ignore repeating decimals
        /// </summary>
        /// <param name="value">The value to ignore </param>
        /// <returns>Value with 2 decimals places/returns>
        public static decimal IgnoreRepeatingDecimals(this decimal value)
        {
            return Math.Truncate(value * 100) / 100;
        }
    }
}
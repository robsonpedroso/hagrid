using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// Class string extensions
    /// </summary>
    public static class StringExtensions
    {
        #region "  Validation  "

        /// <summary>
        /// Check CPF is valid
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public static bool IsValidCPF(this string CPF)
        {
            try
            {
                CPF = CPF.ClearStrings();

                if (CPF.IsNullOrWhiteSpace() || CPF.Length != 11)
                    return false;

                for (int i = 0; i < CPF.Length; i++)
                    Convert.ToInt32(CPF[i].ToString());

                if (CPF == "00000000000" || CPF == "11111111111" || CPF == "22222222222" || CPF == "33333333333" || CPF == "44444444444" || CPF == "55555555555" || CPF == "66666666666" || CPF == "77777777777" || CPF == "88888888888" || CPF == "99999999999")
                    return false;

                int[] a = new int[11];
                int b = 0;
                int c = 10;
                int x = 0;

                for (int i = 0; i < 9; i++)
                {
                    a[i] = Convert.ToInt32(CPF[i].ToString());

                    b += (a[i] * c);
                    c--;
                }


                x = b % 11;

                if (x < 2)
                    a[9] = 0;
                else
                    a[9] = 11 - x;

                b = 0;
                c = 11;

                for (int i = 0; i < 10; i++)
                {
                    b += (a[i] * c);
                    c--;
                }

                x = b % 11;

                if (x < 2)
                    a[10] = 0;
                else
                    a[10] = 11 - x;

                if ((Convert.ToInt32(CPF[9].ToString()) != a[9]) || (Convert.ToInt32(CPF[10].ToString()) != a[10]))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check CNPJ is valid
        /// </summary>
        /// <param name="CNPJ"></param>
        /// <returns></returns>
        public static bool IsValidCNPJ(this string CNPJ)
        {
            try
            {
                CNPJ = CNPJ.ClearStrings();

                if (CNPJ.IsNullOrWhiteSpace() || CNPJ.Length != 14)
                    return false;

                for (int i = 0; i < 14; i++)
                    Convert.ToInt32(CNPJ[i].ToString());

                int[] a = new int[14];
                int b = 0;
                int[] c = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int x = 0;

                for (int i = 0; i < 12; i++)
                {
                    a[i] = Convert.ToInt32(CNPJ[i].ToString());
                    b += a[i] * c[i + 1];
                }

                x = b % 11;

                if (x < 2)
                    a[12] = 0;
                else
                    a[12] = 11 - x;


                b = 0;
                for (int j = 0; j < 13; j++)
                    b += (a[j] * c[j]);

                x = b % 11;

                if (x < 2)
                    a[13] = 0;
                else
                    a[13] = 11 - x;

                if ((Convert.ToInt32(CNPJ[12].ToString()) != a[12]) || (Convert.ToInt32(CNPJ[13].ToString()) != a[13]))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check zipcode is valid
        /// </summary>
        /// <param name="cep"></param>
        /// <param name="allowDash"></param>
        /// <returns></returns>
        public static bool IsValidCEP(this string cep, bool allowDash = true)
        {
            Match match;

            if (allowDash)
                match = Regex.Match(cep, "^[0-9]{5}-[0-9]{3}$");
            else
                match = Regex.Match(cep.ClearStrings(), "^[0-9]{8}$");

            return match.Success;
        }

        /// <summary>
        /// Check e-mail is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            if (email.IsNullOrWhiteSpace()) return false;

            Match match = Regex.Match(email, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$", RegexOptions.IgnoreCase);

            return match.Success;
        }

        /// <summary>
        /// Check string is null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullorEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Check string is null or empty or white space
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Check is numeric
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value)
        {
            Match match = Regex.Match(value, "^[0-9]*$");

            return match.Success;
        }

        /// <summary>
        /// Check is json request
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsJSONRequest(this string contentType)
        {
            return contentType.Contains("application/json");
        }

        /// <summary>
        /// Check string is json request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="uriScheme"></param>
        /// <returns></returns>
        public static bool IsURL(this string url, string uriScheme = null)
        {
            Uri uriResult = null;

            if (uriScheme.IsNull())
                return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uriResult);

            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) && uriResult.Scheme == uriScheme;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUnicode(this string input)
        {
            const int MaxAnsiCode = 255;

            return input.Any(c => c > MaxAnsiCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasUnicodeChar(this string input)
        {
            const int MaxAnsiCode = 255;
            return input.Any(c => c > MaxAnsiCode);
        }

        #endregion

        #region "  Strings Ajustment  "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ClearStrings(this string value)
        {
            if (value.IsNullorEmpty()) return value;

            return value.Replace(".", string.Empty)
                        .Replace(",", string.Empty)
                        .Replace(":", string.Empty)
                        .Replace("/", string.Empty)
                        .Replace(@"\", string.Empty)
                        .Replace("\"", string.Empty)
                        .Replace("'", string.Empty)
                        .Replace("-", string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TakeAccentsOff(this string value)
        {
            if (value.IsNullorEmpty()) return value;

            value = Regex.Replace(value, "[áàâãª]", "a");
            value = Regex.Replace(value, "[ÁÀÂÃ]", "A");
            value = Regex.Replace(value, "[éèê]", "e");
            value = Regex.Replace(value, "[ÉÈÊ]", "e");
            value = Regex.Replace(value, "[íìî]", "i");
            value = Regex.Replace(value, "[ÍÌÎ]", "I");
            value = Regex.Replace(value, "[óòôõº]", "o");
            value = Regex.Replace(value, "[ÓÒÔÕ]", "O");
            value = Regex.Replace(value, "[úùû]", "u");
            value = Regex.Replace(value, "[ÚÙÛ]", "U");
            value = Regex.Replace(value, "[ç]", "c");
            value = Regex.Replace(value, "[Ç]", "C");

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TakeHyphenOff(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Replace("-", " ");

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TakeQuotationMarksOff(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Replace("\"", string.Empty);

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TakeUnderlineOff(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Replace("_", " ");

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="keepWhiteSpaces"></param>
        /// <param name="keepBar"></param>
        /// <returns></returns>
        public static string TakeSpecialCharactersOff(this string value, bool keepWhiteSpaces = false, bool keepBar = false)
        {
            if (value.IsNullOrWhiteSpace())
                return string.Empty;

            var sb = new StringBuilder();

            foreach (char c in value)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') | c == '.' || c == '_' || c == '-' || (c == ' ' && keepWhiteSpaces) || (c == '/' && keepBar) || (c == '\\' && keepBar))
                    sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TakeHTMLOff(this string value)
        {
            return Regex.Replace(value, @"(\<[^\>]+\>)", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TakeWhiteSpacesOff(this string value)
        {
            return value.Replace(" ", string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncodeURIComponent(this string value)
        {
            return System.Net.WebUtility.UrlEncode(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string URLEncode(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string URLDecode(this string url)
        {
            return HttpUtility.UrlDecode(url).Replace(" ", "+");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="inputWWW"></param>
        /// <returns></returns>
        public static string InputHTTPonURL(this string URL, bool inputWWW = false)
        {
            if (URL.IsNullorEmpty()) return URL;

            if (inputWWW)
                return String.Format("http://{0}", Regex.Replace(URL, "http://", string.Empty, RegexOptions.IgnoreCase).InputWWWonURL());
            else
                return String.Format("http://{0}", Regex.Replace(URL, "http://", string.Empty, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static string InputWWWonURL(this string URL)
        {
            if (URL.IsNullorEmpty()) return URL;

            return String.Format("www.{0}", Regex.Replace(URL, "www.", string.Empty, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static string RemoveHTTPonURL(this string URL)
        {
            if (URL.IsNullorEmpty()) return URL;

            return URL.Replace("http://", string.Empty).Replace("https://", string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static string RemoveWWWonURL(this string URL)
        {
            if (URL.IsNullorEmpty()) return URL;

            return URL.Replace("www.", string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveHyphen(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Replace("-", string.Empty).Replace("  ", " ");

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveHeaderXML(this string value)
        {
            Regex rgxXML = new Regex("(<?xml|<?XML)");
            Regex rgxVersion = new Regex("(version=\"1.0\")");
            Regex rgxEncoding = new Regex("(encoding=\"iso-8859-1\"|encoding=\"ISO-8859-1\"|encoding=\"utf-8\"|encoding=\"UTF-8\"|encoding=\"UTF-16\"|encoding=\"utf-16\")");

            if (rgxXML.IsMatch(value))
                value = rgxXML.Replace(value, string.Empty);

            if (rgxVersion.IsMatch(value))
                value = rgxVersion.Replace(value, string.Empty);

            if (rgxEncoding.IsMatch(value))
                value = rgxEncoding.Replace(value, string.Empty);

            return value.Replace("<?", string.Empty).Replace("?>", string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveQueryStrings(this string value)
        {
            if (value == null) return value;

            bool hasQueryStrings = value.Contains("?");

            if (hasQueryStrings)
                return new Uri(value).GetLeftPart(UriPartial.Path);
            else
                return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (value.IsNullOrWhiteSpace())
                return string.Empty;

            int length = value.Length > maxLength ? maxLength : value.Length;
            return value.Substring(0, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MaskCreditCard4Digits(this string value)
        {
            if (value.Length < 10)
                return "---";
            else
                return string.Format("**** **** **** {0}", value.Substring(0, 6), value.Substring(value.Length - 4), 4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CNPJ"></param>
        /// <returns></returns>
        public static string MaskCNPJ(this string CNPJ)
        {
            if (CNPJ.IsNullorEmpty() || CNPJ.Length < 14) return CNPJ;

            return string.Format("{0}.{1}.{2}/{3}-{4}",
                CNPJ.Substring(0, 2), CNPJ.Substring(2, 3), CNPJ.Substring(5, 3), CNPJ.Substring(8, 4), CNPJ.Substring(12, 2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public static string MaskCPF(this string CPF)
        {
            if (CPF.IsNullorEmpty() || CPF.Length < 11) return CPF;

            return string.Format("{0}.{1}.{2}-{3}",
                CPF.Substring(0, 3), CPF.Substring(3, 3), CPF.Substring(6, 3), CPF.Substring(9, 2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CEP"></param>
        /// <returns></returns>
        public static string MaskCEP(this string CEP)
        {
            if (CEP.IsNullorEmpty() || CEP.Length < 8) return CEP;

            return string.Format("{0}-{1}",
                CEP.Substring(0, 5), CEP.Substring(5));
        }

        /// <summary>
        /// OnlyNumbers
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int OnlyNumbers(this string value)
        {
            return Regex.Match(value, @"\d+").Value.AsInt();
        }

        /// <summary>
        /// OnlyAlphanumeric
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string OnlyAlphanumeric(this string value)
        {
            return Regex.Replace(value, @"[^A-Za-z0-9 ]", string.Empty).Trim();
        }

        /// <summary>
        /// Replaces the format item in a specified string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="value">A composite format string (see Remarks).</param>
        /// <param name="values">An object array that contains zero or more objects to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.
        /// </returns>
        public static string ToFormat(this string value, params object[] values)
        {
            return string.Format(value, values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string IfNullOrWhiteSpace(this string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        /// <summary>
        /// Clear json strings
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimJson(this string value)
        {
            if (value.IsNullorEmpty()) return value;

            return Regex.Replace(value, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
        }

        /// <summary>
        /// OnlyNumbers
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToNumber(this string value)
        {
            var num = string.Empty;

            if (!value.IsNullOrWhiteSpace())
            {
                foreach (Match match in Regex.Matches(value, @"\d+"))
                    num += match.Value;
            }   
            
            return num;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncodeToUTF8(this string value)
        {
            byte[] bytes = Encoding.Default.GetBytes(value);
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion

        #region "  Compress  "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Zip(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    msi.CopyTo(mso);

                return mso.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Unzip(this byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    msi.CopyTo(mso);

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Unzip(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            return Unzip(bytes);
        }

        #endregion
    }
}
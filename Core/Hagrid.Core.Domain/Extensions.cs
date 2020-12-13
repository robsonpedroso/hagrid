using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Hagrid.Core.Domain
{
    public static class Extensions
    {
        #region "  TypedKey  "
        public static string MakeTypedKey(this string key, TypeCode type)
        {
            return string.Format("{0}${1}", (int)type, key);
        }

        public static string CleanTypedKey(this string key)
        {
            var parts = key.Split('$');

            if (parts.Length == 2)
                return parts[1];

            return key;
        }

        public static TypeCode TypeCodeFromKey(this string key)
        {
            var parts = key.Split('$');

            if (parts.Length == 2)
            {
                int code = 0;
                if (int.TryParse(parts[0], out code) && code > 0 && code <= 18)
                    return (TypeCode)code;
            }

            return TypeCode.Empty;
        }

        public static object ConvertFromTypedKey(this KeyValuePair<string, string> pair)
        {
            var code = pair.Key.TypeCodeFromKey();

            switch (code)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                case TypeCode.String:
                case TypeCode.Object:
                    return pair.Value;
                default:
                    return Convert.ChangeType(pair.Value, code);
            }
        }
        #endregion

        public static T GetValue<T>(this JObject obj, string name)
        {
            JToken prop = null;
            if (obj.TryGetValue(name, out prop))
                return prop.ToObject<T>();

            return default(T);
        }

        public static T Deserialize<T>(this string serialized)
        {
            var deserialized = Deserialize(serialized, typeof(T));

            if (deserialized != null)
                return (T)deserialized;

            return default(T);
        }

        private static object Deserialize(string serialized, Type typeToSerializer)
        {
            object finalObject = null;

            if (!string.IsNullOrWhiteSpace(serialized))
            {
                try
                {
                    var serializer = new XmlSerializer(typeToSerializer);

                    using (XmlTextReader reader = new XmlTextReader(serialized, XmlNodeType.Document, null))
                        finalObject = serializer.Deserialize(reader);
                }
                catch (Exception)
                {
                    finalObject = null;
                }
            }

            return finalObject;
        }

        public static string Encrypt(this string field)
        {
            if (string.IsNullOrWhiteSpace(field))
                return field;

            string key = "#7Yg?ApQor4@b9=8"; //KeyPassword
            string iv = "*R2#4sW55m6";
            Encryption enc = new Encryption(Encoding.Default.GetBytes(key), Encoding.Default.GetBytes(iv));

            return enc.Encrypt(field);
        }

        public static string Decrypt(this string field)
        {
            if (string.IsNullOrWhiteSpace(field))
                return field;

            string key = "#7Yg?ApQor4@b9=8";
            string iv = "*R2#4sW55m6";
            Encryption enc = new Encryption(Encoding.Default.GetBytes(key), Encoding.Default.GetBytes(iv));

            return enc.Decrypt(field);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
        public static string EncryptDES(this object value, bool useHashing = false)
        {
            return DESTriple.EncryptString(value.ToString(), useHashing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
        public static string DecryptDES(this string value, bool useHashing = false)
        {
            return DESTriple.DecryptString(value, useHashing);
        }

        public static string GetHash(this Guid input)
        {
            return input.ToString().GetHash();
        }

        public static string GetHash(this string input)
        {
            HashAlgorithm hashAlgorithm = new SHA512CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static bool IsValidJson(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            var value = stringValue.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                (value.StartsWith("[") && value.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(value);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }

            return false;
        }
    }

    //Shopping legacy encrypt method 
    internal class Encryption
    {
        private TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        private UTF8Encoding utf8 = new UTF8Encoding();

        private byte[] keyValue;
        private byte[] iVValue;

        public byte[] Key
        {
            get { return keyValue; }
            set { keyValue = value; }
        }
        public byte[] iV
        {
            get { return iVValue; }
            set { iVValue = value; }
        }

        public Encryption(byte[] key, byte[] iV)
        {
            this.keyValue = key;
            this.iVValue = iV;
        }

        public byte[] Decrypt(byte[] bytes)
        {
            return Transform(bytes, des.CreateDecryptor(this.keyValue, this.iVValue));
        }

        public byte[] Encrypt(byte[] bytes)
        {
            return Transform(bytes, des.CreateEncryptor(this.keyValue, this.iVValue));
        }

        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Transform(input, des.CreateDecryptor(this.keyValue, this.iVValue));
            return utf8.GetString(output);
        }

        public string Encrypt(string text)
        {
            byte[] input = utf8.GetBytes(text);
            byte[] output = Transform(input, des.CreateEncryptor(this.keyValue, this.iVValue));
            return Convert.ToBase64String(output);
        }

        private byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            MemoryStream memory = new MemoryStream();
            CryptoStream stream = new CryptoStream(memory, cryptoTransform, CryptoStreamMode.Write);

            stream.Write(input, 0, input.Length);
            stream.FlushFinalBlock();

            memory.Position = 0;
            byte[] result = new byte[memory.Length];
            memory.Read(result, 0, result.Length);

            memory.Close();
            stream.Close();

            return result;
        }
    }

    internal class DESTriple
    {
        private const string secretKey = "9F6CC2E6393E468F";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
        public static string EncryptString(string inputString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(inputString);

            // Get the key from config file

            string key = secretKey;

            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return ToUrlSafeBase64(resultArray);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
        public static string DecryptString(string inputString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = FromUrlSafeBase64(inputString);

            //Get your key from config file to open the lock!
            string key = secretKey;

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string ToUrlSafeBase64(byte[] resultArray)
        {
            return Convert.ToBase64String(resultArray, 0, resultArray.Length).Replace('+', '-').Replace('/', '_').Replace("=", "");
        }

        public static byte[] FromUrlSafeBase64(string s)
        {
            while (s.Length % 4 != 0)
                s += "=";
            s = s.Replace('-', '+').Replace('_', '/');

            return Convert.FromBase64String(s);
        }
    }
}

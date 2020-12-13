using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// Class security extensions
    /// </summary>
    public static class SecurityExtensions
    {
        #region "  Constant keys  "

        private const string secretKeyDES = "9F6CC2E6393E468F";

        private static readonly byte[] secretKeyJWT = Encoding.UTF8.GetBytes("ed31b54d15639b3877d641600ba7d44015c0aaa013f0398a5af1c9cabd544183");

        private const string publicKeyRSA = "<RSAKeyValue><Modulus>muju/yxj8WldtbTAbnZTPuPhJ4phosXcKkh2OEspOGkzl0EAhxfrUyFPftoIFUf/V3Bhv0f7Md+hlozM4q6+UKx5VwCkmJAKS/Jc7s6hvBFZmydkdaLuZKNnZ25MUXGygn3fvycB/uc0X5QGvE3tDpWq3h6ga5EwgYaUpxom1PLJHdkWCe/rSg0nstCdSq8sWZ2uA3+0uR/pIuhuep+YQBZP97HR8mL38BWi6YM7hEgpZ6TJ4szYlc4ATai3TCHyD9206dhXPynePyZxqQunSNY4V04ncgQk1vSCUuC/kzg7aIect/lEOh2gscG0Dune+jlEIjOSwrWs9X+UAgLp/Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        private const string privateKeyRSA = "<RSAKeyValue><Modulus>muju/yxj8WldtbTAbnZTPuPhJ4phosXcKkh2OEspOGkzl0EAhxfrUyFPftoIFUf/V3Bhv0f7Md+hlozM4q6+UKx5VwCkmJAKS/Jc7s6hvBFZmydkdaLuZKNnZ25MUXGygn3fvycB/uc0X5QGvE3tDpWq3h6ga5EwgYaUpxom1PLJHdkWCe/rSg0nstCdSq8sWZ2uA3+0uR/pIuhuep+YQBZP97HR8mL38BWi6YM7hEgpZ6TJ4szYlc4ATai3TCHyD9206dhXPynePyZxqQunSNY4V04ncgQk1vSCUuC/kzg7aIect/lEOh2gscG0Dune+jlEIjOSwrWs9X+UAgLp/Q==</Modulus><Exponent>AQAB</Exponent><P>y+OlfixIyIA84DxjBNCjyZh9juzuW+ziFv2m8nEFh7ZDmziHYS0b9nei7EuQrEJr18Yzp+BF6V77sA3PsDzqqiaOL1ebkQrKSJ1NdlNpdarm0N3I/46BAgPpp6Hpo2ZmVhk1pfRj5SdX4k1OHLxEjr4GlurmpZI9JogiVym0WZc=</P><Q>woCY4Y+koDnIlelULF4oa6U5T0pk/OBv0w+3euOjQlHguewrQ3zS2J68tgSN7BzZcfJsjljdp5gsHkL3/mR8OJjC78HEl9X567e3X97TV/w4g/k7+AD/s/YFOrRXecxQK4kBPq9X2IhUz2RZeFSzcv2uztVo246UUSQDCgJyg4s=</Q><DP>go79dDQLT0i+sqA4j+bCWt8o15LkdzzS8gHvG6Q/9E1EMWsbVaC7HTIyw7kHpSbLQ9qJTWCRpATMikntyl7XrakOt0YUOeZ87c68wZ5cE5siPnEmum6YMaAryMongBicQ1nVPrWmGiD9Z840zLXJ+NW6Bn3YwAFK4xedVc9Ay3U=</DP><DQ>WZJpVW8WTGdv9YHHrssJ6FDlrJtBGKevN87EG4bbt34HdPfLEMBaRUIM+/Hq/fJnPS98SK2qHjVZE/KZTIEwJ8xJ8aoVhCsZdjFb9H2kbJ+N01EjCdpD57eDvv4wTroFrZbhiOGtHd3i2MOI5H51SZ6EM2JacMofiaKA98oavHM=</DQ><InverseQ>bgxs08VsC30gN0z9VNFXRQLxbmWwwkjAkREl4l0hulwt6WazyndtTxa3ScL+vFFH9/e4hs1vUUBI7/xLuKLp2/kJoVIENb9KPORJW9Dwy4vy7F2dCGRwrSy0wOGqWlbR1xegFrZxuLSGPUjjEcuVPSVonoA2+OVlRjoUgxS2sho=</InverseQ><D>iILGwlXDCSXaL9tGTNG3EE/OxYJ8Ae4s05IhpAAQJicHEL52kZYxiYNsQ5Qt1VsqGErvyJnNMikpg7s2fniRPBSRpWrqoYsll8HPxZ6QBmSfu0ueis/3FvaslAgt9wOj1LzKJPBiINhsXeHbgauGQPkBp1YD9mbvAOWxMASqvbKLFq7RArPRjM1K2NXTYEvFiXAAJKPeqVFR/hAsbOEu3zw91hyLMjMzII/zvTljT6VVqyz0V2nWtGcu2/MPKS1JFKmfCCQwcH1Um//buXoVbR33I8DbEtjSJLHlR8EBXdxT8wCFB0kku/FDnKpoL08psgKRQWCzbR/nGWoELFQLfQ==</D></RSAKeyValue>";

        #endregion

        /// <summary>
        /// Encode value JWT
        /// </summary>
        /// <param name="value">The value to encode</param>
        /// <param name="secretKey">The specific secret key in HS256 to use on encode JWT</param>
        /// <returns>The value encoded</returns>
        public static string EncodeJWT(this object value, string secretKey = null)
        {
            if (value.IsNull()) return string.Empty;

            byte[] secret = null;

            if (!secretKey.IsNullOrWhiteSpace())
                secret = Encoding.UTF8.GetBytes(secretKey);
            else
                secret = secretKeyJWT;

            Jose.JWT.JsonMapper = new JwtNewtonsoftMapper();
            return Jose.JWT.Encode(value, secret, Jose.JwsAlgorithm.HS256);
        }

        /// <summary>
        /// Encode value JWT
        /// </summary>
        /// <param name="value">The value to encode</param>
        /// <param name="secretKey">The specific secret key in HS256 to use on encode JWT</param>
        /// <returns>The value encoded</returns>
        public static string EncodeJWT(this IDictionary<string, object> value, string secretKey = null)
        {
            if (value.Count.IsZero()) return string.Empty;

            byte[] secret = null;

            if (!secretKey.IsNullorEmpty())
                secret = Encoding.UTF8.GetBytes(secretKey);
            else
                secret = secretKeyJWT;

            Jose.JWT.JsonMapper = new JwtNewtonsoftMapper();
            return Jose.JWT.Encode(value, secret, Jose.JwsAlgorithm.HS256);
        }

        /// <summary>
        /// Decode value JWT
        /// </summary>
        /// <param name="value">The value to decode</param>
        /// <param name="secretKey">The specific secret key in HS256 to use on decode JWT</param>
        /// <returns>The value decoded</returns>
        public static string DecodeJWT(this string value, string secretKey = null)
        {
            if (value.IsNullOrWhiteSpace()) return string.Empty;

            byte[] secret = null;

            if (!secretKey.IsNullOrWhiteSpace())
                secret = Encoding.UTF8.GetBytes(secretKey);
            else
                secret = secretKeyJWT;

            Jose.JWT.JsonMapper = new JwtNewtonsoftMapper();
            return Jose.JWT.Decode(value, secret);
        }

        /// <summary>
        /// Try to decode the Jose.JwsAlgorithm.HS256 encrypted string and cast to specified class.
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="value">Encrypted string</param>
        /// <param name="secretKey">Secret key</param>
        /// <returns>Desserialized object of the specified class type</returns>
        public static T TryDecodeJWT<T>(this string value, string secretKey = null) where T : class
        {
            try
            {
                Jose.JWT.JsonMapper = new JwtNewtonsoftMapper();
                return Jose.JWT.Decode<T>(value, Encoding.UTF8.GetBytes(secretKey));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string EncodeRSA(this string value, string publicKey = publicKeyRSA)
        {
            return System.Net.WebUtility.UrlEncode(RSACrypto.EncryptString(value, publicKey));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string EncodeRSA(this object value, string publicKey = publicKeyRSA)
        {
            return System.Net.WebUtility.UrlEncode(RSACrypto.EncryptString(value.ToString(), publicKey));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string DecodeRSA(this string value, string privateKey = privateKeyRSA)
        {
            return RSACrypto.DecryptString(System.Net.WebUtility.UrlDecode(value), privateKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="useHashing"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static string EncryptDES(this object value, bool useHashing = false, string secretKey = secretKeyDES)
        {
            return DESTriple.EncryptString(value.ToString(), useHashing, secretKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="useHashing"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static string DecryptDES(this string value, bool useHashing = false, string secretKey = secretKeyDES)
        {
            return DESTriple.DecryptString(value, useHashing, secretKey);
        }

        public static string EncryptGateway(string keyHash, string input)
        {
            if (input.IsNullorEmpty())
                return input;

            using (var hashMD5 = new MD5CryptoServiceProvider())
            {
                byte[] key = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(keyHash));
                byte[] data = Encoding.UTF8.GetBytes(input);

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = aes.CreateEncryptor();
                    byte[] dataEncrypted = cTransform.TransformFinalBlock(data, 0, data.Length);

                    string encryptionDataBase64 = Convert.ToBase64String(dataEncrypted, 0, dataEncrypted.Length);
                    string ivStr = BitConverter.ToString(aes.IV);
                    string concatenatedData = string.Format("{0}{1}", ivStr.Replace("-", string.Empty).ToLower(), encryptionDataBase64);

                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(concatenatedData));
                }
            }
        }

        /// <summary>
        /// Generate HMAC SHA 256 Payments
        /// </summary>
        /// <param name="value"></param>
        /// <param name="authKey"></param>
        /// <returns></returns>
        public static string GenerateHMACSHA256(this string value, string authKey)
        {
            byte[] bytes_key = Encoding.UTF8.GetBytes(authKey);
            byte[] bytes_message = Encoding.UTF8.GetBytes(value);

            using (var hmacSha256 = new HMACSHA256(bytes_key))
            {
                byte[] hashmessage = hmacSha256.ComputeHash(bytes_message);
                string hash = Convert.ToBase64String(hashmessage);

                return hash;
            }
        }

        /// <summary>
        /// Generate HMAC SHA
        /// </summary>
        /// <param name="value"></param>
        /// <param name="authKey"></param>
        /// <returns></returns>
        public static string GenerateHMACSHA(this string value, string authKey)
        {
            var hmacAuthenticationKey = new HMACSHA256(Encoding.UTF8.GetBytes(authKey));
            var byteHash = hmacAuthenticationKey.ComputeHash(Encoding.UTF8.GetBytes(value));

            var strbHash = new StringBuilder(byteHash.Length * 2);

            foreach (byte b in byteHash)
                strbHash.AppendFormat("{0:x2}", b);

            return strbHash.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class RSACrypto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string EncryptString(string inputString, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(publicKey);
                var inputBytes = Encoding.UTF32.GetBytes(inputString);
                var outBytes = rsa.Encrypt(inputBytes, false);
                return Convert.ToBase64String(outBytes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string DecryptString(string inputString, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(privateKey);
                var inputBytes = Convert.FromBase64String(inputString);
                var outBytes = rsa.Decrypt(inputBytes, false);
                return Encoding.UTF32.GetString(outBytes);
            }
        }
    }

    internal class DESTriple
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="useHashing"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptString(string inputString, bool useHashing, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(inputString);

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
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="useHashing"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptString(string inputString, bool useHashing, string key)
        {
            byte[] keyArray;
            //get the byte code of the string
            byte[] toEncryptArray = FromUrlSafeBase64(inputString);

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
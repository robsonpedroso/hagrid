using Microsoft.Owin.Security;
using System;
using System.Configuration;

namespace Hagrid.Infra.Utils.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtTokenFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private static readonly string secretKey = ConfigurationManager.AppSettings["JwtTokenKey"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var token = new JwtToken(data);

            return token.EncodeJWT(secretKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="protectedText"></param>
        /// <returns></returns>
        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}

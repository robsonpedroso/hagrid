using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace Hagrid.Infra.Utils.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtToken
    {
        private static readonly string[] excludedProperties = { ".refresh", ".issued", ".expires" };

        /// <summary>
        /// // Describes a point in time, defined as the number of seconds that have elapsed since 00:00:00 UTC, Thursday, 1 January 1970, not counting leap seconds.
        /// See https://en.wikipedia.org/wiki/Unix_time />
        /// </summary>
        private DateTime centuryBegin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        #region "  Func<string, string> getClaimType = (rawType) =>  "
        private static Func<string, string> getClaimType = (rawType) =>
        {
            if (rawType.IndexOf('/') >= 0)
                return rawType.Substring(rawType.LastIndexOf('/') + 1);

            return rawType;
        };
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public JwtToken() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public JwtToken(AuthenticationTicket data)
        {
            var identity = data.Identity;
            var properties = data.Properties;

            clm = new Dictionary<string, string>();

            foreach (var claim in identity.Claims.Where(c => !c.Type.Equals("urn:oauth:scope")))
            {
                switch (claim.Type)
                {
                    case System.Security.Claims.ClaimTypes.Name:
                        name = claim.Value;
                        break;
                    case System.Security.Claims.ClaimTypes.Role:
                        role = claim.Value;
                        break;
                    default:
                        clm.Add(getClaimType(claim.Type), claim.Value);
                        break;
                }
            }

            iss = "https://accounts.hagrid.com.br";
            nbf = Math.Round((properties.IssuedUtc.Value - centuryBegin).TotalSeconds);
            exp = Math.Round((properties.ExpiresUtc.Value - centuryBegin).TotalSeconds);
            rfs = properties.AllowRefresh.Value;

            foreach (var property in properties.Dictionary.Where(p => !excludedProperties.Contains(p.Key)))
                clm.Add(property.Key, property.Value);

            type = identity.AuthenticationType.ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// The "iss" (issuer) claim identifies the principal that issued the
        /// JWT.  The processing of this claim is generally application specific.
        /// The "iss" value is a case-sensitive string containing a StringOrURI
        /// value.  Use of this claim is OPTIONAL.
        /// </summary>
        public string iss { get; set; }

        /// <summary>
        ///  The "aud" (audience) claim identifies the recipients that the JWT is
        ///  intended for.  Each principal intended to process the JWT MUST
        ///  identify itself with a value in the audience claim.
        /// </summary>
        public string aud { get; set; }

        /// <summary>
        /// The "nbf" (not before) claim identifies the time before which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public double nbf { get; set; }

        /// <summary>
        /// The "exp" (expiration time) claim identifies the expiration time on
        /// or after which the JWT MUST NOT be accepted for processing.  The
        /// processing of the "exp" claim requires that the current date/time
        /// MUST be before the expiration date/time listed in the "exp" claim.
        /// </summary>
        public double exp { get; set; }

        /// <summary>
        /// Gets or sets if refreshing the authentication session should be allowed.
        /// </summary>
        public bool rfs { get; set; }

        /// <summary>
        /// The URI for a claim that specifies the name of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The URI for a claim that specifies the role of an entity, http://schemas.microsoft.com/ws/2008/06/identity/claims/role.
        /// </summary>
        public string role { get; set; }

        /// <summary>
        /// Represents a claims.
        /// </summary>
        public Dictionary<string, string> clm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsExpired()
        {
            return Math.Round((DateTime.UtcNow - centuryBegin).TotalSeconds) >= exp;
        }
    }
}

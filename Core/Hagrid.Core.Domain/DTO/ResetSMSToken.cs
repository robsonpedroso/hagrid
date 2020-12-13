using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;
using System.Configuration;
using Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.DTO
{
    public class ResetSMSToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public DateTime ExpiresUtc { get; set; }

        [JsonProperty("token_validation_url")]
        public string TokenValidationUrl { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        public ResetSMSToken(Entities.ResetSMSToken smsToken)
        {
            AccessToken = smsToken.Code;
            TokenType = smsToken.TokenType;
            ExpiresUtc = smsToken.ExpiresUtc;
            PhoneNumber = string.Format("(XX) XXXX-{0}", smsToken.PhoneNumber.Substring(smsToken.PhoneNumber.Length - 4));

            var accountSite = Config.AccountsSiteURL;

            TokenValidationUrl = string.Format("{0}/#/sms-validate/{1}", accountSite, smsToken.Code);

            if (!smsToken.UrlBack.IsNullorEmpty())
                TokenValidationUrl += string.Format("?ub={0}", smsToken.UrlBack.URLEncode());
        }
    }
}

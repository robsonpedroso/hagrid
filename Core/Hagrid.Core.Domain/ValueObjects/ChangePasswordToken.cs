using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.ValueObjects
{
    public class ChangePasswordToken : SmallToken
    {
        public Guid ApplicationStoreCode { get; private set; }
        public string UrlBack { get; private set; }
        public bool ShowMessage { get; private set; }
        public string Token { get; private set; }

        public ChangePasswordToken() { }

        public ChangePasswordToken(string token)
        {
            try
            {
                JObject parsed = JObject.Parse(Jose.JWT.Decode(token, Config.JwtTokenKeyByte));

                this.OwnerCode = new Guid(parsed["id"].ToString());
                this.ApplicationStoreCode = new Guid(parsed["cid"].ToString());
                this.UrlBack = parsed["ub"].ToString();
                this.ShowMessage = parsed["sm"].Value<bool>();
                this.ExpiresUtc = new DateTime(parsed["exp"].Value<long>());
            }
            catch
            {
                this.ExpiresUtc = DateTime.Today.AddDays(-1);
            }
        }

        public ChangePasswordToken(Guid accountCode, Guid applicationStoreCode, string urlBack, bool showMessage)
        {
            this.OwnerCode = accountCode;
            this.ApplicationStoreCode = applicationStoreCode;
            this.UrlBack = urlBack;
            this.ShowMessage = showMessage;
            this.ExpiresUtc = DateTime.UtcNow.AddSeconds(Config.ChangePassworkTokenExpirationTimeInSeconds);
        }

        public override string ToString()
        {
            var dic = new Dictionary<string, object>()
            {
                { "id", this.OwnerCode.Value },
                { "cid", this.ApplicationStoreCode },
                { "ub", this.UrlBack },
                { "sm", this.ShowMessage },
                { "exp", this.ExpiresUtc.Ticks }
            };

            string AccountsSiteURL = "http://localhost:55777";
            if (!string.IsNullOrWhiteSpace(Config.AccountsSiteURL))
                AccountsSiteURL = Config.AccountsSiteURL;

            this.Token = Jose.JWT.Encode(dic, Config.JwtTokenKeyByte, Jose.JwsAlgorithm.HS256);

            return string.Format("{0}/{1}/{2}", AccountsSiteURL, Properties.ChangePasswordLocation, this.Token);
        }

        public bool IsExpired()
        {
            return this.ExpiresUtc.CompareTo(DateTime.UtcNow) <= 0;
        }

        public bool Validate()
        {
            return this != null && !this.IsExpired();
        }
    }
}

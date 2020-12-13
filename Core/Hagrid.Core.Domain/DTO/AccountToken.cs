using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Hagrid.Infra.Utils;
using System;
using System.Text;

namespace Hagrid.Core.Domain.DTO
{
    public class AccountToken
    {
        private JToken _identity;
        private string _accessToken;

        [JsonProperty("code")]
        public Guid? Code { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken
        {
            get { return _accessToken; }

            set
            {
                _accessToken = value;

                try
                {
                    if (!string.IsNullOrEmpty(_accessToken))
                    {
                        var payload = _accessToken.Split('.')[1];

                        if (!string.IsNullOrWhiteSpace(payload))
                        {
                            int mod4 = payload.Length % 4;

                            if (mod4 > 0)
                                payload += new string('=', 4 - mod4);

                            payload = Encoding.UTF8.GetString(Convert.FromBase64String(payload));

                            _identity = JsonConvert.DeserializeObject<JToken>(payload);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Account code from member
        /// </summary>
        [JsonIgnore]
        public Guid AccountCode
        {
            get
            {
                if (_identity != null && _identity["role"].ToString() == "Member")
                {
                    var accountCode = _identity["clm"]["sid"];

                    return Guid.Parse(accountCode.Value<string>());
                }

                return Guid.Empty;
            }
        }

        public AccountToken() { }

        public AccountToken(Entities.Account account)
        {
            this.Code = account.Code;
        }

        public override string ToString()
        {
            return this.ToJsonString(ignoreNullValue: true);
        }
    }
}

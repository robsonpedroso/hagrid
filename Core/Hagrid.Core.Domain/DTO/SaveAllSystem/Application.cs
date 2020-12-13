using Newtonsoft.Json;
using System;

namespace Hagrid.Core.Domain.DTO.SaveAllSystem
{
    public class Application
    {
        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }

        [JsonProperty("client")]
        public Guid Client { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("js_client")]
        public Guid JSClient { get; set; }

        [JsonProperty("refresh_token_life_time_in_minutes")]
        public int RefreshTokenLifeTimeInMinutes { get; set; }

        [JsonProperty("member_type")]
        public string MemberType { get; set; }

        [JsonProperty("auth_type")]
        public string AuthType { get; set; }

        [JsonProperty("allowed_origins")]
        public string AllowedOrigins { get; set; }

        public Application() { }

    }
}

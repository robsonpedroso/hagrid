using Newtonsoft.Json;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Domain.DTO
{
    public class ApplicationStore
    {
        [JsonProperty("store_code")]
        public Guid? StoreCode { get; set; }

        [JsonProperty("store_name")]
        public string StoreName { get; set; }

        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }

        [JsonProperty("confidential_client")]
        public Guid ConfidentialClient { get; set; }

        [JsonProperty("confidential_secret")]
        public string ConfidentialSecret { get; set; }

        [JsonProperty("js_client")]
        public Guid JSClient { get; set; }

        [JsonProperty("refresh_token_life_time_in_minutes")]
        public int RefreshTokenLifeTimeInMinutes { get; set; }

        [JsonProperty("member_type")]
        public string MemberType { get; set; }

        [JsonProperty("auth_type")]
        public string AuthType { get; set; }

        [JsonProperty("confidential_client_hash")]
        public string ConfidentialClientHash { get; set; }

        [JsonProperty("allowed_origins")]
        public string[] AllowedOrigins { get; set; }

        public ApplicationStore() { }

        public ApplicationStore(Entities.ApplicationStore applicationStore, bool withDataStore = true)
        {
            if (withDataStore)
            {
                this.StoreCode = applicationStore.Store.Code;
                this.StoreName = applicationStore.Store.Name;
                this.ConfidentialClientHash = applicationStore.Store.Code.ToString();
            }

            this.ApplicationName = applicationStore.Application.Name;
            this.ConfidentialClient = applicationStore.ConfClient.Value;
            this.ConfidentialSecret = applicationStore.ConfSecret;
            this.JSClient = applicationStore.JSClient.Value;
            this.RefreshTokenLifeTimeInMinutes = applicationStore.Application.RefreshTokenLifeTimeInMinutes;
            this.MemberType = applicationStore.Application.MemberType.ToString();
            this.AuthType = applicationStore.Application.AuthType.ToString();
            this.AllowedOrigins = !applicationStore.JSAllowedOrigins.IsNullOrWhiteSpace() ? applicationStore.JSAllowedOrigins.Split(',') : new string[] { };
        }

        public Entities.ApplicationStore Transfer()
        {
            return new Entities.ApplicationStore()
            {
                StoreCode = this.StoreCode.Value,
                ConfClient = this.ConfidentialClient,
                ConfSecret = this.ConfidentialSecret,
                JSClient = this.JSClient,
                
                Application = new Entities.Application()
                {
                    Name = this.ApplicationName,
                    RefreshTokenLifeTimeInMinutes = this.RefreshTokenLifeTimeInMinutes,
                },
                Store = new Entities.Store()
                {
                    Name = this.StoreName
                },
            };
        }
    }
}

using Newtonsoft.Json;
using System;

namespace Hagrid.Core.Domain.DTO.SaveAllSystem
{
    public class Account
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("document")]
        public string Document { get; set; }

        [JsonProperty("save_date")]
        public DateTime SaveDate { get; set; }

    }
}

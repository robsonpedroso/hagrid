using System;
using Newtonsoft.Json;

namespace Hagrid.Core.Domain.DTO
{
    public class AccountRoleResult
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("account")]
        public Account Account { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("status")]
        public bool? Status { get; set; }


    }
}

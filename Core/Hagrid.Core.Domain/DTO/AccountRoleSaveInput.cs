using System;
using Newtonsoft.Json;

namespace Hagrid.Core.Domain.DTO
{
    public class AccountRoleSaveInput
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("account_code")]
        public Guid AccountCode { get; set; }

        [JsonProperty("status")]
        public bool? Status { get; set; }
    }
}

using Newtonsoft.Json;
using System;

namespace Hagrid.Core.Domain.DTO
{
    public class AccountPermission
    {
        [JsonProperty("store_code")]
        public Guid? StoreCode { get; set; }

        [JsonProperty("account_codes")]
        public Guid[] AccountCodes { get; set; }

        [JsonProperty("applications")]
        public string[] Applications { get; set; }
    }
}

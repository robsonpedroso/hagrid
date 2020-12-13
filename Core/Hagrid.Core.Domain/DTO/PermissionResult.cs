using Newtonsoft.Json;
using System;

namespace Hagrid.Core.Domain.DTO
{
    public class PermissionResult
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("resource")]
        public ResourceResult Resource { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("operations")]
        public string Operations { get; set; }

        [JsonProperty("status")]
        public Boolean? Status { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    public class PermissionSaveInput
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("resource")]
        public ResourceSaveInput Resource { get; set; }

        [JsonProperty("operations")]
        public string Operations { get; set; }

        [JsonProperty("status")]
        public Boolean? Status { get; set; }
    }
}

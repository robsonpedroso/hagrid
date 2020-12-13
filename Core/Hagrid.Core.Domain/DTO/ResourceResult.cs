using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    public class ResourceResult
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("internal_code")]
        public string InternalCode { get; set; }

        [JsonProperty("application")]
        public Application Application { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("operations")]
        public string Operations { get; set; }
    }
}

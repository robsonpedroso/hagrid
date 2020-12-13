using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    public class ResourceSaveInput
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("internal_code")]
        public string InternalCode { get; set; }

        [JsonProperty("application_code")]
        public Guid ApplicationCode { get; set; }
    }
}

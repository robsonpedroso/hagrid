using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    [JsonObject("errors")]
    public class RequisitionError
    {
        [JsonProperty("line")]
        public int Line { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("messages")]
        public List<String> ErrorMessages { get; set; }

        public RequisitionError(Entities.RequisitionError requisitionError)
        {
            this.Line = requisitionError.Line;
            this.Email = requisitionError.Email;
            this.Name = requisitionError.Name;
            this.ErrorMessages = requisitionError.ErrorMessages;
        }
    }
}

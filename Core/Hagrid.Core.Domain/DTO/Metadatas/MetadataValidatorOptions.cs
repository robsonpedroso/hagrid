using Newtonsoft.Json;
using System.Collections.Generic;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.DTO
{
    [JsonObject("metadata_validator_Options")]
    public class MetadataValidatorOptions : MetadataValidator
    {
        [JsonProperty("values")]
        public List<string> Values { get; set; }

        public MetadataValidatorOptions() { }

        public override DO.MetadataValidator Transfer()
        {
            return new DO.MetadataValidatorOptions
            {
                Type = Type,
                Values = Values
            };
        }
    }
}
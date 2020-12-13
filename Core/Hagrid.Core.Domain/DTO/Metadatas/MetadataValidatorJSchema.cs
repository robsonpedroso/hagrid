using Newtonsoft.Json;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.DTO
{
    [JsonObject("metadata_validator_jschema")]
    public class MetadataValidatorJSchema : MetadataValidator
    {
        [JsonProperty("schema")]
        public string Schema { get; set; }

        public MetadataValidatorJSchema() { }

        public override DO.MetadataValidator Transfer()
        {
            return new DO.MetadataValidatorJSchema
            {
                Type = Type,
                Schema = Schema
            };
        }
    }
}
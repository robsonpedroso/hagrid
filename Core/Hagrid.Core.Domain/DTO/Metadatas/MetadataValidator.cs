using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.DTO
{
    [JsonObject("metadata_validator")]
    [JsonConverter(typeof(DiscriminatorConverter))]
    public abstract class MetadataValidator
    {
        [JsonDiscriminator]
        [JsonProperty("type")]
        public ValidatorType Type { get; set; }

        public abstract DO.MetadataValidator Transfer();
    }
}
using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using DTO = Hagrid.Core.Domain.DTO;
 
namespace Hagrid.Core.Domain.Entities
{
    [JsonConverter(typeof(DiscriminatorConverter))]
    public abstract class MetadataValidator
    {
        [JsonDiscriminator]
        public ValidatorType Type { get; set; }

        public abstract bool IsValid(BaseMetadata metadata);

        public abstract DTO.MetadataValidator GetResult();
    }
}
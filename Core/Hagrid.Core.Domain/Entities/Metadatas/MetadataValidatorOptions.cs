using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System.Collections.Generic;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Entities
{
    public class MetadataValidatorOptions : MetadataValidator
    {
        public List<string> Values { get; set; }

        public MetadataValidatorOptions()
        {
            Type = ValidatorType.Options;
        }

        public override bool IsValid(BaseMetadata metadata)
        {
            if (metadata.Value.IsNullorEmpty() || Values.IsNull() || Values.Count.IsZero()) return true;

            bool valid = Values.Any(v => v == metadata.Value);

            return valid;
        }

        public override DTO.MetadataValidator GetResult()
        {
            return new DTO.MetadataValidatorOptions
            {
                Type = Type,
                Values = Values
            };
        }
    }
}
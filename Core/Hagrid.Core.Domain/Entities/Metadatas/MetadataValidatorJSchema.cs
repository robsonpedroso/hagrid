using NJsonSchema;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Entities
{
    public class MetadataValidatorJSchema : MetadataValidator
    {
        public string Schema { get; set; }

        public MetadataValidatorJSchema()
        {
            Type = ValidatorType.JsonSchema;
        }

        public override bool IsValid(BaseMetadata metadata)
        {
            if (metadata.Value.IsNullorEmpty()) return true;

            var schema = JsonSchema.FromJsonAsync(Schema);

            schema.Wait();

            var errors = schema.Result.Validate(metadata.Value);

            bool valid = errors.Count == 0;

            return valid;
        }

        public override DTO.MetadataValidator GetResult()
        {
            return new DTO.MetadataValidatorJSchema
            {
                Type = Type,
                Schema = Schema
            };
        }
    }
}
using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.DTO
{
    [JsonConverter(typeof(DiscriminatorConverter))]
    public class Restriction
    {
        [JsonProperty("code")]
        public Guid? Code { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("role_code")]
        public Guid? RoleCode { get; set; }

        [JsonProperty("type"), JsonDiscriminator]
        public RestrictionType Type { get; set; }

        public Restriction() { }

        public Restriction(Entities.Restriction restriction)
        {
            Code = restriction.Code;
            Type = restriction.Type;

            RoleCode = restriction.Role?.Code ?? restriction.RoleCode;

            Role = new Role
            {
                Name = restriction.Role?.Name,
                Code = restriction.Role?.Code ?? restriction.RoleCode
            };
        }

        public virtual Entities.Restriction Transfer() { return default; }

        public virtual void IsValid()
        {
            var validationErros = new List<string>();

            if (Role?.Code.IsEmpty() ?? false)
                validationErros.Add("Código do grupo não informado");

            if (!Enum.IsDefined(typeof(RestrictionType), Type))
                validationErros.Add("Tipo de restrição inválido");

            var errorMessage = string.Join(" | ", validationErros);

            if (!string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException(errorMessage);
        }
    }
}

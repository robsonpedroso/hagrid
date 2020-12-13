using Newtonsoft.Json;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.DTO
{
    public class Permission
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("resource")]
        public Resource Resource { get; set; }

        [JsonProperty("resource_code")]
        public Guid? ResourceCode { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("role_code")]
        public Guid? RoleCode { get; set; }

        [JsonProperty("operations")]
        public string Operations { get; set; }

        public Permission() { }

        public Permission(Entities.Permission permission)
        {
            this.Code = permission.Code;
            this.Operations = permission.Operations.ToString();
            this.Resource = new Resource()
            {
                Name = permission.Resource.Name,
                Code = permission.Resource.Code,
                InternalCode = permission.Resource.InternalCode,
                Operations = permission.Resource.Operations.ToString(),
                Application = new Application()
                {
                    Name = permission.Resource.Application.Name,
                    Code = permission.Resource.ApplicationCode
                }
            };

            this.Role = new Role()
            {
                Name = permission.Role.Name,
                Code = permission.Role.Code
            };
        }

        public virtual Entities.Permission Transfer(bool includeRole = false)
        {
            return new Entities.Permission()
            {
                Code = !this.Code.IsEmpty() ? this.Code : Guid.NewGuid(),
                Operations = (Enums.Operations)Enum.Parse(typeof(Enums.Operations), this.Operations),
                ResourceCode = this.Resource.Code,
                Status = true,
                RoleCode = includeRole ? this.Role.Code : Guid.Empty
            };
        }

        public void IsValid()
        {
            List<string> validationErros = new List<string>();

            if (this.Role.IsNull() || this.Role.Code.IsNull() || this.Role.Code.IsEmpty())
                validationErros.Add("Código do grupo não informado");

            if (this.Resource.IsNull() || this.Resource.Code.IsNull() || this.Resource.Code.IsEmpty())
                validationErros.Add("Código do módulo não informado");

            if (this.Operations.IsNullOrWhiteSpace())
                validationErros.Add("Operações não informadas");
            else
            {
                var isCorrect = ValidateOperations(this.Operations);

                if (!isCorrect)
                    validationErros.Add("Operações no formato incorreto");
            }

            var errorMessage = string.Join(" | ", validationErros);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public bool ValidateOperations(string operations)
        {
            Enums.Operations result;
            return Enum.TryParse<Enums.Operations>(operations, out result);
        }
    }
}

using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.DTO
{
    public class Resource
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("internal_code")]
        public string InternalCode { get; set; }

        [JsonProperty("application")]
        public Application Application { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("operations")]
        public string Operations { get; set; }

        [JsonProperty("type_code")]
        public int? TypeCode { get; set; }

        [JsonProperty("type")]
        public ResourceType? Type { get; set; }

        [JsonProperty("type_description")]
        public string TypeDescription { get; set; }

        public Resource() { }

        public Resource(Entities.Resource resource)
        {
            this.Code = resource.Code;
            this.Name = resource.Name;
            this.Description = resource.Description;
            this.Operations = resource.Operations.ToString();
            this.Type = resource.Type;
            this.TypeCode = resource.Type.AsInt();
            this.TypeDescription = resource.Type.GetDescription();
            this.InternalCode = resource.InternalCode;
        }

        public Resource(Entities.Resource resource, bool includeApp = false)
            : this(resource)
        {
            if (includeApp)
                this.Application = new Application(resource.Application);
        }

        public virtual Entities.Resource Transfer()
        {
            return new Entities.Resource()
            {
                Code = !this.Code.IsEmpty() ? this.Code : Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Operations = (Enums.Operations)Enum.Parse(typeof(Enums.Operations), this.Operations),
                InternalCode = this.InternalCode,
                Application = new Entities.Application()
                {
                    Code = this.Application.Code
                }
            };
        }

        public void IsValid()
        {
            List<string> validationErros = new List<string>();

            if (string.IsNullOrWhiteSpace(this.Name))
                validationErros.Add("Nome do módulo não informado");

            if (this.Application.IsNull() || !this.Application.Code.HasValue || this.Application.Code.Value.IsEmpty())
                validationErros.Add("Código da aplicação não informado");

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
        private bool ValidateOperations(string operations)
        {
            Enums.Operations result;
            return Enum.TryParse<Enums.Operations>(operations, out result);
        }
    }
}

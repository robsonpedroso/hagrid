using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Core.Domain.DTO
{
    public class Role
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("store")]
        public Store Store { get; set; }

        [JsonProperty("account_roles")]
        public List<AccountRole> AccountRoles { get; set; }

        [JsonProperty("permissions")]
        public List<Permission> Permissions { get; set; }

        [JsonProperty("status")]
        public bool? Status { get; set; }

        [JsonProperty("type_code")]
        public int? TypeCode { get; set; }

        [JsonProperty("type")]
        public RoleType? Type { get; set; }

        [JsonProperty("type_description")]
        public string TypeDescription { get; set; }

        public Role() { }

        public Role(Entities.Role role)
        {
            this.Code = role.Code;
            this.Name = role.Name;
            this.Description = role.Description;
            this.Status = role.Status;
            this.Type = role.Type;
            this.TypeCode = role.Type.AsInt();
            this.TypeDescription = role.Type.GetDescription();
            this.Store = new Store()
            {
                Code = role.Store.Code,
                Name = role.Store.Name,
                IsMain = role.Store.IsMain
            };
        }
        public Role(Entities.Role role, bool detail)
            : this(role)
        {
            if (detail)
            {
                this.AccountRoles = role.AccountRoles.Select(ac => new AccountRole(ac)).ToList();
                this.Permissions = role.Permissions.Select(p => new Permission(p)).ToList();
            }
        }

        public Role(Entities.Role role, bool withPermission, bool withAccountRole)
            : this(role)
        {
            if (withPermission)
                this.Permissions = role.Permissions.Select(p => new Permission(p)).ToList();

            if (withAccountRole)
                this.AccountRoles = role.AccountRoles.Select(ac => new AccountRole(ac)).ToList();
        }

        public virtual Entities.Role Transfer()
        {
            return new Entities.Role()
            {
                Code = !this.Code.IsEmpty() ? this.Code : Guid.NewGuid(),
                Name = this.Name,
                Description = this.Description,
                Status = this.Status.Value,
                Store = new Entities.Store() { Code = this.Store.Code },
                AccountRoles = !this.AccountRoles.IsNull() ? this.AccountRoles.Select(x => x.Transfer()).ToList() : null,
                Permissions = !this.Permissions.IsNull() ? this.Permissions.Select(x => x.Transfer()).ToList() : null
            };
        }

        public void IsValid()
        {
            List<string> validationErros = new List<string>();

            if (string.IsNullOrWhiteSpace(this.Name))
                validationErros.Add("Nome do grupo não informado");

            if (string.IsNullOrWhiteSpace(this.Description))
                validationErros.Add("Descrição do grupo não informado");

            if (this.Store.IsNull() || this.Store.Code.IsNull() || this.Store.Code.IsEmpty())
                validationErros.Add("Código da loja não informado");

            if (!this.Permissions.IsNull())
            {
                if (this.Permissions.Count == 0)
                    validationErros.Add("Nenhuma permissão informada");
                else
                {

                    if (this.Permissions.Any(x => x.Operations.IsNullOrWhiteSpace()))
                        validationErros.Add("Operações não informadas");

                    var isCorrect = this.Permissions.Select(a => a.ValidateOperations(a.Operations));

                    if (isCorrect.Contains(false))
                        validationErros.Add("Operações no formato incorreto");

                    if (this.Permissions.Any(x => x.Resource.IsNull() || x.Resource.Code.IsNull() || x.Resource.Code.IsEmpty()))
                        validationErros.Add("Código do módulo não informado");
                }
            }

            if (!this.AccountRoles.IsNull())
            {
                if (this.AccountRoles.Count == 0)
                    validationErros.Add("Nenhum código de usuário informado");
                else if (this.AccountRoles.Any(x => x.AccountCode.IsNull() || x.AccountCode.IsEmpty()))
                    validationErros.Add("Código do usuário não informado");
            }

            var errorMessage = string.Join(" | ", validationErros);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }
        }
    }
}

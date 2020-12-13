using System;
using Newtonsoft.Json;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.DTO
{
    public class AccountRole
    {
        [JsonProperty("code")]
        public Guid? Code { get; set; }

        [JsonProperty("account")]
        public Account Account { get; set; }

        [JsonProperty("account_code")]
        public Guid? AccountCode { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("role_code")]
        public Guid? RoleCode { get; set; }

        [JsonProperty("status")]
        public bool? Status { get; set; }

        public AccountRole(){}

        public AccountRole(Entities.AccountRole accountRole)
        {
            this.AccountCode = accountRole.AccountCode;
            this.Account = new Account()
            {
                Code = accountRole.AccountCode,
                Email = accountRole.Account.Email,
                Document = accountRole.Account.Document
            };

        }
        public virtual Entities.AccountRole Transfer()
        {
            return new Entities.AccountRole()
            {
                Code = this.Code.HasValue && !this.Code.Value.IsEmpty() ? this.Code.Value : Guid.NewGuid(),
                AccountCode = this.AccountCode.Value,
                Status = this.Status.Value
            };
        }

    }
}

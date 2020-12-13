using Hagrid.Infra.Contracts;
using System;

namespace Hagrid.Core.Domain.Entities
{
    public class AccountRole : IEntity, IStatus, IDate
    {
        public Guid Code { get; set; }
        public Account Account { get; set; }
        public Guid AccountCode { get; set; }
        public Role Role { get; set; }
        public Guid RoleCode { get; set; }
        public bool Status { get; set; }
        public DateTime SaveDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public AccountRole() { }

        public AccountRole(Role role, Guid accountCode)
        {
            Code = Guid.NewGuid();
            AccountCode = accountCode;
            RoleCode = role.Code;
            Status = true;
            UpdateDate = DateTime.Now;
            SaveDate = DateTime.Now;
        }
    }
}

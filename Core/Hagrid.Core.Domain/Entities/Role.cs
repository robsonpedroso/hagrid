using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Entities
{
    public class Role : IEntity, IStatus, IDate
    {
        public Guid Code { get; set; }
        public Guid StoreCode { get; set; }
        public Store Store { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public RoleType Type { get; set; }
        public DateTime SaveDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<Permission> Permissions { get; set; }
        public ICollection<AccountRole> AccountRoles { get; set; }
        public ICollection<Restriction> Restrictions { get; set; }

        public Role() { }
    }
}

using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;

namespace Hagrid.Core.Domain.Entities
{
    public class Permission : IEntity, IStatus
    {
        public Guid Code { get; set; }
        public Role Role { get; set; }
        public Guid RoleCode { get; set; }
        public Resource Resource { get; set; }
        public Guid ResourceCode { get; set; }
        public Operations Operations { get; set; }
        public bool Status { get; set; }
        public DateTime SaveDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Permission() { }
    }
}

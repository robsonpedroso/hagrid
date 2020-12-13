using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Entities
{
    public class Resource : IEntity
    {
        public Guid Code { get; set; }
        public string InternalCode { get; set; }
        public Application Application { get; set; }
        public Guid ApplicationCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Operations Operations { get; set; }
        public ResourceType Type { get; set; }
        public DateTime SaveDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<Permission> Permissions { get; set; }
        
        public Resource() { }
    }
}

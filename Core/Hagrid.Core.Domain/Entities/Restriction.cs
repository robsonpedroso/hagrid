using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;

namespace Hagrid.Core.Domain.Entities
{
    public abstract class Restriction : IEntity
    {
        [JsonIgnore]
        public virtual Guid Code { get; set; }

        [JsonIgnore]
        public virtual Role Role { get; set; }

        [JsonIgnore]
        public virtual Guid RoleCode { get; set; }

        [JsonIgnore]
        public abstract RestrictionType Type { get; }

        [JsonIgnore]
        public virtual DateTime SaveDate { get; set; }

        [JsonIgnore]
        public virtual DateTime UpdateDate { get; set; }

        [JsonIgnore]
        public virtual string ObjectSerialized { get; set; }

        public abstract DTO.Restriction GetResult();
    }
}

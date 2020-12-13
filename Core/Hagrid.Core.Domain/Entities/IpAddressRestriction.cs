using Newtonsoft.Json;
using Hagrid.Core.Domain.DTO;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Entities
{
    public class IpAddressRestriction : Restriction
    {
        [JsonIgnore]
        public override RestrictionType Type { get => RestrictionType.IpAddress; }

        public virtual List<string> AllowedIpAddresses { get; set; }

        public override DTO.Restriction GetResult() => new DTO.IpAddressRestriction(this);

        public override string ObjectSerialized
        {
            get => this.ToJsonString();
            set
            {
                if (!value.IsNullOrWhiteSpace())
                {
                    var obj = value.JsonTo<IpAddressRestriction>();
                    AllowedIpAddresses = obj.AllowedIpAddresses;
                }
            }
        }
    }
}

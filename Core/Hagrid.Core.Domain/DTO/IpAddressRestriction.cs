using Newtonsoft.Json;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Core.Domain.DTO
{
    public class IpAddressRestriction : Restriction
    {
        [JsonProperty("allowed_ip_addresses")]
        public List<string> AllowedIpAddresses { get; set; }

        public IpAddressRestriction() { }

        public IpAddressRestriction(Entities.IpAddressRestriction restriction) 
            : base(restriction)
        {
            AllowedIpAddresses = restriction.AllowedIpAddresses;
        }

        public override Entities.Restriction Transfer()
        {
            return new Entities.IpAddressRestriction()
            {
                Code = (Code?.IsEmpty() ?? true) ? Guid.NewGuid() : Code.Value,
                RoleCode = Role?.Code ?? RoleCode ?? Guid.Empty,
                AllowedIpAddresses = AllowedIpAddresses,
            };
        }

        public override void IsValid()
        {
            base.IsValid();

            if ((AllowedIpAddresses?.Count ?? 0) <= 0)
                throw new ArgumentException("Os ips permitidos não foram preenchidos");

            if (AllowedIpAddresses.Any(x => x.IsNullOrWhiteSpace()))
                throw new ArgumentException("Ips devem ser informados corretamente");
        }
    }
}

using Newtonsoft.Json;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Core.Domain.DTO
{
    public class PeriodRestriction : Restriction
    {
        [JsonProperty("periods")]
        public List<AccessPeriod> Periods { get; set; }

        public PeriodRestriction() { }

        public PeriodRestriction(Entities.PeriodRestriction restriction) 
            : base(restriction)
        {
            Periods = restriction.Periods?.Select(p => new AccessPeriod(p)).ToList();
        }

        public override Entities.Restriction Transfer()
        {
            return new Entities.PeriodRestriction()
            {
                Code = (Code?.IsEmpty() ?? true) ? Guid.NewGuid() : Code.Value,
                RoleCode = Role?.Code ?? RoleCode ?? Guid.Empty,
                Periods = Periods?.Select(p => p.Transfer()).ToList()
            };
        }

        public override void IsValid()
        {
            base.IsValid();
            Periods?.ForEach(p => p.IsValid());
        }
    }
}

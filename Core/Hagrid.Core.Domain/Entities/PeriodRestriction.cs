using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Entities
{
    public class PeriodRestriction : Restriction
    {
        [JsonIgnore]
        public override RestrictionType Type { get => RestrictionType.Period; }

        public virtual List<AccessPeriod> Periods { get; set; }

        public override DTO.Restriction GetResult() => new DTO.PeriodRestriction(this);

        public override string ObjectSerialized
        {
            get => this.ToJsonString();
            set
            {
                if (!value.IsNullOrWhiteSpace())
                {
                    var obj = value.JsonTo<PeriodRestriction>();
                    Periods = obj.Periods;
                }
            }
        }
    }
}

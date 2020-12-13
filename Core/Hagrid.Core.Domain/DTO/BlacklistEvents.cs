using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.DTO
{
    public class BlacklistEvents
    {
        public BlacklistEvents(VO.BlacklistEvents events)
        {
            this.Reason = events.Reason;
            this.Blocked = events.Blocked;
            this.BlockedBy = events.BlockedBy;
            this.SaveDate = events.SaveDate;
        }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("blocked_by")]
        public Guid BlockedBy { get; set; }

        [JsonProperty("save_date")]
        public DateTime SaveDate { get; set; }

        [JsonProperty("blocked")]
        public bool Blocked { get; set; }

    }
}

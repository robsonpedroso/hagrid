using Newtonsoft.Json;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.DTO
{
    public class Blacklist
    {
        public Blacklist(DO.Blacklist blacklist)
        {
            if (!blacklist.Store.IsNull())
                this.StoreName = blacklist.Store.Name;
            else
                this.StoreName = "Todas as lojas";

            this.Blocked = blacklist.Blocked;
            this.UpdateDate = blacklist.UpdateDate;
            this.AccountCode = blacklist.AccountCode;
            this.StoreCode = blacklist.StoreCode;

            if (blacklist.Events.Count > 0)
                this.Events = blacklist.Events.Select(x => new BlacklistEvents(x)).OrderByDescending(x => x.SaveDate).ToList();
        }

        public Blacklist()
        {

        }

        [JsonProperty("account_code")]
        public Guid AccountCode { get; set; }

        [JsonProperty("blocked_by")]
        public Guid? BlockedBy { get; set; }

        [JsonProperty("store_name")]
        public string StoreName { get; set; }

        [JsonProperty("store_code")]
        public Guid? StoreCode { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("update_date")]
        public DateTime UpdateDate { get; set; }

        [JsonProperty("blocked")]
        public bool Blocked { get; set; }

        [JsonProperty("events")]
        public List<BlacklistEvents> Events { get; set; }
    }
}

using Newtonsoft.Json;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Entities
{
    public class Blacklist : IEntity
    {
        public Blacklist(DTO.Blacklist blacklist)
            :this()
        {
            this.AccountCode = blacklist.AccountCode;
            this.StoreCode = blacklist.StoreCode;
            this.Events = new List<BlacklistEvents>()
            {
                new BlacklistEvents()
                {
                    BlockedBy = blacklist.BlockedBy.Value,
                    Reason = blacklist.Reason,
                    SaveDate = DateTime.Now
                }
            };
        }

        public Blacklist()
        {
            this.Code = Guid.NewGuid();
            this.SaveDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Events = new List<BlacklistEvents>();
        }

        [JsonIgnore]
        public Guid Code { get; set; }

        [JsonIgnore]
        public Account Account { get; set; }

        [JsonIgnore]
        public Guid AccountCode { get; set; }

        [JsonIgnore]
        public Store Store { get; set; }

        [JsonIgnore]
        public Guid? StoreCode { get; set; }

        [JsonIgnore]
        public DateTime SaveDate { get; set; }

        [JsonIgnore]
        public DateTime UpdateDate { get; set; }

        [JsonIgnore]
        public bool Blocked { get; set; }

        [NotMapped]
        public List<BlacklistEvents> Events { get; set; }

        [JsonIgnore]
        public string Object
        {
            get
            {
                return JsonConvert.SerializeObject(this.Events);
            }
            set
            {
                var objs = string.IsNullOrEmpty(value)
                        ? new List<BlacklistEvents>()
                        : JsonConvert.DeserializeObject<List<BlacklistEvents>>(value);

                this.Events.AddRange(objs);

            }
        }
    }
}

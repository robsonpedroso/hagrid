using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using DO = Hagrid.Core.Domain.Entities;
namespace Hagrid.Core.Domain.DTO.SaveAllSystem
{
    public class Store
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("cnpj")]
        public string CNPJ { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("account")]
        public Account Account { get; set; }

        [JsonProperty("marketplace_integration")]
        public MarketPlace MarketPlace { get; set; }

        [JsonProperty("applications")]
        public List<Application> Applications { get; set; }

        public Store() { }
    }
}

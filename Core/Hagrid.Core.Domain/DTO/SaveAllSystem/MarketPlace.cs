using Newtonsoft.Json;
using System;

namespace Hagrid.Core.Domain.DTO.SaveAllSystem
{
    public class MarketPlace
    {
        [JsonProperty("account")]
        public Account Account { get; set; }

        [JsonProperty("integration_key")]
        public Guid IntegrationKey { get; set; }
    }
}

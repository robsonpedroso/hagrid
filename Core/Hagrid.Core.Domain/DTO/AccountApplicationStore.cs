using Newtonsoft.Json;
using System;
using System.Linq;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.DTO
{
    public class AccountApplicationStore
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("store_code")]
        public Guid StoreCode { get; set; }

        [JsonProperty("store_name")]
        public string StoreName { get; set; }

        [JsonProperty("save_date")]
        public DateTime? SaveDate { get; set; }

        [JsonProperty("update_date")]
        public DateTime UpdateDate { get; set; }

        public AccountApplicationStore() { }

        public AccountApplicationStore(DO.ApplicationStore app)
        {
            Name = app.Application.Name;
            StoreCode = app.Store.Code;
            StoreName = app.Store.Name;
            SaveDate = app.AccountApplicationStoreCollection.Select(s => s.SaveDate).FirstOrDefault();
            UpdateDate = app.AccountApplicationStoreCollection.Select(s => s.UpdateDate).FirstOrDefault();
        }
    }
}

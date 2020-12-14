using Newtonsoft.Json;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.DTO
{
    public class Store
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("is_main")]
        public bool? IsMain { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("applications")]
        public List<DTO.ApplicationStore> Applications { get; set; }

        [JsonProperty("addresses")]
        public List<DTO.StoreAddress> Addresses { get; set; }

        [JsonProperty("metadata")]
        public object Metadata { get; set; }

        [JsonProperty("metadata_fields")]
        public object MetadataFields { get; set; }

        public Store() { }

        public Store(DO.Store store, bool withMetadataFields = false)
        {
            this.Code = store.Code;
            this.Name = store.Name;
            this.Cnpj = store.Cnpj;
            this.IsMain = store.IsMain;
            this.Logo = store.GetLogoURL();

            if (!store.Metadata.IsNull())
            {
                var metadata = new Dictionary<string, object>();

                store.Metadata.ForEach(m => metadata.Add(m.Field.JsonId, m.Value));

                this.Metadata = metadata;

                if (withMetadataFields)
                    MetadataFields = store.Metadata.OrderBy(m => m.Field.SaveDate).Select(m => new DTO.MetadataField(m.Field, m)).ToList();
            }
        }

        public virtual Entities.Store Transfer()
        {
            return new DO.Store()
            {
                Code = this.Code
            };
        }
    }
}

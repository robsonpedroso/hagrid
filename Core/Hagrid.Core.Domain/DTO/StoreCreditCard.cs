using Newtonsoft.Json;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.DTO
{
    [JsonObject("credit_card")]
    public class StoreCreditCard
    {
        [JsonProperty("store_code")]
        public virtual string StoreCode { get; set; }

        [JsonProperty("store_name")]
        public virtual string StoreName { get; set; }

        [JsonProperty("cnpj")]
        public virtual string CNPJ { get; set; }

        [JsonProperty("number")]
        public virtual string Number { get; set; }

        [JsonProperty("holder")]
        public virtual string Holder { get; set; }

        [JsonProperty("expiration_month")]
        public virtual string ExpMonth { get; set; }

        [JsonProperty("expiration_year")]
        public virtual string ExpYear { get; set; }

        [JsonProperty("security_code")]
        public virtual string SecurityCode { get; set; }

        [JsonProperty("document")]
        public virtual string Document { get; set; }

        public StoreCreditCard() { }
    }
}
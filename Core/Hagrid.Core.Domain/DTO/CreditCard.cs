using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO = Hagrid.Core.Domain.Entities;

namespace Hagrid.Core.Domain.DTO
{
    public class CreditCard
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("expiration_month")]
        public string ExpirationMonth { get; set; }

        [JsonProperty("expiration_year")]
        public string ExpirationYear { get; set; }

        [JsonProperty("holder_name")]
        public string HolderName { get; set; }

        [JsonProperty("holder_document")]
        public string HolderDocument { get; set; }

        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [JsonProperty("card_bin")]
        public string Bin { get; set; }

        [JsonProperty("card_last_digits")]
        public string LastDigits { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonProperty("card_token")]
        public string Token { get; set; }

        public CreditCard() { }

        public CreditCard(DO.CreditCard creditCard)
        {
            this.Code = creditCard.Code;
            this.Brand = creditCard.Brand;
            this.ExpirationMonth = creditCard.ExpirationMonth;
            this.ExpirationYear = creditCard.ExpirationYear;
            this.HolderName = creditCard.HolderName;
            this.HolderDocument = creditCard.HolderDocument;
            this.CardNumber = creditCard.CardNumber;
            this.Bin = creditCard.Bin;
            this.LastDigits = creditCard.LastDigits;
        }
    }
}

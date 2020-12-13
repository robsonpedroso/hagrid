using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    public class Phone
    {
        [JsonProperty("code_country")]
        public string CodeCountry { get; set; }

        [JsonProperty("phone_type")]
        public PhoneType PhoneType { get; set; }

        [JsonProperty("ddd")]
        public string DDD { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        public Phone() { }

        public Phone(ValueObjects.Phone phone)
        {
            CodeCountry = phone.CodeCountry;
            DDD = phone.DDD;
            Extension = phone.Extension;
            Number = phone.Number;
            PhoneType = phone.PhoneType;
        }

        public ValueObjects.Phone Transfer()
        {
            var phone = new ValueObjects.Phone()
            {
                CodeCountry = this.CodeCountry,
                DDD = this.DDD,
                Extension = this.Extension,
                Number = this.Number,
                PhoneType = this.PhoneType
            };

            return phone;
        }
    }
}

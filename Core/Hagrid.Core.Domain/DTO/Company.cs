using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    public class Company : Customer
    {
        [JsonProperty("company_name")]
        public String CompanyName { get; set; }

        [JsonProperty("trade_name")]
        public String TradeName { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("ie")]
        public string Ie { get; set; }

        [JsonProperty("im")]
        public string Im { get; set; }

        public Company()
            : base()
        {
            Type = CustomerType.Company;
        }

        public Company(Entities.Company customer)
            : base(customer)
        {
            CompanyName = customer.CompanyName;
            TradeName = customer.TradeName;
            Cnpj = customer.Cnpj;
            Ie = customer.Ie;
            Im = customer.Im;
        }
    }
}

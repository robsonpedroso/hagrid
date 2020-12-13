using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.DTO
{
    public class Person : Customer
    {
        [JsonProperty("first_name")]
        public String FirstName { get; set; }

        [JsonProperty("last_name")]
        public String LastName { get; set; }

        [JsonProperty("cpf")]
        public string Cpf { get; set; }

        [JsonProperty("rg")]
        public string Rg { get; set; }

        [JsonProperty("birth_date")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        public Person() { }

        public Person(Entities.Person customer)
            : base(customer)
        {
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Cpf = customer.Cpf;
            Rg = customer.Rg.AsString("");
            BirthDate = customer.BirthDate;
            Gender = customer.Gender;
        }
    }
}

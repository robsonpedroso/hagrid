using Newtonsoft.Json;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    [JsonConverter(typeof(DiscriminatorConverter))]
    public class Customer
    {

        [JsonDiscriminator]
        [JsonProperty("type")]
        public CustomerType Type { get; set; }

        [JsonProperty("origin_store")]
        public Guid? OriginStore { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }

        [JsonProperty("newsletter")]
        public bool? NewsLetter { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("save_date")]
        public DateTime SaveDate { get; set; }

        [JsonProperty("update_date")]
        public DateTime UpdateDate { get; set; }

        [JsonProperty("mobile")]
        public Phone Mobile { get; private set; }

        public Customer() { }

        public Customer(Entities.Customer customer)
        {
            Type = customer.Type;
            OriginStore = customer.OriginStore;
            Name = customer.Name;
            Addresses = customer.Addresses.Select(a => new Address(a)).ToList();
            NewsLetter = customer.NewsLetter;
            Status = customer.Status;
            SaveDate = customer.SaveDate;
            UpdateDate = customer.UpdateDate;

            if (!customer.Mobile.IsNull())
                Mobile = new Phone(customer.Mobile);
        }

        public Entities.Customer Transfer(Account account = null)
        {
            Entities.Customer customer;
            if (this.Type == CustomerType.Person)
            {
                customer = new Entities.Person()
                {
                    FirstName = ((Person)this).FirstName,
                    LastName = ((Person)this).LastName,
                    Cpf = ((Person)this).Cpf,
                    Rg = ((Person)this).Rg,
                    BirthDate = ((Person)this).BirthDate,
                    Gender = ((Person)this).Gender.IsNullorEmpty() ? null : ((Person)this).Gender,
                    Name = string.Format("{0} {1}", ((Person)this).FirstName, ((Person)this).LastName)
                };

                if (!account.IsNull())
                {
                    ((Entities.Person)customer).Cpf = account.Document;
                }
            }
            else
            {
                customer = new Entities.Company()
                {
                    CompanyName = ((Company)this).CompanyName,
                    TradeName = ((Company)this).TradeName,
                    Cnpj = ((Company)this).Cnpj,
                    Ie = ((Company)this).Ie,
                    Im = ((Company)this).Im,
                    Name = ((Company)this).CompanyName
                };

                if (!account.IsNull())
                {
                    ((Entities.Company)customer).Cnpj = account.Document;
                }
            }

            customer.Type = this.Type;
            customer.Addresses = !this.Addresses.IsNull() ? this.Addresses.Select(a => a.Transfer()).ToList() : new List<AddressCustomer>();
            customer.NewsLetter = this.NewsLetter ?? false;
            customer.Status = this.Status;

            if (!account.IsNull())
            {
                customer.Email = account.Email;
            }

            if(!account.Code.IsNull())
                customer.Account = new Entities.Account() { Code = account.Code };

            return customer;
        }
    }
}

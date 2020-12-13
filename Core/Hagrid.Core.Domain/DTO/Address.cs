using Newtonsoft.Json;
using Hagrid.Infra.Utils;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.DTO
{
    public class Address
    {
        [JsonProperty("address_customer_code")]
        public Guid AddressCustomerCode { get; set; }

        [JsonProperty("type")]
        public AddressType Type { get; set; }

        [JsonProperty("purpose")]
        public AddressPurposeType Purpose { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }

        [JsonProperty("phones")]
        public List<Phone> Phones { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contact_name")]
        public String ContactName { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("removed")]
        public bool Removed { get; set; }

        [JsonProperty("save_date")]
        public DateTime SaveDate { get; set; }

        [JsonProperty("update_date")]
        public DateTime UpdateDate { get; set; }

        [JsonProperty("others")]
        public List<string[]> Others { get; set; }


        public Address() { }

        public Address(AddressCustomer customer)
        {
            AddressCustomerCode = customer.AddressCustomerCode;
            City = customer.City;
            Complement = customer.Complement;
            ContactName = customer.ContactName;
            Country = customer.Country;
            District = customer.District;
            Name = customer.Name;
            Number = customer.Number;
            Others = customer.Others;
            Purpose = customer.Purpose;
            Removed = customer.Removed;
            SaveDate = customer.SaveDate;
            State = customer.State;
            Status = customer.Status;
            Street = customer.Street;
            Type = customer.Type;
            UpdateDate = customer.UpdateDate;
            ZipCode = customer.ZipCode;
            Phones = customer.Phones.Select(p => new Phone(p)).ToList();
        }

        public AddressCustomer Transfer()
        {
            var addressCustomer = new AddressCustomer()
            {
                AddressCustomerCode = this.AddressCustomerCode,
                AddressGlobalIdCode = 0,
                City = this.City,
                Complement = this.Complement,
                ContactName = this.ContactName,
                Country = this.Country,
                District = this.District,
                ExternalType = ExternalType.AddressExternal,
                Name = this.Name,
                Number = this.Number,
                Others = this.Others,
                Purpose = this.Purpose,
                Removed = this.Removed,
                State = this.State,
                Status = this.Status,
                Street = this.Street,
                Type = this.Type,
                ZipCode = this.ZipCode,
                Phones = this.Phones.Select(p => p.Transfer()).ToList()
            };

            return addressCustomer;
        }
    }
}

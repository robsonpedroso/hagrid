using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using DO = Hagrid.Core.Domain.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.DTO
{
    public class StoreAddress
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("address_identifier")]
        public string AddressIdentifier { get; set; }

        [JsonProperty("contact_name")]
        public string ContactName { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("main_phone_number")]
        public string Phone { get; set; }

        [JsonProperty("phone_ddd")]
        public String PhoneDdd1 { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber1 { get; set; }

        [JsonProperty("phone_ddd_2")]
        public String PhoneDdd2 { get; set; }

        [JsonProperty("phone_number_2")]
        public string PhoneNumber2 { get; set; }

        [JsonProperty("phone_ddd_3")]
        public String PhoneDdd3 { get; set; }

        [JsonProperty("phone_number_3")]
        public string PhoneNumber3 { get; set; }

        public StoreAddress() { }

        public StoreAddress(DO.StoreAddress address)
        {
            this.Code = address.Code;
            this.AddressIdentifier = address.AddressIdentifier;
            this.ContactName = address.ContactName;
            this.ZipCode = address.ZipCode;
            this.Street = address.Street;
            this.Number = address.Number;
            this.Complement = address.Complement;
            this.District = address.District;
            this.City = address.City;
            this.State = address.State;
            this.PhoneNumber1 = address.PhoneNumber1;
            this.PhoneNumber2 = address.PhoneNumber2;
            this.PhoneNumber3 = address.PhoneNumber3;
        }

        public void Validate()
        {
            List<String> validationErros = new List<string>();

            if (string.IsNullOrWhiteSpace(this.Street))
                validationErros.Add("Informe o logradouro");

            if (string.IsNullOrWhiteSpace(this.Number))
                validationErros.Add("Informe o número");

            if (string.IsNullOrWhiteSpace(this.District))
                validationErros.Add("Informe o bairro");

            if (string.IsNullOrWhiteSpace(this.City))
                validationErros.Add("Informe a cidade");

            if (string.IsNullOrWhiteSpace(this.State))
                validationErros.Add("Estado não pode ser vazio");

            if (string.IsNullOrWhiteSpace(this.ZipCode))
                validationErros.Add("Informe o CEP");
            else
            {
                if (string.IsNullOrWhiteSpace(this.ZipCode.Replace("-", "")))
                    validationErros.Add("Informe o CEP");

                if (!this.ZipCode.IsValidCEP(false))
                    validationErros.Add("Informe o CEP corretamente");
            }

            if (this.State.IsNullorEmpty() || this.State.Length > 2)
                validationErros.Add("Informe o Estado corretamente");

            var phoneNumber = !this.PhoneNumber1.IsNullOrWhiteSpace() ? this.PhoneNumber1 : this.Phone;

            if (string.IsNullOrWhiteSpace(this.Phone) || string.IsNullOrWhiteSpace(this.PhoneNumber1))
            {
                if (phoneNumber.IsNullOrWhiteSpace())
                    validationErros.Add("Informe o número do telefone");

                if (!string.IsNullOrWhiteSpace(this.PhoneNumber1))
                {
                    if (string.IsNullOrWhiteSpace(this.PhoneDdd1))
                        validationErros.Add("Informe o ddd");

                    if (this.PhoneNumber1.Trim().Length < 8)
                        validationErros.Add("Número de telefone inválido");

                    if (this.PhoneDdd1.Trim().Length < 2)
                        validationErros.Add("DDD do telefone inválido");
                }
            }

            var errorMessage = string.Join(" | ", validationErros);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }
        }
    }
}

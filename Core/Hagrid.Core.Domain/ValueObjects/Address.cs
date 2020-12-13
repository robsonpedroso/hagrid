using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hagrid.Core.Domain.ValueObjects
{
    [XmlInclude(typeof(AddressCustomer))]
    public class Address : IEntity<Decimal>, IStatus
    {
        public AddressType Type { get; set; }

        [XmlAttribute]
        public AddressPurposeType Purpose { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public ExternalType ExternalType { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public int AccountCode { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string Complement { get; set; }

        public List<string[]> Phone { get; set; }

        public List<Phone> Phones { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public String PhonesData { get; set; }

        #region Orders

        /// <summary>
        /// Shopping
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Shopping
        /// </summary>
        public String ContactName { get; set; }

        #endregion

        #region "  IEntityDecimal Members  "

        public decimal Code { get; set; }

        #endregion

        #region "  ICommonFields Members  "

        public bool Status { get; set; }

        public bool Removed { get; set; }

        public DateTime SaveDate { get; set; }

        public DateTime UpdateDate { get; set; }

        #endregion

        #region "  IExtraFields Members  "

        public List<string[]> Others { get; set; }

        #endregion

        #region "  Contructor  "

        public Address()
        {
            Code = default(int);
            Others = new List<string[]>();
            Phone = new List<string[]>();
            Phones = new List<Phone>();
            Purpose = AddressPurposeType.Contact;
            Type = AddressType.HomeAddress;
            SaveDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }

        #endregion

        #region "  ToString  "

        public override string ToString()
        {
            return Code.ToString();
        }

        #endregion

        public void RemoveSpecialCharacter()
        {
            for (int j = 0; j < Phones.Count; j++)
            {
                Phones[j].Number = Phones[j].Number.Replace("-", "").Trim();
                Phones[j].DDD = Phones[j].DDD.Replace("-", "").Trim();
                Phones[j].CodeCountry = !Phones[j].CodeCountry.IsNullorEmpty() ? Phones[j].CodeCountry.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : string.Empty;
                Phones[j].Extension = !Phones[j].Extension.IsNull() ? Phones[j].Extension.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "") : null;
            }
        }

        public void IsValid()
        {
            if (Purpose == AddressPurposeType.Shipping)
            {
                if (Name.IsNullorEmpty())
                    throw new ArgumentException("Nome inválido");

                if (ContactName.IsNullorEmpty())
                    throw new ArgumentException("Nome do destinatário inválido");
            }

            if (Type == AddressType.None)
                Type = AddressType.HomeAddress;

            if (ZipCode.IsNullorEmpty() || ZipCode.ClearStrings().Length != 8)
                throw new ArgumentException("CEP inválido");

            if (Street.IsNullOrWhiteSpace())
                throw new ArgumentException("Logradouro inválido");

            if (Number.IsNullorEmpty())
                throw new ArgumentException("Número inválido");

            if (District.IsNullorEmpty())
                throw new ArgumentException("Bairro inválido");

            if (City.IsNullorEmpty())
                throw new ArgumentException("Cidade inválida");

            List<string> states = new List<string>() { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
            if (!states.Contains(State))
                throw new ArgumentException("Estado inválido");

            if (Phones.Count == 0)
                throw new ArgumentException("Telefone inválido. Informe ao menos 1");

            var _phone = "";

            Phones.ForEach(phone =>
            {
                phone.DDD.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "").Trim();

                if (!phone.DDD.IsNullorEmpty() && phone.DDD.TakeSpecialCharactersOff().Length != 2)
                {
                    if (phone.DDD.TakeSpecialCharactersOff().Length == 3)
                        phone.DDD = phone.DDD.Substring(1);
                    else if (phone.DDD.TakeSpecialCharactersOff().Length == 4)
                        phone.DDD = phone.DDD.Substring(2);
                    else
                        throw new ArgumentException("DDD inválido");
                }

                _phone = phone.Number.Replace("\0", "").Replace("&#x0", "").Replace("\\0", "").Trim();

                if (phone.PhoneType != PhoneType.Celular)
                {
                    if (!_phone.IsNullorEmpty() && _phone.ClearStrings().Length != 8 && _phone.ClearStrings().Length != 9)
                        throw new ArgumentException("Número de telefone inválido");
                }
                else
                {
                    if (!_phone.IsNullorEmpty() && _phone.ClearStrings().Length != 8 && _phone.ClearStrings().Length != 9)
                        throw new ArgumentException("Número de celular inválido");
                }
            });
        }
    }

}

using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ModelValidation;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Hagrid.Core.Domain.Entities
{
    [XmlInclude(typeof(Person))]
    [KnownType(typeof(Person))]

    [XmlInclude(typeof(Company))]
    [KnownType(typeof(Company))]
    [Audit]
    public class Customer : Member
    {
        #region " Fields  "

        [JsonIgnore]
        public Account Account { get; set; }

        public Guid Guid { get { return Account.Code; } }

        public Guid Code { get; set; }

        public string Name { get; set; }

        [Audit(Type = Enums.AuditLogsType.Email)]
        public string Email { get; set; }

        public string Password { get; set; }

        public bool NewsLetter { get; set; }

        public CustomerType Type { get; set; }

        public string AddressData { get; set; }

        public List<AddressCustomer> Addresses { get; set; }

        public Guid? OriginStore { get; set; }

        public bool Status { get; set; }

        public bool Removed { get; set; }

        public DateTime SaveDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public Phone Mobile
        {
            get
            {
                var address = this.Addresses.FirstOrDefault(x => x.Purpose == AddressPurposeType.Contact);

                if (address.IsNull())
                    return null;

                Phone phone = address.Phones.FirstOrDefault(x => x.PhoneType == PhoneType.Celular);
                string number = string.Empty;
                string ddd = string.Empty;

                if (phone.IsNull())
                {
                    phone = address.Phones.FirstOrDefault(x => x.PhoneType == PhoneType.Residencial);

                    if (!phone.IsNull() && phone.Number.Replace("-", string.Empty).Length == 9 && phone.Number.StartsWith("9"))
                    {
                        number = phone.Number.Replace("-", string.Empty);
                        ddd = phone.DDD;
                    }
                }
                else
                {
                    ddd = phone.DDD;
                    number = phone.Number.Replace("-", string.Empty);

                    if (number.Length == 8)
                        number = string.Concat("9", number);
                }

                if (number.IsNullorEmpty())
                    return null;

                return new Phone()
                {
                    DDD = ddd,
                    Number = number,
                    PhoneType = PhoneType.Celular
                };
            }
        }

        #endregion

        #region "  Contructor  "

        public Customer()
        {
            SaveDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            Addresses = new List<AddressCustomer>();
            Status = true;
        }

        public Customer(Customer customer, Account account = null, Guid? originStore = null)
        {
            if (!account.IsNull())
            {
                if(customer is Person)
                    customer.Name = string.Format("{0} {1}", ((Entities.Person)customer).FirstName, ((Entities.Person)customer).LastName);
                else
                    customer.Name = ((Entities.Company)customer).CompanyName;
            }
        }

        public Customer(CustomerImport customerImport)
            : this()
        {
            this.Email = customerImport.Email;
            this.Status = customerImport.Status;
            this.SaveDate = customerImport.SaveDate;
            this.UpdateDate = customerImport.UpdateDate;

            this.Addresses.AddRange(customerImport.Address);
        }

        #endregion

        #region  "  Methods  "

        public void HandlerToGet()
        {
            if (this != null)
            {
                if (!Account.IsNull())
                {
                    this.Email = Account.Email;
                    this.Password = Account.Password;
                }

                this.Addresses = this.AddressData.Deserialize<List<AddressCustomer>>();

                if (this is Person)
                {
                    this.Name = string.Format("{0} {1}", ((Person)this).FirstName, ((Person)this).LastName);
                }
                else
                {
                    this.Name = ((Company)this).CompanyName;
                }
            }
        }

        public void HandlerToSave()
        {
            OriginStore = null;

            if (!Code.IsEmpty())
            {
                UpdateDate = DateTime.Now;
            }
            else
            {
                SaveDate = DateTime.Now;
                UpdateDate = SaveDate;
            }
        }

        public void Transfer(Customer newCustomer)
        {
            Email = newCustomer.Email;

            if (newCustomer is Person)
            {
                Name = string.Format("{0} {1}", ((Person)newCustomer).FirstName, ((Person)newCustomer).LastName);
            }
            else
            {
                Name = ((Company)newCustomer).CompanyName;
            }
        }

        #endregion

        public virtual void IsValid(Entities.Store currentStore = null)
        {
            if (!Email.IsValidEmail())
                throw new ArgumentException("E-mail inválido");

            if (this is Person)
            {
                ((Person)this).IsValid();
            }
            else if (this is Company)
            {
                ((Company)this).IsValid();
            }


            List<AddressCustomer> addresses = new List<AddressCustomer>();

            if (!currentStore.IsNull() && currentStore.IsMain)
            {
                addresses = Addresses;
            }
            else
            {
                var _contactAddress = Addresses.FirstOrDefault(a => a.Purpose == AddressPurposeType.Contact);

                if (!_contactAddress.IsNull()) 
                    addresses.Add(_contactAddress);
            }

            if (addresses.IsNull() || addresses.Count() == 0)
            {
                throw new ArgumentException("Endereço inválido. Informe o endereço de contato");
            }
            else
            {
                addresses.ForEach(address => address.IsValid());
            }
        }

        public virtual void Trim()
        {
            Name = Name.IsNullOrWhiteSpace() ? Name : Name.Trim();
            Email = Email.IsNullOrWhiteSpace() ? Email : Email.Trim();
        }
    }
}

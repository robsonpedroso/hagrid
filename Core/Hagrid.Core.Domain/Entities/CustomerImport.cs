using Hagrid.Core.Domain.Contracts.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Entities
{
    public class CustomerImport : IResetPasswordTokenOwner
    {
        public Guid AccountCode { get; set; }

        public string DisplayName { get; set; }

        public string DisplayDocument { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool NewsLetter { get; set; }

        public CustomerType Type { get; set; }

        public LocalLogin LocalLogin { get; set; }

        public List<AddressCustomer> Address { get; set; }

        public Guid? StoreCode { get; set; }

        public string StoreName { get; set; }

        public string IP { get; set; }

        public bool Status { get; set; }

        public bool Removed { get; set; }

        public DateTime SaveDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string AddressData
        {
            get
            {
                return Address.Serialize<List<AddressCustomer>>();
            }
            set
            {
                if (value.IsNullOrWhiteSpace())
                    return;

                Address = value.Deserialize<List<AddressCustomer>>();
            }

        }

        public int? QtyWrongsPassword { get; set; }

        public DateTime? LockedUp { get; set; }

        public void HandleCustomer()
        {
            if (this != null)
            {
                this.Address = this.AddressData.Deserialize<List<AddressCustomer>>();

                if (this is PersonImport)
                {
                    this.DisplayName = string.Format("{0} {1}", ((PersonImport)this).FirstName, ((PersonImport)this).LastName);
                    this.DisplayDocument = ((PersonImport)this).CPF;
                }
                else
                {
                    this.DisplayName = ((CompanyImport)this).CompanyName;
                    this.DisplayDocument = ((CompanyImport)this).CNPJ;
                }
            }
        }

        public Tuple<bool, List<string>> isValid()
        {
            bool result = true;
            List<string> messages = new List<string>();

            if (!Email.IsValidEmail())
            {
                result = false;
                messages.Add("E-mail inválido");
            }

            if (this is PersonImport)
            {
                var validPerson = ((PersonImport)this).IsValid();
                if (!validPerson.Item1)
                {
                    result = false;
                    messages.AddRange(validPerson.Item2);
                }
            }
            else if (this is CompanyImport)
            {
                var validCompany = ((CompanyImport)this).IsValid();
                if (!validCompany.Item1)
                {
                    result = false;
                    messages.AddRange(validCompany.Item2);
                }
            }

            if (this.Address.IsNull() || this.Address.Count() == 0)
            {
                result = false;
                messages.Add("Endereço inválido. Informe o endereço de contato");
            }
            else
            {
                this.Address.ForEach(a =>
                {
                    try
                    {
                        a.IsValid();
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        messages.Add(ex.Message);
                    }
                });
            }

            return new Tuple<bool, List<string>>(result, messages);
        }
    }
}

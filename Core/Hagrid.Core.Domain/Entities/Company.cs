using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ModelValidation;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Domain.Entities
{
    [Audit]
    public class Company : Customer
    {
        public String CompanyName { get; set; }

        public String TradeName { get; set; }

        [Audit(Type = AuditLogsType.Document)]
        public string Cnpj { get; set; }

        public string Ie { get; set; }

        public string Im { get; set; }

        public Company()
            : base()
        {
            Type = CustomerType.Company;
        }

        public override void Trim()
        {
            CompanyName = CompanyName.IsNullOrWhiteSpace() ? CompanyName : CompanyName.Trim();
            TradeName = TradeName.IsNullOrWhiteSpace() ? TradeName : TradeName.Trim();
            Cnpj = Cnpj.IsNullOrWhiteSpace() ? Cnpj : Cnpj.Trim();
            Ie = Ie.IsNullOrWhiteSpace() ? Ie : Ie.Trim();
            Im = Im.IsNullOrWhiteSpace() ? Im : Im.Trim();

            base.Trim();
        }

        public void Transfer(Company newCompany)
        {
            Cnpj = newCompany.Cnpj;
            CompanyName = newCompany.CompanyName;
            TradeName = newCompany.TradeName;
            Ie = newCompany.Ie;
            Im = newCompany.Im;

            base.Transfer(newCompany);
        }

        public Company(Person person)
        {
            Account = person.Account;
            Code = person.Code;
            Name = person.Name;
            Email = person.Email;
            Password = person.Password;
            Type = CustomerType.Company;
            AddressData = person.AddressData;
            Addresses = person.Addresses;
            OriginStore = person.OriginStore;
            Status = person.Status;
            Removed = person.Removed;
            SaveDate = person.SaveDate;
            UpdateDate = person.UpdateDate;
        }

        public Company(CustomerImport customerImport)
            : base(customerImport)
        {
            var company = customerImport as CompanyImport;

            this.CompanyName = company.CompanyName;
            this.TradeName = company.TradeName;
            this.Cnpj = company.CNPJ;
            this.Ie = company.IE;
            this.Im = company.IM;
            this.Type = CustomerType.Company;
        }

        public void IsValid()
        {
            if (this.Cnpj.IsValidCNPJ())
                this.Cnpj = this.Cnpj.ClearStrings();
            else
                throw new ArgumentException("CNPJ Inválido");

            if (this.CompanyName.IsNullorEmpty())
                throw new ArgumentException("Razão social inválida");

            if (this.TradeName.IsNullorEmpty())
                throw new ArgumentException("Nome fantasia inválido");
        }
    }
}

using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ModelValidation;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Domain.Entities
{
    [Audit]
    public class Person : Customer
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }

        [Audit(Type = AuditLogsType.Document)]
        public string Cpf { get; set; }

        public string Rg { get; set; }

        [Audit(Type = AuditLogsType.Birthdate)]
        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public Person()
            : base()
        {
            BirthDate = null;
            Type = CustomerType.Person;
        }

        public Person(Person person, Account account = null, Guid? originStore = null)
            : base(person, account, originStore)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            Cpf = person.Cpf;
            Rg = person.Rg;
            BirthDate = person.BirthDate;
            Gender = person.Gender;

            if (!account.IsNull())
            {
                person.Cpf = account.Document;
            }
        }

        public Person(CustomerImport customerImport)
            : base(customerImport)
        {
            var person = customerImport as PersonImport;

            this.FirstName = person.FirstName;
            this.LastName = person.LastName;
            this.Cpf = person.CPF;
            this.Rg = person.RG;
            this.BirthDate = person.BirthDate;
            this.Gender = person.Gender;
            this.Type = CustomerType.Person;
        }

        public override void Trim()
        {
            FirstName = FirstName.IsNullOrWhiteSpace() ? FirstName : FirstName.Trim();
            LastName = LastName.IsNullOrWhiteSpace() ? LastName : LastName.Trim();
            Cpf = Cpf.IsNullOrWhiteSpace() ? Cpf : Cpf.Trim();
            Rg = Rg.IsNullOrWhiteSpace() ? Rg : Rg.Trim();
            Gender = Gender.IsNullOrWhiteSpace() ? null : Gender.Trim();

            base.Trim();
        }

        public void Transfer(Person newPerson)
        {
            BirthDate = newPerson.BirthDate;
            Cpf = newPerson.Cpf;
            FirstName = newPerson.FirstName;
            LastName = newPerson.LastName;
            Rg = newPerson.Rg;
            Gender = newPerson.Gender;

            base.Transfer(newPerson);
        }

        public void IsValid()
        {
            if (this.FirstName.IsNullorEmpty())
                throw new ArgumentException("Nome inválido");

            if (this.LastName.IsNullorEmpty())
                throw new ArgumentException("Sobrenome inválido");

            if (!this.Gender.IsNullorEmpty() && this.Gender != "M" && this.Gender != "F")
                throw new ArgumentException("Sexo inválido. Preencha com \"M\" para masculino ou \"F\" para feminino");

            if (this.BirthDate.IsNull() || this.BirthDate.Value.Year < 1900)
                throw new ArgumentException("Data de nascimento inválida");

            if (this.BirthDate >= DateTime.Today)
                throw new ArgumentException("A data de nascimento não pode ser maior que a data atual");

            if (this.Cpf.IsValidCPF())
                this.Cpf = this.Cpf.ClearStrings();
            else
                throw new ArgumentException("CPF Inválido");
        }
    }
}

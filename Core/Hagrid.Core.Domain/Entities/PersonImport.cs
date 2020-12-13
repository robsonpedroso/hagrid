using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.Entities
{
    public class PersonImport : CustomerImport
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public string CPF { get; set; }

        public string RG { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Gender { get; set; }

        public PersonImport()
            : base()
        {
            AccountCode = Guid.NewGuid();
            BirthDate = null;
            Type = CustomerType.Person;
        }

        public Tuple<bool, List<string>> IsValid()
        {
            bool result = true;
            List<string> messages = new List<string>();

            if (this.FirstName.IsNullorEmpty())
            {
                result = false;
                messages.Add("Nome inválido");
            }

            if (this.LastName.IsNullorEmpty())
            {
                result = false;
                messages.Add("Sobrenome inválido");
            }

            if (!this.Gender.IsNullorEmpty() && this.Gender != "M" && this.Gender != "F")
            {
                result = false;
                messages.Add("Sexo inválido. Preencha com \"M\" para masculino ou \"F\" para feminino");
            }

            if (this.BirthDate.IsNull() || this.BirthDate.Value.Year < 1900)
            {
                result = false;
                messages.Add("Data de nascimento inválida");
            }

            if (this.BirthDate >= DateTime.Today)
            {
                result = false;
                messages.Add("A data de nascimento não pode ser maior que a data atual");
            }

            if (this.CPF.IsValidCPF())
                this.CPF = this.CPF.ClearStrings();
            else
            {
                result = false;
                messages.Add("CPF inválido");
            }

            return new Tuple<bool, List<string>>(result, messages);
        }
    }
}

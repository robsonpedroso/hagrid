using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.Entities
{
    public class CompanyImport : CustomerImport
    {
        public String CompanyName { get; set; }

        public String TradeName { get; set; }

        public string CNPJ { get; set; }

        public string IE { get; set; }

        public string IM { get; set; }

        public CompanyImport()
            : base()
        {
            AccountCode = Guid.NewGuid();
            Type = CustomerType.Company;
        }

        public Tuple<bool, List<string>> IsValid()
        {
            bool result = true;
            List<string> messages = new List<string>();

            if (this.CNPJ.IsValidCNPJ())
                this.CNPJ = this.CNPJ.ClearStrings();
            else
            {
                result = false;
                messages.Add("CNPJ Inválido");
            }

            if (this.CompanyName.IsNullorEmpty())
            {
                result = false;
                messages.Add("Razão social inválida");
            }

            if (this.TradeName.IsNullorEmpty())
            {
                result = false;
                messages.Add("Nome fantasia inválido");
            }

            return new Tuple<bool, List<string>>(result, messages);
        }
    }
}

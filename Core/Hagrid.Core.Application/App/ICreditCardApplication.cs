using Hagrid.Core.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Application.Contracts
{
    public interface ICreditCardApplication
    {
        string Add(Guid accountCode, CreditCard creditCard);

        IEnumerable<CreditCard> Get(Guid accountCode);

        void Remove(Guid accountCode, string cardCode);
    }
}

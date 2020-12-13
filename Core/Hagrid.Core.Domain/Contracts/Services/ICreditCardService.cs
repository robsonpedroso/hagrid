using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface ICreditCardService
    {
        string Add(Account account, CreditCard creditCard);

        IEnumerable<CreditCard> Get(Account account);

        void Remove(Account account, string cardCode);
    }
}

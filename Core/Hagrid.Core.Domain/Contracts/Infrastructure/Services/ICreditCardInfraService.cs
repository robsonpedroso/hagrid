using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Services
{
    public interface ICreditCardInfraService
    {
        CreditCard Add(string document, CreditCard creditCard);

        List<CreditCard> Get(string document);

        void Remove(string document, string cardCode);
    }
}

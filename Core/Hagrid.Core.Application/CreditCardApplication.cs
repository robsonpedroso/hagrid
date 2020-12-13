using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.DTO;
using DO = Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Autofac;

namespace Hagrid.Core.Application
{
    public class CreditCardApplication : AccountBaseApplication, ICreditCardApplication
    {
        private ICreditCardService creditCardService;
        private IAccountRepository accountRepository;

        public CreditCardApplication(IComponentContext context, ICreditCardService creditCardService, IAccountRepository accountRepository)
            : base(context)
        {
            this.creditCardService = creditCardService;
            this.accountRepository = accountRepository;
        }

        public string Add(Guid accountCode, CreditCard creditCard)
        {
            var account = accountRepository.Get(accountCode);

            return creditCardService.Add(account, new DO.CreditCard(creditCard));
        }

        public IEnumerable<CreditCard> Get(Guid accountCode)
        {
            var account = accountRepository.Get(accountCode);

            var cards = creditCardService.Get(account);

            return cards.Select(card => new CreditCard(card));
        }

        public void Remove(Guid accountCode, string cardCode)
        {
            var account = accountRepository.Get(accountCode);

            creditCardService.Remove(account, cardCode);
        }
    }
}

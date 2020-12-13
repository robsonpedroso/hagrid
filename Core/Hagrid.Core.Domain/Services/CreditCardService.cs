using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;

namespace Hagrid.Core.Domain.Services
{
    public class CreditCardService : ICreditCardService
    {
        private Contracts.Infrastructure.Services.ICreditCardInfraService creditCardInfraService;

        public CreditCardService(Contracts.Infrastructure.Services.ICreditCardInfraService creditCardInfraService)
        {
            this.creditCardInfraService = creditCardInfraService;
        }

        public string Add(Account account, CreditCard creditCard)
        {
            if (account.IsNull() || account.Code.IsEmpty())
                throw new ArgumentException("Conta inválida");

            if (account.Document.IsNullOrWhiteSpace())
                throw new ArgumentException("Documento não informado");

            if (!account.Document.IsValidCPF() && !account.Document.IsValidCNPJ())
                throw new ArgumentException("Documento inválido");

            if(creditCard.IsValid())
            {
                creditCardInfraService.Add(account.Document, creditCard);
            }

            return creditCard.Code;
        }

        public IEnumerable<CreditCard> Get(Account account)
        {
            if (account.IsNull() || account.Code.IsEmpty())
                throw new ArgumentException("Conta inválida");

            if (account.Document.IsNullOrWhiteSpace())
                throw new ArgumentException("Documento não informado");

            if (!account.Document.IsValidCPF() && !account.Document.IsValidCNPJ())
                throw new ArgumentException("Documento inválido");

            return creditCardInfraService.Get(account.Document);
        }

        public void Remove(Account account, string cardCode)
        {
            if (account.IsNull() || account.Code.IsEmpty())
                throw new ArgumentException("Conta inválida");

            if (account.Document.IsNullOrWhiteSpace())
                throw new ArgumentException("Documento não informado");

            if (!account.Document.IsValidCPF() && !account.Document.IsValidCNPJ())
                throw new ArgumentException("Documento inválido");

            if (cardCode.IsNullOrWhiteSpace())
                throw new ArgumentException("Código não informado");

            creditCardInfraService.Remove(account.Document, cardCode);
        }
    }
}

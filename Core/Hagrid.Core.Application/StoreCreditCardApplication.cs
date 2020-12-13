using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using DTO = Hagrid.Core.Domain.DTO;
using Hagrid.Infra.Utils;
using Autofac;

namespace Hagrid.Core.Application
{
    public class StoreCreditCardApplication : AccountBaseApplication, IStoreCreditCardApplication
    {
        private IStoreCreditCardRepository creditCardRepository;

        public StoreCreditCardApplication(IComponentContext context, IStoreCreditCardRepository creditCardRepository)
            : base(context)
        {
            this.creditCardRepository = creditCardRepository;
        }

        public string GetSecretPhrase(string phrase)
        {
            return phrase.DecryptDES();
        }

        public void Save(DTO.StoreCreditCard credicard)
        {
            var _credicard = new StoreCreditCard(credicard);

            if (_credicard.IsValid())
            {
                using (var transaction = Connection.BeginTransaction())
                {
                    try
                    {
                        creditCardRepository.Save(_credicard);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}

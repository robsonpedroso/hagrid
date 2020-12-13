using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.DTO;
using Hagrid.Infra.Utils;
using System;
using System.Linq;
using DO = Hagrid.Core.Domain.Entities;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class StoreAccountApplication : AccountBaseApplication, IStoreAccountApplication
    {
        private readonly IStoreAccountRepository storeAccountRepository;
        private readonly IAccountRepository accRepository;
        private readonly IStoreRepository storeRepository;

        public StoreAccountApplication(IComponentContext context,
            IStoreAccountRepository storeAccountRepository,
            IAccountRepository accRepository,
            IStoreRepository storeRepository)
            : base(context)
        {
            this.storeAccountRepository = storeAccountRepository;
            this.storeRepository = storeRepository;
            this.accRepository = accRepository;
        }
        public void Delete(Guid storeCode, Guid accountCode)
        {
            var account = accRepository.Get(accountCode,includeCustomer:false, includeRole:true);
            if (account.IsNull())
                throw new ArgumentException("Usuário não encontrado");

            if (!account.AccountRoles.IsNull() && account.AccountRoles.Any(x => x.Role.StoreCode == storeCode && x.Role.Status))
                throw new ArgumentException("Usuário se encontra em grupos de permissões da loja, remova-o dos grupos antes de deletar o vínculo");

            var result = storeAccountRepository.Get(storeCode, accountCode);
            if (result.IsNull())
                throw new ArgumentException("Vínculo do usuário com a loja não existe");

            using (var transaction = Connection.BeginTransaction())
                storeAccountRepository.Delete(result.Code);
        }

        public StoreAccount Get(Guid storeCode, Guid accountCode)
        {
            var account = accRepository.Get(accountCode);
            if (account.IsNull())
                throw new ArgumentException("Usuário não encontrado");

            var store = storeRepository.Get(storeCode);
            if (store.IsNull())
                throw new ArgumentException("Loja não encontrada");

            var result = storeAccountRepository.Get(storeCode, accountCode);
            if (result.IsNull())
                throw new ArgumentException("Vínculo do usuário com a loja não existe");

            return new DTO.StoreAccount(result);
        }

        public StoreAccount Save(StoreAccount storeAccount)
        {
            storeAccount.IsValid();

            DO.StoreAccount result;

            var storeAccountDO = new DO.StoreAccount(storeAccount);

            var account = accRepository.Get(storeAccountDO.Account.Code);
            if (account.IsNull())
                throw new ArgumentException("Usuário não encontrado");

            var store = storeRepository.Get(storeAccountDO.Store.Code);
            if (store.IsNull())
                throw new ArgumentException("Loja não encontrada");

            var link = storeAccountRepository.Get(storeAccountDO.Store.Code, storeAccountDO.Account.Code);
            if (!link.IsNull())
                throw new ArgumentException("O usuário já possui vínculo com a loja");

            storeAccountDO.Account = null;
            storeAccountDO.Store = null;
            storeAccountDO.AccountCode = account.Code;
            storeAccountDO.StoreCode = store.Code;

            using (var transaction = Connection.BeginTransaction())
                result = storeAccountRepository.Save(storeAccountDO);

            return new DTO.StoreAccount(result);
        }

        public SearchResult Search(SearchFilterStoreAccount filter)
        {
            if (filter.Name.IsNullOrWhiteSpace() && filter.Email.IsNullOrWhiteSpace())
                throw new ArgumentException("Necessário informar ao menos um parâmetro para o filtro");

            var result = storeAccountRepository.Search(filter);

            var storeAccounts = result.Results.Select(x => new DTO.StoreAccount(x));

            return new DTO.SearchResult(storeAccounts).SetResult(result);
        }
    }
}

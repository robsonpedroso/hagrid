using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Services
{
    public class AccountApplicationStoreService : IAccountApplicationStoreService
    {
        private readonly IAccountApplicationStoreRepository _accountApplicationStoreRepository;
        public AccountApplicationStoreService(IAccountApplicationStoreRepository accountApplicationStoreRepository)
        {
            this._accountApplicationStoreRepository = accountApplicationStoreRepository;
        }
        public void Save(AccountApplicationStore accountApplicationStore)
        {
            if(accountApplicationStore.Code.IsEmpty())
            {
                var _accountApplicationStore = _accountApplicationStoreRepository.Get(accountApplicationStore.AccountCode, accountApplicationStore.ApplicationStoreCode);

                if(!_accountApplicationStore.IsNull() && !_accountApplicationStore.Code.IsEmpty())
                {
                    _accountApplicationStore.UpdateDate = DateTime.Now;
                    _accountApplicationStoreRepository.Update(_accountApplicationStore);

                    accountApplicationStore = _accountApplicationStore;
                }
                else
                {
                    _accountApplicationStoreRepository.Save(accountApplicationStore);
                }
            }
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() {
                _accountApplicationStoreRepository
            };
        }

        #endregion
    }
}

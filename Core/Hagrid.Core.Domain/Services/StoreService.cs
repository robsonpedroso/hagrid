using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Hagrid.Infra.Utils;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Core.Domain.Services
{
    public class StoreService : IStoreService
    {
        private IStoreRepository storeRepository;
        private IApplicationRepository applicationRepository;
        private IApplicationStoreService applicationStoreService;
        private IApplicationStoreRepository applicationStoreRepository;
        private IRoleService roleService;

        public StoreService(IStoreRepository storeRepository,
            IApplicationRepository applicationRepository,
            IApplicationStoreService applicationStoreService,
            IApplicationStoreRepository applicationStoreRepository,
            IRoleService roleService)
        {
            this.storeRepository = storeRepository;
            this.applicationRepository = applicationRepository;
            this.applicationStoreService = applicationStoreService;
            this.applicationStoreRepository = applicationStoreRepository;
            this.roleService = roleService;
        }

        public Store CreateStore(Store store)
        {
            return storeRepository.Save(store);
        }

        public IEnumerable<Store> GetByCnpj(string cnpj)
        {
            if (cnpj.IsNullOrWhiteSpace())
                throw new ArgumentException("CNPJ não informado");

            if (!cnpj.IsValidCNPJ())
                throw new ArgumentException("CNPJ inválido");

            return storeRepository.GetByCnpj(cnpj);
        }

        public Store Get(Guid storeCode)
        {
            return storeRepository.Get(storeCode);

        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            var serviesRepository = new List<IRepository>();
            serviesRepository.AddRange(roleService.GetRepositories());
            serviesRepository.AddRange(applicationStoreService.GetRepositories());

            return new List<IRepository>(serviesRepository) {
                storeRepository,
                applicationRepository,
                applicationStoreRepository
            };
        }

        #endregion
    }
}

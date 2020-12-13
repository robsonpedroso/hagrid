using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using DTO = Hagrid.Core.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Hagrid.Infra.Utils;
using Autofac;

namespace Hagrid.Core.Application
{
    public class ApplicationStoreApplication : AccountBaseApplication, IApplicationStoreApplication
    {
        private readonly IApplicationStoreService applicationStoreService;
        private readonly IApplicationStoreRepository applicationStoreRepository;
        private readonly IApplicationRepository applicationRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IStoreRepository storeRepository;

        public ApplicationStoreApplication(
                IComponentContext context,
                IApplicationStoreService applicationStoreService,
                IApplicationStoreRepository applicationStoreRepository,
                IApplicationRepository applicationRepository,
                IAccountRepository accountRepository,
                IStoreRepository storeRepository)
            : base(context)
        {
            this.applicationStoreService = applicationStoreService;
            this.applicationStoreRepository = applicationStoreRepository;
            this.applicationRepository = applicationRepository;
            this.accountRepository = accountRepository;
            this.storeRepository = storeRepository;
        }

        public bool Authenticate(string clientId, string clientSecret, string reqOrigin, out string message, out ApplicationStore applicationStore)
        {
            return applicationStoreService.Authenticate(clientId, clientSecret, reqOrigin, out message, out applicationStore);
        }

        public bool Exists(string clientId)
        {
            return applicationStoreService.Exists(clientId);
        }

        public ApplicationStore Get(Guid code)
        {
            return applicationStoreRepository.Get(code);
        }

        public ApplicationStore GetByClientId(Guid clientId)
        {
            return applicationStoreRepository.GetByClientId(clientId);
        }

        public IEnumerable<DTO.AccountApplicationStore> GetByAccount(Guid accountCode, Guid clientId)
        {
            var appSto = applicationStoreRepository.GetByClientId(clientId);

            if (appSto.Application.MemberType == MemberType.Merchant)
            {
                var accRoles = accountRepository.GetAccountRoleAccess(accountCode, MemberType.Merchant);
                var listAccAppStore = new List<DTO.AccountApplicationStore>();

                accRoles.ForEach(x =>
                {
                    listAccAppStore.AddRange(x.Role.Permissions.Select(p => new DTO.AccountApplicationStore()
                    {
                        Name = p.Resource.Application.Name,
                        StoreCode = x.Role.StoreCode,
                        SaveDate = x.SaveDate,
                        UpdateDate = x.UpdateDate
                    }));
                });

                return listAccAppStore;
            }
            else
            {
                throw new ArgumentException("Aplicações de consumidores não possuem permissão para visualizar os acessos dos usuários");
            }
        }

        public DTO.ApplicationStore GetApplicationStoreByStore(Guid storeCode, string nameApplication)
        {
            DTO.ApplicationStore result = null;

            var listAppStore = applicationStoreRepository.GetByStore(storeCode);

            if (!listAppStore.IsNull())
            {
                var appStore = listAppStore.FirstOrDefault(x => x.Application.Name.ToLower() == nameApplication);
                if (!appStore.IsNull())
                {
                    result = new DTO.ApplicationStore(appStore, false);
                }
            }

            return result;
        }

        public DTO.ApplicationStore GetApplicationStoreByStoreTypeMain(string nameApplication)
        {
            DTO.ApplicationStore result = null;

            var apps = applicationRepository.List().FirstOrDefault(x => x.Name.ToLower() == nameApplication.ToLower());

            if (apps != null)
            {
                var listAppStore = applicationStoreRepository.GetByStoreTypeMain(apps.Code.Value);

                if (!listAppStore.IsNull())
                {
                    var appStore = listAppStore.FirstOrDefault(x => x.Application.Name.ToLower() == nameApplication);
                    if (!appStore.IsNull())
                    {
                        return new DTO.ApplicationStore(appStore, false);
                    }
                }
            }

            return result;
        }

        public DTO.Store GetByStore(Guid storeCode, Guid clientId)
        {
            if (storeCode.IsEmpty())
                throw new ArgumentException("Código da loja inválido!");

            Store store = null;

            var applicationStore = applicationStoreRepository.GetByClientId(clientId);

            if (!applicationStore.Store.IsMain && storeCode != applicationStore.StoreCode)
                throw new ArgumentException("Não é possível efetuar essa operação");

            store = storeRepository.Get(storeCode, includeApplicationStore: true);

            if (store.IsNull())
                throw new ArgumentException("Loja não encontrada");

            var dto = new DTO.Store()
            {
                Code = store.Code,
                Applications = store.ApplicationsStore.Select(a => new DTO.ApplicationStore(a, false)).ToList()
            };

            return dto;
        }
    }
}

using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Infrastructure.Services;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DO = Hagrid.Core.Domain.Entities;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class StoreApplication : AccountBaseApplication, IStoreApplication
    {
        private readonly IStoreService storeService;
        private readonly IStoreRepository storeRepository;
        private readonly IStoreAddressRepository storeAddressRepository;
        private readonly IApplicationRepository applicationRepository;
        private readonly IApplicationStoreService applicationStoreService;
        private readonly IApplicationStoreRepository applicationStoreRepository;
        private readonly IAccountService accountService;
        private readonly IMetadataService metadataService;
        private readonly IAccountInfraService accountInfraService;
        private readonly IRoleService roleService;

        public StoreApplication(
            IComponentContext context,
            IStoreService storeService, 
            IStoreRepository storeRepository,
            IApplicationRepository applicationRepository,
            IApplicationStoreService applicationStoreService,
            IApplicationStoreRepository applicationStoreRepository,
            IStoreAddressRepository storeAddressRepository,
            IAccountService accountService,
            IAccountInfraService accountInfraService,
            IRoleService roleService)
            : base(context)
        {
            this.storeService = storeService;
            this.storeRepository = storeRepository;
            this.applicationRepository = applicationRepository;
            this.applicationStoreService = applicationStoreService;
            this.applicationStoreRepository = applicationStoreRepository;
            this.storeAddressRepository = storeAddressRepository;
            this.accountService = accountService;

            if (context.TryResolveNamed(FieldType.Store.ToLower(), typeof(IMetadataService), out var metadataService))
                this.metadataService = metadataService as IMetadataService;

            this.accountInfraService = accountInfraService;

            this.roleService = roleService;
        }

        public IEnumerable<DTO.ApplicationStore> Save(DTO.Store store)
        {
            if (store.IsNull())
                throw new ArgumentException("Informe os dados da loja.");

            var domainStore = new Store(store);

            domainStore.IsValid();

            Store newStore = Create(domainStore);

            return newStore.ApplicationsStore.Select(a => new DTO.ApplicationStore(a));
        }

        public Store Create(DO.Store store, Guid? clientId = null, bool storeExists = false, bool onlyApplicationMain = false, bool userExists = false, bool syncInApplication = false)
        {
            Store newStore = new Store();

            if (storeExists)
            {
                var _store = storeService.Get(store.Code);

                if (!_store.IsNull() && !_store.Code.IsEmpty())
                    throw new ArgumentException("Esta loja já possui cadastro com o nome '{0}' e código {1}".ToFormat(_store.Name, _store.Code));
            }

            if (onlyApplicationMain)
            {
                if (!clientId.HasValue)
                    throw new ArgumentException("Não é possível efetuar essa operação");

                var applicationStore = applicationStoreRepository.GetByClientId(clientId.Value);

                if (!applicationStore.Store.IsMain)
                    throw new ArgumentException("Não é possível efetuar essa operação");
            }

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    newStore = storeService.CreateStore(store);
                    applicationStoreService.CreateAppStore(newStore);
                    roleService.SaveAll(newStore.Code);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return newStore;
        }

        public IEnumerable<DTO.ApplicationStore> ConnectApplications(DTO.Store store)
        {
            var applicationStoreCollection = new List<DTO.ApplicationStore>();

            var _store = storeRepository.Get(store.Code);

            if (_store.IsNull())
                throw new ArgumentException("Ops! Esta loja não possui cadastro. :(");

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    store.Applications.ForEach(application =>
                    {
                        var _application = applicationRepository.Get(application.ApplicationName);

                        if (_application.IsNull())
                            throw new ArgumentException("Ops! A aplicação '{0}' não existe. :(".ToFormat(application.ApplicationName));

                        var applicationStore = applicationStoreRepository.Get(_application.Code.Value, _store.Code);

                        if (applicationStore.IsNull())
                        {
                            applicationStoreCollection.Add(new DTO.ApplicationStore(applicationStoreService.Save(_application, _store, application.AllowedOrigins)));
                        }
                        else
                        {
                            if (!applicationStore.Status)
                            {
                                applicationStore.Status = true;
                                applicationStoreRepository.Update(applicationStore);
                            }

                            applicationStoreCollection.Add(new DTO.ApplicationStore(applicationStore));
                        }
                    });

                    transaction.Commit();
                }
                catch
                {
                    if (!transaction.IsNull())
                        transaction.Rollback();

                    throw;
                }
            }

            return applicationStoreCollection;
        }

        public IEnumerable<DTO.Store> GetByMember(Guid accountCode, Guid clientId)
        {
            return storeRepository.GetByMember(accountCode, clientId).Select(s => new DTO.Store(s));
        }

        public IEnumerable<DTO.Store> GetStore(string term)
        {
            var stores = storeRepository.Get(term);

            return stores.Select(s => new DTO.Store(s)).ToList();
        }

        public IEnumerable<DTO.Store> GetByCnpj(string cnpj)
        {
            var stores = storeRepository.GetByCnpj(cnpj);

            return stores.Select(s => new DTO.Store(s));
        }

        public DTO.Store GetStore(Guid code, Guid clientId)
        {
            if (code.IsEmpty())
                throw new ArgumentException("Código da loja inválido!");

            var applicationStore = applicationStoreRepository.GetByClientId(clientId);

            if (!applicationStore.Store.IsMain && code != applicationStore.StoreCode)
                throw new ArgumentException("Não é possível efetuar essa operação");

            var store = storeRepository.Get(code);

            if (store.IsNull())
                throw new ArgumentException("Loja não encontrada!");

            store.Metadata = metadataService.GetFieldAndFill(FieldType.Store, store.Metadata).Cast<StoreMetadata>().ToList();

            return new DTO.Store(store, true);
        }

        public IEnumerable<DTO.StoreAddress> GetStoreAddresses(Guid codeStore)
        {
            var addresses = storeAddressRepository.GetByStore(codeStore);

            return addresses.Select(s => new DTO.StoreAddress(s)).ToList();
        }

        public DTO.SearchResult Search(DTO.SearchFilter filter)
        {
            filter.Document = filter.Document.ClearStrings().AsString("").Trim();
            filter.Term = filter.Term.ClearStrings().AsString("").Trim();

            if (filter.Code.IsEmpty() && filter.Document.IsNullorEmpty() && filter.Term.IsNullorEmpty())
                throw new ArgumentException("Nenhum parametro informado para pesquisa!");

            var result = storeRepository.Search(filter);
            var stores = result.Results.Select(s => new DTO.Store(s));

            return new DTO.SearchResult(stores).SetResult<Store>(result);
        }

        public DTO.Store Update(DTO.Store store)
        {
            Store result;

            if (store.Code.IsEmpty())
                throw new ArgumentException("Ops! É necessário enviar o código da loja. :(");

            if (!store.Cnpj.IsNullOrWhiteSpace() && ((store.Cnpj.Length <= 11 && !store.Cnpj.IsValidCPF()) || (store.Cnpj.Length > 11 && !store.Cnpj.IsValidCNPJ())))
                throw new ArgumentException("Ops, CNPJ inválido");

            if (store.Name.IsNullOrWhiteSpace())
                throw new ArgumentException("Ops, Nome de loja inválido");

            using (var transaction = Connection.BeginTransaction())
            {
                try
                {
                    var updateStore = storeRepository.Get(store.Code);

                    if (updateStore.IsNull())
                        throw new ArgumentException("Ops, loja não encontrada");

                    updateStore.Name = store.Name;
                    updateStore.Cnpj = store.Cnpj;
                    updateStore.UpdateDate = DateTime.Now;

                    result = storeRepository.Save(updateStore);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return new DTO.Store(result);
        }

        public DTO.Store UploadLogo(byte[] item, Guid storeCode)
        {
            var store = storeRepository.Get(storeCode);
            store.SaveLogo(item);

            return new DTO.Store(store);
        }

        public string GetEncriptedStore(Guid code, string phrase)
        {
            if (phrase.IsNullOrWhiteSpace() || phrase == "undefined")
                throw new ArgumentException("É necessário preencher a frase secreta.");

            return string.Format("{0}/#/credit-card/{1}/{2}", Config.AccountsSiteURL, code.EncryptDES(), phrase.EncryptDES());
        }
    }
}

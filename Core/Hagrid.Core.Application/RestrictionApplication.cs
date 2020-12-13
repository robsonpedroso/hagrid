using Autofac;
using Hagrid.Core.Application.Contracts;
using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Utils;
using System;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application
{
    public class RestrictionApplication : AccountBaseApplication, IRestrictionApplication
    {
        private readonly IRestrictionRepository restrictionRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IApplicationStoreRepository applicationStoreRepository;

        public RestrictionApplication(
            IComponentContext context,
            IRestrictionRepository restrictionRepository,
            IRoleRepository roleRepository,
            IApplicationStoreRepository applicationStoreRepository)
            : base(context)
        {
            this.restrictionRepository = restrictionRepository;
            this.roleRepository = roleRepository;
            this.applicationStoreRepository = applicationStoreRepository;
        }

        public DTO.Restriction Save(bool isRKAdmin, Guid clientId, DTO.Restriction restriction)
        {
            restriction.IsValid();

            var _restriction = restriction.Transfer();

            var _role = roleRepository.Get(_restriction.RoleCode, null);

            if (_role.IsNull())
                throw new ArgumentException("Grupo não encontrado");

            if (!isRKAdmin)
            {
                var appStore = applicationStoreRepository.GetByClientId(clientId);

                if (appStore.StoreCode != _role.Store.Code)
                    throw new ArgumentException("Usuário não possui permissão para realizar essa operação");
            }

            _restriction.Role = null;
            _restriction.RoleCode = _role.Code;

            Restriction result;

            using (var transaction = Connection.BeginTransaction())
                result = restrictionRepository.Save(_restriction);

            return result.GetResult();
        }

        public DTO.SearchResult Search(bool isRKAdmin, Guid clientId, DTO.SearchFilterRestriction filter)
        {
            if (!isRKAdmin)
            {
                var appStore = applicationStoreRepository.GetByClientId(clientId);

                if (filter.StoreCode.IsEmpty())
                    filter.StoreCode = appStore.StoreCode;
                else if (appStore.StoreCode != filter.StoreCode)
                    throw new ArgumentException("Usuário não possui permissão para realizar essa operação");
            }

            var result = restrictionRepository.Search(filter);

            var restrictions = result.Results.Select(r => r.GetResult());

            return new DTO.SearchResult(restrictions).SetResult(result);
        }

        public DTO.Restriction Get(bool isRKAdmin, Guid clientId, Guid roleCode, Guid code)
        {
            var result = restrictionRepository.Get(code);

            if (!isRKAdmin)
            {
                var appStore = applicationStoreRepository.GetByClientId(clientId);

                if (result != null && appStore.StoreCode != result.Role.Store.Code)
                    throw new ArgumentException("Usuário não possui permissão para realizar essa operação");
            }

            if (result.IsNull())
                throw new ArgumentException("Restrição não encontrada");

            if (result.RoleCode != roleCode)
                throw new ArgumentException("Restrição não pertence ao grupo informado");

            return result.GetResult();
        }

        public void Delete(Guid roleCode, Guid code)
        {
            var result = restrictionRepository.Get(code);

            if (result.IsNull())
                throw new ArgumentException("Restrição não encontrada");

            if (result.RoleCode != roleCode)
                throw new ArgumentException("Restrição não pertence ao grupo informado");

            using (var transaction = Connection.BeginTransaction())
                restrictionRepository.Delete(code);
        }
    }
}

using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Providers.EntityFramework.Context;
using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class ApplicationStoreRepository : BaseRepository<ApplicationStore, CustomerContext, Guid>, IApplicationStoreRepository
    {
        public ApplicationStoreRepository(IConnection connection) : base(connection) { }

        public override ApplicationStore Get(Guid id)
        {
            return Collection.Where(x => x.Code == id)
                .Include(c => c.Store)
                .FirstOrDefault();
        }

        public ApplicationStore Get(ClientAuthType clientAuthType, Guid confClient)
        {
            switch (clientAuthType)
            {
                case ClientAuthType.JavaScript:
                    return Collection
                        .Include(c => c.Application)
                        .Include(c => c.Store)
                        .FirstOrDefault(c => c.JSClient == confClient && c.Status);
                case ClientAuthType.Confidential:
                    return Collection
                        .Include(c => c.Application)
                        .Include(c => c.Store)
                        .FirstOrDefault(c => c.ConfClient == confClient && c.Status);
                default:
                    return null;
            }

        }

        public ApplicationStore GetByClientId(Guid clientId)
        {
            return Collection
                .Include(c => c.Application)
                .Include(c => c.Store)
                .FirstOrDefault(c => (c.JSClient == clientId || c.ConfClient == clientId)
                    && c.Status);
        }

        public ApplicationStore Get(Guid applicationCode, Guid storeCode, bool includeStore = false)
        {
            if (includeStore)
            {
                return Collection
                    .Include(c => c.Store)
                    .Where(appSto => appSto.Status && appSto.Application.Code == applicationCode && appSto.Store.Code == storeCode)
                    .FirstOrDefault();
            }
            else
            {
                return Collection
                   .Where(appSto => appSto.Status && appSto.Application.Code == applicationCode && appSto.Store.Code == storeCode)
                   .FirstOrDefault();
            }
        }

        public IEnumerable<ApplicationStore> GetByStoreTypeMain(Guid applicationCode, bool includeStore = false)
        {
            if (includeStore)
            {
                return Collection
                    .Include(c => c.Store)
                    .Where(appSto => appSto.Status && appSto.Application.Code == applicationCode && appSto.Store.IsMain)
                    .ToList();
            }
            else
            {
                return Collection
                   .Where(appSto => appSto.Status && appSto.Application.Code == applicationCode && appSto.Store.IsMain)
                   .ToList();
            }
        }

        public IEnumerable<ApplicationStore> GetByStore(Guid storeCode)
        {
            return Collection
                    .Include(c => c.Application)
                    .Where(appSto => appSto.Status && appSto.Store.Code == storeCode)
                    .ToList();
        }
    }
}

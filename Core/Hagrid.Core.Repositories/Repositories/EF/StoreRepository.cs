using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;
using VO = Hagrid.Core.Domain.ValueObjects;
using System.Data.Entity;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class StoreRepository : BaseRepository<Store, CustomerContext, Guid>, IStoreRepository
    {
        public StoreRepository(IConnection connection) : base(connection) { }

        public Store Get(Guid id, bool includeApplicationStore = false)
        {
            var store = Collection.Where(s => s.Code == id)
                .Include(s => s.Metadata)
                .Include(s => s.Metadata.Select(m => m.Field));

            store = IncludesResult(
                store,
                includeApplicationStore: true
            );

            return store.FirstOrDefault();
        }

        public IEnumerable<Store> GetByCodes(List<Guid> codes)
        {
            return Collection.Where(s => codes.Contains(s.Code))
                .Include(s => s.Metadata)
                .Include(s => s.Metadata.Select(m => m.Field));
        }

        public IEnumerable<Store> Get(string name, bool onlyActive = false)
        {
            return Collection
                .Where(s => s.Name.Contains(name) && (onlyActive.Equals(false) || s.Status))
                .ToList();
        }

        public IEnumerable<Store> GetByCnpj(string cnpj)
        {
            return Collection
                .Where(s => s.Cnpj == cnpj && s.Status == true)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Store> GetByMember(Guid accountCode, Guid clientId)
        {
            var application = Context.Set<ApplicationStore>()
                .Where(a => a.ConfClient == clientId || a.JSClient == clientId)
                .Select(a => a.Application).FirstOrDefault();

            return Context
                .Set<Account>()
                .Where(a => a.Code == accountCode && a.Removed == false)
                .SelectMany(a => a.AccountRoles.Where(accRole => accRole.Status && accRole.Role.Status && accRole.Role.Permissions.Any(p => p.Resource.Application.Code == application.Code && p.Status && p.Resource.Application.Status))
                .Select(accRole => accRole.Role.Store)
                .Where(s => s.Status == true)).Distinct().ToList();
        }

        public VO.SearchResult<Store> Search(DTO.SearchFilter filter)
        {

#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);
#endif
            IQueryable<Store> result = Collection.AsQueryable();

            if (!filter.Code.IsNull() && !filter.Code.IsEmpty())
                result = result.Where(store => store.Code == filter.Code);

            if (!filter.Document.IsNullOrWhiteSpace())
                result = result.Where(store => store.Cnpj == filter.Document);

            if (!filter.Term.IsNullOrWhiteSpace())
                result = result.Where(store => store.Name.ToLower().Contains(filter.Term.ToLower()));

            result = result.OrderBy(i => i.SaveDate);

            var count = result.Count();

            result = result.Skip((filter.Skip.Value) * filter.Take.Value);

            result = result.Take(filter.Take.Value);

            return new VO.SearchResult<Store>(result.ToList(), filter, count);
        }

        private IQueryable<Store> IncludesResult(IQueryable<Store> store, bool includeApplicationStore = false)
        {
            if (includeApplicationStore)
            {
                store = store.Include(a => a.ApplicationsStore);
                store = store.Include(a => a.ApplicationsStore.Select(x => x.Application));
            }

            return store;
        }
    }
}

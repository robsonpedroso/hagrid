using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using VO = Hagrid.Core.Domain.ValueObjects;
using DTO = Hagrid.Core.Domain.DTO;
using Hagrid.Infra.Utils;
using System.Data.Entity;
using Z.EntityFramework.Plus;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class RoleRepository : BaseRepository<Role, CustomerContext, Guid>, IRoleRepository
    {
        public RoleRepository(IConnection connection) : base(connection) { }

        public override Role Save(Role role)
        {
            var original = Collection.Where(r => r.Code == role.Code).FirstOrDefault();

            if (original == null)
            {
                role.SaveDate = role.UpdateDate = DateTime.Now;
                return Collection.Add(role);
            }
            else
            {
                role.UpdateDate = DateTime.Now;
                role.SaveDate = original.SaveDate;

                Context.Entry(original).CurrentValues.SetValues(role);
                Context.Entry(original).State = EntityState.Modified;

                return Collection.Find(role.Code);
            }
        }
        public VO.SearchResult<Role> Search(DTO.SearchFilterRole filter)
        {
            IQueryable<Role> result = Collection.Include(x => x.Store).AsQueryable();

            if (!filter.StoreCode.IsEmpty())
            {
                result = result.Where(r => r.StoreCode == filter.StoreCode);
            }
            
            if (!filter.Name.IsNullOrWhiteSpace())
            {
                result = result.Where(n => n.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            result = result.OrderBy(i => i.SaveDate);

            var count = result.Count();

            result = result.Skip((filter.Skip.Value) * filter.Take.Value);

            result = result.Take(filter.Take.Value);

            return new VO.SearchResult<Role>(result.ToList(), filter, count);
        }

        public Role GetByApplication(string appName, Guid storeCode)
        {
            return Collection
                .Include(a => a.AccountRoles)
                .Include(a => a.Restrictions)
                .Where(r => r.StoreCode == storeCode && 
                                  r.Name.ToLower().Contains(VO.Config.PermissionNameRole.ToLower()) &&
                                  r.Status && 
                                  r.Permissions.Any(p => 
                                    p.Resource.Application.Name.ToLower() == appName.ToLower() &&
                                      p.Status &&
                                      p.Operations == Operations.View)
                                  ).FirstOrDefault();
        }

        public Role GetByApplication(Guid applicationCode, Guid storeCode)
        {
            return Collection
                .Include(a => a.AccountRoles)
                .Include(a => a.Restrictions)
                .Where(r =>
                    r.StoreCode == storeCode &&
                    r.Name.ToLower().Contains(VO.Config.PermissionNameRole.ToLower()) &&
                    r.Status &&
                    r.Permissions.Any(p =>
                        p.Resource.ApplicationCode == applicationCode &&
                        p.Status &&
                        p.Operations == Operations.View)
                ).FirstOrDefault();
        }

        public Role Get(Guid code, Guid? storeCode)
        {
            IQueryable<Role> result = Collection
                .IncludeFilter(x => x.Store)
                .IncludeFilter(x => x.Restrictions)
                .IncludeFilter(x => x.AccountRoles.Where(a => a.Status))
                .IncludeFilter(x => x.AccountRoles.Select(a => a.Account))
                .IncludeFilter(x => x.Permissions.Where(p => p.Status))
                .IncludeFilter(x => x.Permissions.Select(r => r.Resource))
                .IncludeFilter(x => x.Permissions.Select(r => r.Resource).Select(a => a.Application))
                .AsQueryable();

            if (!storeCode.IsEmpty())
                result = result.Where(r => r.StoreCode == storeCode);

            result = result.Where(r => r.Code == code);

            return result.FirstOrDefault();
        }

        public IEnumerable<Role> ListRoles(Guid storeCode, Guid applicationCode, Guid accountCode)
        {
            return Collection
                .IncludeFilter(x => x.AccountRoles)
                .IncludeFilter(x => x.Restrictions)
                .IncludeFilter(x => x.AccountRoles.Select(a => a.Account))
                .IncludeFilter(x => x.Permissions.Where(p => p.Status))
                .IncludeFilter(x => x.Permissions.Select(y => y.Resource))
                .IncludeFilter(x => x.Permissions.Select(r => r.Resource).Select(a => a.Application))
                .Where(r => r.StoreCode == storeCode && r.Status && r.Permissions.Any(y => y.Resource.ApplicationCode == applicationCode) && r.AccountRoles.Any(a => a.AccountCode == accountCode))
                .ToList();
        }

        public override void Delete(Guid code)
        {
            var original = Collection.Find(code);

            if (original != null)
                Context.Entry(original).State = System.Data.Entity.EntityState.Deleted;
        }
    }
}

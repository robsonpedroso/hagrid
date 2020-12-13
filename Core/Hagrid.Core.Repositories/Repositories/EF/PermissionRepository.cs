using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class PermissionRepository : BaseRepository<Permission, CustomerContext, Guid>, IPermissionRepository
    {
        public PermissionRepository(IConnection connection) : base(connection) { }

        public override Permission Save(Permission permission)
        {
            var original = Collection.Find(permission.Code);

            if (original == null)
            {
                permission.SaveDate = permission.UpdateDate = DateTime.Now;

                return Collection.Add(permission);
            }
            else
            {
                permission.UpdateDate = DateTime.Now;
                permission.SaveDate = original.SaveDate;

                Context.Entry(original).CurrentValues.SetValues(permission);
                Context.Entry(original).State = System.Data.Entity.EntityState.Modified;

                return Collection.Find(permission.Code);
            }
        }

        public VO.SearchResult<Permission> Search(DTO.SearchFilterPermission searchFilter)
        {
            IQueryable<Permission> result = Collection.
                                            Include(x => x.Role).
                                            Include(p => p.Resource.Application).
                                            Include(x => x.Resource).
                                            AsQueryable();

            if (!searchFilter.ResourceName.IsNullOrWhiteSpace())
                result = result.Where(p => p.Resource.Name.ToLower().Contains(searchFilter.ResourceName.ToLower()));

            if (!searchFilter.ResourceCode.IsNull() && !searchFilter.ResourceCode.IsEmpty())
                result = result.Where(p => p.ResourceCode == searchFilter.ResourceCode);

            if (!searchFilter.RoleCode.IsNull() && !searchFilter.RoleCode.IsEmpty())
                result = result.Where(p => p.RoleCode == searchFilter.RoleCode);

            if (!searchFilter.ApplicationCode.IsNull() && !searchFilter.ApplicationCode.IsEmpty())
                result = result.Where(p => p.Resource.ApplicationCode == searchFilter.ApplicationCode);

            result = result.OrderBy(p => p.SaveDate);

            var count = result.Count();

            result = result.Skip((searchFilter.Skip.Value) * searchFilter.Take.Value);

            result = result.Take(searchFilter.Take.Value);

            return new VO.SearchResult<Permission>(result.ToList(), searchFilter, count);
        }

        public Permission Get(Guid code, Guid? appCode)
        {
            IQueryable<Permission> result = Collection.
                                            Include(x => x.Role).
                                            Include(p => p.Resource.Application).
                                            Include(x => x.Resource).
                                            AsQueryable();

            if (appCode.HasValue)
            {
                result = result.Where(p => p.Resource.ApplicationCode == appCode);
            }
            result = result.Where(p => p.Code == code);

            return result.FirstOrDefault();
        }

        public Permission GetPermission(Guid code)
        {
            IQueryable<Permission> result = Collection.
                                            Include(x => x.Resource).
                                            AsQueryable();

            return result.Where(p => p.Code == code).FirstOrDefault();
        }

        public override void Delete(Guid code)
        {
            var original = Collection.Find(code);

            if (original != null)
                Context.Entry(original).State = System.Data.Entity.EntityState.Deleted;
        }

        public void Delete(IEnumerable<Permission> permissions)
        {
            Collection.RemoveRange(permissions);
        }
    }
}

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
    public class RestrictionRepository : BaseRepository<Restriction, CustomerContext, Guid>, IRestrictionRepository
    {
        public RestrictionRepository(IConnection connection) : base(connection) { }

        public override Restriction Save(Restriction restriction)
        {
            var original = Collection.Find(restriction.Code);

            if (original == null)
            {
                restriction.SaveDate = restriction.UpdateDate = DateTime.Now;

                return Collection.Add(restriction);
            }
            else
            {
                restriction.UpdateDate = DateTime.Now;
                restriction.SaveDate = original.SaveDate;

                Context.Entry(original).CurrentValues.SetValues(restriction);
                Context.Entry(original).State = EntityState.Modified;

                return Collection.Find(restriction.Code);
            }
        }

        public VO.SearchResult<Restriction> Search(DTO.SearchFilterRestriction filter)
        {
            var result = Collection.Include(x => x.Role).AsQueryable();

            if (!filter?.StoreCode.IsEmpty() ?? false)
                result = result.Where(x => x.Role.StoreCode == filter.StoreCode);

            if (filter?.RoleCode.HasValue ?? false)
                result = result.Where(x => x.Role.Code == filter.RoleCode);

            result = result.OrderBy(x => x.SaveDate);

            var count = result.Count();

            result = result.Skip(filter.Skip.Value * filter.Take.Value);

            result = result.Take(filter.Take.Value);

            return new VO.SearchResult<Restriction>(result.ToList(), filter, count);
        }

        public override void Delete(Guid code)
        {
            var original = Collection.Find(code);

            if (original != null)
                Context.Entry(original).State = EntityState.Deleted;
        }

        public void Delete(IEnumerable<Restriction> restrictions)
        {
            Collection.RemoveRange(restrictions);
        }
    }
}

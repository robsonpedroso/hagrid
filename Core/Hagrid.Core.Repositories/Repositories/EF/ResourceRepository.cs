using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO = Hagrid.Core.Domain.DTO;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class ResourceRepository : BaseRepository<Resource, CustomerContext, Guid>, IResourceRepository
    {
        public ResourceRepository(IConnection connection) : base(connection) { }

        public override Resource Save(Resource resource)
        {
            var original = Collection.Find(resource.Code);

            if (original == null)
            {
                resource.SaveDate = DateTime.Now;
                resource.UpdateDate = DateTime.Now;

                return Collection.Add(resource);
            }
            else
            {
                resource.UpdateDate = DateTime.Now;
                resource.SaveDate = original.SaveDate;

                Context.Entry(original).CurrentValues.SetValues(resource);
                Context.Entry(original).State = System.Data.Entity.EntityState.Modified;

                return Collection.Find(resource.Code);
            }
        }

        public VO.SearchResult<Resource> Search(DTO.SearchFilterResource filter)
        {
            IQueryable<Resource> result = Collection.Include("Application").AsQueryable();

            if (!filter.Name.IsNullOrWhiteSpace())
                result = result.Where(f => f.Name.ToLower().Contains(filter.Name.ToLower()));

            if (!filter.ApplicationCode.IsNull() && !filter.ApplicationCode.IsEmpty())
                result = result.Where(f => f.ApplicationCode == filter.ApplicationCode);

            result = result.OrderBy(i => i.SaveDate);

            var count = result.Count();

            result = result.Skip((filter.Skip.Value) * filter.Take.Value);

            result = result.Take(filter.Take.Value);

            return new VO.SearchResult<Resource>(result.ToList(), filter, count);
        }

        public override void Delete(Guid code)
        {
            var original = Collection.Find(code);

            if (original != null)
                Context.Entry(original).State = System.Data.Entity.EntityState.Deleted;
        }

        public Resource GetByCode(Guid code, Guid? codeApp)
        {
            IQueryable<Resource> result = Collection.AsQueryable();

            if (codeApp.HasValue)
                result = result.Where(f => f.ApplicationCode == codeApp.Value);

            result = result.Where(f => f.Code == code);

            return result.FirstOrDefault();
        }

        public Resource GetByInternalCode(string internalCode, Guid codeApp)
        {
            return Collection.FirstOrDefault(f => f.ApplicationCode == codeApp && f.InternalCode == internalCode);
        }

        public List<Resource> GetApplicationAccess()
        {
            return Collection
                .Where(x => x.Type == Domain.Enums.ResourceType.ApplicationAccess)
                .ToList();
        }
    }
}

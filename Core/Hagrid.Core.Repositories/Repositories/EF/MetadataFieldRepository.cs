using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
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
    public class MetadataFieldRepository : BaseRepository<MetadataField, CustomerContext, Guid>, IMetadataFieldRepository
    {
        public MetadataFieldRepository(IConnection connection) : base(connection) { }

        public VO.SearchResult<MetadataField> Search(DTO.SearchFilterMetadataField filter)
        {
            IQueryable<MetadataField> result = Collection.AsQueryable();

            if (!filter.Term.IsNullOrWhiteSpace())
                result = result.Where(f => f.Name.ToLower().Contains(filter.Term.ToLower()));

            if (!filter.Type.IsNull())
                result = result.Where(f => f.Type == filter.Type);

            if (!filter.JsonId.IsNull())
                result = result.Where(f => f.JsonId == filter.JsonId);

            result = result.OrderBy(i => i.SaveDate);

            var count = result.Count();

            result = result.Skip((filter.Skip.Value) * filter.Take.Value);

            result = result.Take(filter.Take.Value);

            return new VO.SearchResult<MetadataField>(result.ToList(), filter, count);
        }

        public MetadataField GetByJsonId(MetadataField field)
        {
            return Collection.FirstOrDefault(c => c.JsonId == field.JsonId);
        }

        public IEnumerable<MetadataField> GetByType(FieldType type)
        {
            return Collection.Where(c => c.Type == type).ToList();
        }
    }
}
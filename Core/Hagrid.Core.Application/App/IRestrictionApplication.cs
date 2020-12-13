using System;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IRestrictionApplication : IBaseApplication
    {
        DTO.Restriction Save(bool isRKAdmin, Guid clientId, DTO.Restriction restriction);
        DTO.SearchResult Search(bool isRKAdmin, Guid clientId, DTO.SearchFilterRestriction filter);
        DTO.Restriction Get(bool isRKAdmin, Guid clientId, Guid roleCode, Guid code);
        void Delete(Guid roleCode, Guid code);
    }
}

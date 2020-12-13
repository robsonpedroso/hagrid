using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IResourceRepository : 
        IRepository, 
        IRepositoryUpdate<Resource>, 
        IRepositoryGet<Resource, Guid>,
        IRepositoryDeleteByCode<Guid>,
        IRepositorySave<Resource>
    {
        VO.SearchResult<Resource> Search(DTO.SearchFilterResource filter);
        Resource GetByCode(Guid code, Guid? codeApp);
        Resource GetByInternalCode(string internalCode, Guid codeApp);
        List<Resource> GetApplicationAccess();
    }
}

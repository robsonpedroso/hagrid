using Hagrid.Infra.Contracts.Repository;
using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IApplicationRepository : IRepository, IRepositoryGet<Application, Guid?>, IRepositoryList<Application>, IRepositorySave<Application>
    {
        IEnumerable<Application> Get(IEnumerable<string> applicationsName, bool onlyActive = false);

        Application Get(string name, bool onlyActive = false);
    }
}

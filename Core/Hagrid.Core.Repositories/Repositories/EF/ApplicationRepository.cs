using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class ApplicationRepository : BaseRepository<Application, CustomerContext, Guid?>, IApplicationRepository
    {
        public ApplicationRepository(IConnection connection) : base(connection) { }

        public IEnumerable<Application> Get(IEnumerable<string> applicationsName, bool onlyActive = false)
        {
            return Collection
                .Where(a => applicationsName.Contains(a.Name) && (onlyActive.Equals(false) || a.Status))
                .ToList();
        }

        public Application Get(string name, bool onlyActive = false)
        {
            return Collection
                .Where(a => a.Name.Equals(name) && (onlyActive.Equals(false) || a.Status))
                .FirstOrDefault();
        }
    }
}

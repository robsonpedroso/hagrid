using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class AuditLogsRepository : BaseRepository<AuditLogs, CustomerContext, Guid>, IAuditLogsRepository
    {
        public AuditLogsRepository(IConnection connection) : base(connection) { }

        public void Save(IEnumerable<AuditLogs> list)
        {
            Collection.AddRange(list);
        }
    }
}

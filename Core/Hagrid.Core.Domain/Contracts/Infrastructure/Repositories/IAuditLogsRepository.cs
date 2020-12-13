using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IAuditLogsRepository : IRepository, IRepositorySave<AuditLogs>, IRepositoryGet<AuditLogs, Guid>
    {
        void Save(IEnumerable<AuditLogs> list);
    }
}

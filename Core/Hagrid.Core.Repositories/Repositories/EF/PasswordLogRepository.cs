using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class PasswordLogRepository : BaseRepository<Domain.Entities.PasswordLog, CustomerContext, Guid>, IPasswordLogRepository
    {
        public PasswordLogRepository(IConnection connection) : base(connection) { }
    }
}

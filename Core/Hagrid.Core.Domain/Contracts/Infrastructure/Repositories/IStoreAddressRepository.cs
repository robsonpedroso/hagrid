using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IStoreAddressRepository : IRepository, IRepositorySave<StoreAddress>, IRepositoryGet<StoreAddress, Guid>
    {
        IEnumerable<StoreAddress> GetByStore(Guid codeStore, bool activeRecordsOnly = true);
    }
}

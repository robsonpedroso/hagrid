using Hagrid.Infra.Contracts.Repository;
using System.Collections.Generic;

namespace Hagrid.Infra.Contracts
{
    public interface IDomainService
    {
        List<IRepository> GetRepositories();
    }
}

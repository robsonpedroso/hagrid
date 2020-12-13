using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IStoreCreditCardRepository : IRepository, IRepositorySave<StoreCreditCard>
    {
    }
}
using Hagrid.Infra.Contracts.Repository;
using System.Data.Entity;

namespace Hagrid.Infra.Providers.EntityFramework
{
    public interface IEFRepository<T> : IRepository where T : EFContext
    {
        DbContext Context { get; set; }
    }
}

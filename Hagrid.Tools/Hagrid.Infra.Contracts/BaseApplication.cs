using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hagrid.Infra.Contracts
{
    public abstract class BaseApplication
    {
        public abstract IConnection CreateConnection(params IDomainService[] services);

        public abstract IConnection CreateConnection(IRepository[] repositories = null, params IDomainService[] services);

        public virtual T GetRepository<T>(IDomainService service) where T : IRepository
        {
            return (T)service.GetRepositories().FirstOrDefault(r => r is T);
        }

        public virtual void SetConnection(IConnection conn, IDomainService service)
        {
            var repositories = service.GetRepositories();

            if (repositories != null)
            {
                repositories.Where(rep => rep != null).ToList()
                .ForEach(rep => rep.Connection = conn);
            }
        }

        public virtual void SetConnection(IConnection conn, IRepository repository)
        {
            repository.Connection = conn;
        }
    }
}

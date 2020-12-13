using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using System;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface ICustomerRepository : IRepository
    {
        Customer GetByEmail(string email);
        Person GetByCPF(string CPF);
        Company GetByCNPJ(string CNPJ);

        Customer Get(Guid id);
        void Update(Customer obj);
        Customer Save(Customer obj);

        void ChangeCustomerType(Account account, CustomerType type);
    }
}
    
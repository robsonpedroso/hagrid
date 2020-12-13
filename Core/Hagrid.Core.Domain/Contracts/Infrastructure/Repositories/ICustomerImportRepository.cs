using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface ICustomerImportRepository : IRepository
    {
        CustomerImport Get(Guid accountCode);
        IEnumerable<CustomerImport> GetByEmail(string email);
        CustomerImport GetByEmail(string email, Guid storeCode);
        IEnumerable<PersonImport> GetByCPF(string CPF);
        PersonImport GetByCPF(string CPF, Guid storeCode);
        IEnumerable<CompanyImport> GetByCNPJ(string CNPJ);
        CompanyImport GetByCNPJ(string CNPJ, Guid storeCode);
        CustomerImport Save(CustomerImport obj);
        void Update(CustomerImport obj);
    }
}

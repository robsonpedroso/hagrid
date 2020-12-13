using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Providers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class CustomerImportRepository : EFBase<CustomerImport, CustomerContext>, ICustomerImportRepository
    {
        public CustomerImportRepository(IConnection connection)
            => Connection = connection;

        public virtual CustomerImport Save(CustomerImport obj)
        {
            var original = Collection.Find(obj.AccountCode);

            if (original == null)
            {
                return Collection.Add(obj);
            }
            else
            {
                Context.Entry(original).CurrentValues.SetValues(obj);
                Context.Entry(original).State = System.Data.Entity.EntityState.Modified;

                return Collection.Find(obj.AccountCode);
            }
        }

        public virtual void Update(CustomerImport obj)
        {
            Collection.Attach(obj);
            Context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public CustomerImport Get(Guid accountCode)
        {
            return Collection
                .FirstOrDefault(c => c.AccountCode == accountCode && !c.Removed && c.Status);
        }

        public IEnumerable<CustomerImport>GetByEmail(string email)
        {
            return Collection
                .Where(c => c.Email.Equals(email) && !c.Removed && c.Status).ToList();
        }

        public CustomerImport GetByEmail(string email, Guid storeCode)
        {
            return Collection
                .FirstOrDefault(c => c.Email.Equals(email) && !c.Removed && c.Status && c.StoreCode == storeCode);
        }

        public IEnumerable<PersonImport> GetByCPF(string CPF)
        {
            return Context.Set<PersonImport>().Where(c => c.CPF.Equals(CPF) && !c.Removed && c.Status).ToList();
        }

        public PersonImport GetByCPF(string CPF, Guid storeCode)
        {
            return Context.Set<PersonImport>().FirstOrDefault(c => c.CPF.Equals(CPF) && !c.Removed && c.Status && c.StoreCode == storeCode);
        }

        public IEnumerable<CompanyImport> GetByCNPJ(string CNPJ)
        {
            return Context.Set<CompanyImport>().Where(c => c.CNPJ.Equals(CNPJ) && !c.Removed && c.Status).ToList();

        }

        public CompanyImport GetByCNPJ(string CNPJ, Guid storeCode)
        {
            return Context.Set<CompanyImport>().FirstOrDefault(c => c.CNPJ.Equals(CNPJ) && !c.Removed && c.Status && c.StoreCode == storeCode);

        }
    }
}

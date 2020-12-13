using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Core.Domain.Enums;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Providers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Hagrid.Infra.Contracts.Repository;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class CustomerRepository : EFBase<Customer, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(IConnection connection)
            => Connection = connection;

        public virtual Customer Get(Guid id)
        {
            return Collection.FirstOrDefault(c => c.Account.Code.Equals(id) && !c.Removed && c.Status);
        }

        public Customer GetByEmail(string email)
        {
            return Collection
                .Include(c => c.Account)
                .FirstOrDefault(c => c.Email.Equals(email) && !c.Removed && c.Status);
        }

        public Person GetByCPF(string CPF)
        {
            return Context.Set<Person>().FirstOrDefault(c => c.Cpf.Equals(CPF) && !c.Removed && c.Status);
        }

        public Company GetByCNPJ(string CNPJ)
        {
            return Context.Set<Company>().FirstOrDefault(c => c.Cnpj.Equals(CNPJ) && !c.Removed && c.Status);

        }

        public virtual Customer Save(Customer obj)
        {
            Customer original = null;

            original = Collection.Find(obj.Code);

            if (original == null)
            {
                return Collection.Add(obj);
            }
            else
            {
                Context.Entry(original).CurrentValues.SetValues(obj);
                Context.Entry(original).State = System.Data.Entity.EntityState.Modified;

                return Collection.Find(obj.Code);
            }
        }

        public virtual void Update(Customer obj)
        {
#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif

            Collection.Attach(obj);
            Context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void ChangeCustomerType(Account account, CustomerType type)
        {
#if DEBUG

            Context.Database.Log = (s) => Debug.WriteLine(s);

#endif
            Context.Database.ExecuteSqlCommand("UPDATE THCustomer SET Type_Customer = @p0, FirstName_Customer = null, LastName_Customer = null, CPF_Customer = null, RG_Customer = null, BirthDate_Customer = null, Sexo_Customer = null, UpdateDate_Customer = GETDATE() WHERE Guid_Customer = @p1", (int)type, account.Code);  
        }
    }
}

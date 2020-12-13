using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class AccountRoleRepository : BaseRepository<AccountRole, CustomerContext, Guid>, IAccountRoleRepository
    {
        public AccountRoleRepository(IConnection connection) : base(connection) { }

        public override AccountRole Save(AccountRole accountRole)
        {
            var original = Collection.Find(accountRole.Code);

            if (original == null)
            {
                accountRole.SaveDate = accountRole.UpdateDate = DateTime.Now;
                return Collection.Add(accountRole);
            }
            else
            {
                accountRole.UpdateDate = DateTime.Now;
                accountRole.SaveDate = original.SaveDate;

                Context.Entry(original).CurrentValues.SetValues(accountRole);
                Context.Entry(original).State = EntityState.Modified;

                return Collection.Find(accountRole.Code);
            }
        }

        public AccountRole Get(Guid accountCode, Guid roleCode)
        {
            return Collection.Where(ac => ac.RoleCode == roleCode && ac.AccountCode == accountCode && ac.Status).FirstOrDefault();
        }

        public override void Delete(AccountRole accountRole)
        {
            Collection.Remove(accountRole);
        }

        public void Delete(IEnumerable<AccountRole> accountsRole)
        {
            Collection.RemoveRange(accountsRole);
        }

        public override void Delete(Guid code)
        {
            var original = Collection.Find(code);

            if (original != null)
                Context.Entry(original).State = System.Data.Entity.EntityState.Deleted;
        }
    }
}

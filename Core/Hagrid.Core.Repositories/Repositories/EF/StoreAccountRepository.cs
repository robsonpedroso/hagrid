using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Providers.EntityFramework.Context;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DTO = Hagrid.Core.Domain.DTO;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Infrastructure.Repositories.EF
{
    public class StoreAccountRepository : BaseRepository<StoreAccount, CustomerContext, Guid>, IStoreAccountRepository
    {
        public StoreAccountRepository(IConnection connection) : base(connection) { }

        public StoreAccount Get(Guid storeCode, Guid accountCode)
        {
            return Collection
                .Include(c => c.Store)
                .Include(c => c.Account)
                .FirstOrDefault(x => x.StoreCode == storeCode && x.AccountCode == accountCode);
        }

        public override StoreAccount Save(StoreAccount restriction)
        {
            var original = Collection.Find(restriction.Code);

            if (original == null)
            {
                restriction.SaveDate = restriction.UpdateDate = DateTime.Now;

                return Collection.Add(restriction);
            }
            else
            {
                restriction.UpdateDate = DateTime.Now;
                restriction.SaveDate = original.SaveDate;

                Context.Entry(original).CurrentValues.SetValues(restriction);
                Context.Entry(original).State = EntityState.Modified;

                return Collection.Find(restriction.Code);
            }
        }

        public VO.SearchResult<StoreAccount> Search(DTO.SearchFilterStoreAccount filter)
        {
            Expression<Func<StoreAccount, bool>> accountNameExpression = x => (
                x.Account.Customer != null &&
                (
                    (x.Account.Customer is Person && (x.Account.Customer as Person).FirstName == filter.Name) ||
                    (x.Account.Customer is Company && (x.Account.Customer as Company).CompanyName == filter.Name)
                )
            );

            var result = Collection
                .Include(x => x.Store)
                .Include(x => x.Account)
                .Include(x => x.Account.Customer);

            if (!filter.Name.IsNullOrWhiteSpace())
                result = result.Where(accountNameExpression);

            if (!filter.Email.IsNullOrWhiteSpace())
                result = result.Where(x => x.Account.Email == filter.Email);

            if (!filter.StoreCode.IsEmpty())
                result = result.Where(x => x.Store.Code == filter.StoreCode);

            if (!filter.Document.IsNullOrWhiteSpace())
                result = result.Where(x => x.Account.Document == filter.Document);

            result = result.OrderBy(x => x.SaveDate);

            var count = result.Count();

            result = result.Skip(filter.Skip.Value * filter.Take.Value);

            result = result.Take(filter.Take.Value);

            return new VO.SearchResult<StoreAccount>(result.ToList(), filter, count);
        }

        public override void Delete(Guid code)
        {
            var original = Collection.Find(code);

            if (original != null)
                Context.Entry(original).State = EntityState.Deleted;
        }

        public void Delete(IEnumerable<StoreAccount> storeAccounts)
        {
            Collection.RemoveRange(storeAccounts);
        }
    }
}

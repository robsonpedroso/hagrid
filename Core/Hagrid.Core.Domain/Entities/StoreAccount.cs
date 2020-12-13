using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Domain.Entities
{
    public class StoreAccount : IEntity, IDate
    {
        public Guid Code { get; set; }
        public Guid AccountCode { get; set; }
        public Account Account { get; set; }
        public Guid StoreCode { get; set; }
        public Store Store { get; set; }
        public DateTime SaveDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public StoreAccount()
        {
            if (this.Code.IsEmpty())
            {
                Code = Guid.NewGuid();
                SaveDate = DateTime.Now;
                UpdateDate = DateTime.Now;
            }
        }

        public StoreAccount(DTO.StoreAccount storeAccount)
        {
            Code = storeAccount.Code ?? Guid.NewGuid();

            if (!storeAccount.Account.IsNull() && !storeAccount.Account.Code.IsEmpty())
                Account = new Account() { Code = storeAccount.Account.Code };

            if (!storeAccount.Store.IsNull() && !storeAccount.Store.Code.IsEmpty())
                Store = new Store() { Code = storeAccount.Store.Code };
        }
    }
}

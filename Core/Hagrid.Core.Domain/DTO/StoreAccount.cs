using Newtonsoft.Json;
using Hagrid.Infra.Utils;
using System;

namespace Hagrid.Core.Domain.DTO
{
    public class StoreAccount
    {
        [JsonProperty("code")]
        public Guid? Code { get; set; }

        [JsonProperty("account")]
        public Account Account { get; set; }

        [JsonProperty("store")]
        public Store Store { get; set; }

        [JsonProperty("save_date")]
        public DateTime? SaveDate { get; set; }

        [JsonProperty("update_date")]
        public DateTime? UpdateDate { get; set; }

        public StoreAccount()
        {
        }

        public StoreAccount(Entities.StoreAccount storeAccount)
        {
            Code = storeAccount.Code;
            Account = new DTO.Account()
            {
                Code = storeAccount.Account.Code,
                Name = storeAccount.Account.Customer?.Name ?? storeAccount.Account.Email,
                Email = storeAccount.Account.Email,
                Document = storeAccount.Account.Document
            };

            Store = new DTO.Store()
            {
                Code = storeAccount.Store.Code,
                Name = storeAccount.Store.Name,
                Cnpj = storeAccount.Store.Cnpj
            };

            SaveDate = storeAccount.SaveDate;
            UpdateDate = storeAccount.UpdateDate;
        }

        public void SetStore(Guid storeCode)
        {
            Store = new Store() { Code = storeCode };
        }

        public void IsValid()
        {
            if (Account.IsNull() || Account.Code.IsEmpty())
                throw new ArgumentException("Código do usuário não informado");

            if (Store.IsNull() || Store.Code.IsEmpty())
                throw new ArgumentException("Codigo da loja não informado");
        }
    }
}

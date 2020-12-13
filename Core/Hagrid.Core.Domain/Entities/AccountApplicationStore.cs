using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Entities
{
    public class AccountApplicationStore : IEntity, IDate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Code { get; set; }

        public Guid AccountCode { get; set; }

        public Account Account { get; set; }

        public Guid ApplicationStoreCode { get; set; }

        public ApplicationStore ApplicationStore { get; set; }

        public DateTime SaveDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public AccountApplicationStore(){}

        public AccountApplicationStore(Guid accountCode, Guid applicationStoreCode)
        {
            this.AccountCode = accountCode;
            this.ApplicationStoreCode = applicationStoreCode;
            this.SaveDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
        }

    }
}

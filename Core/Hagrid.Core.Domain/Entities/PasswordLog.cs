using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Entities
{
    public class PasswordLog : IEntity
    {
        public Guid Code { get; set; }

        public Guid AccountCode { get; set; }

        public PasswordEventLog Event { get; set; }

        public DateTime SaveDate { get; set; }

        public Guid StoreCode { get; set; }

        public PasswordLog() { }

        public PasswordLog(Guid accountCode, PasswordEventLog eventLog, Guid storeCode)
        {
            Code = Guid.NewGuid();
            AccountCode = accountCode;
            Event = eventLog;
            SaveDate = DateTime.Now;
            StoreCode = storeCode;
        }
    }
}

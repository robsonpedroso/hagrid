using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Entities
{
    public class AuditLogs : IEntity
    {
        public Guid Code { get; set; }
        public AuditLogsType AuditLogsType { get; set; }
        public string ReferenceEntity { get; set; }
        public string ReferenceCode { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public Guid? AccountCode { get; set; }
        public Guid ApplicationStoreCode { get; set; }
        public DateTime SaveDate { get; set; }
        public AuditLogs()
        {

        }
        public AuditLogs(string oldData, string newData)
        {
            this.Code = Guid.NewGuid();
            this.NewData = newData;
            this.OldData = oldData;
            this.SaveDate = DateTime.Now;
        }
    }
}

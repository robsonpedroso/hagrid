using Newtonsoft.Json;
using System;
using System.Linq;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.DTO
{
    public class DBRequisition : Requisition
    {
        [JsonProperty("likedserver")]
        public string LinkedServerName { get; set; }

        [JsonProperty("database")]
        public string DataBaseName { get; set; }

        public DBRequisition() { }

        public DBRequisition(Entities.Requisition requisition)
            : base(requisition)
        {
            var dbRequisition = requisition as Entities.DBRequisition;

            this.LinkedServerName = dbRequisition.LinkedServerName;
            this.DataBaseName = dbRequisition.DataBaseName;

            if (dbRequisition.RequisitionErrors.Count > 0)
                this.Errors = dbRequisition.RequisitionErrors.Select(e => new RequisitionError(e)).ToList();
        }

        public void isValidate()
        {
            if (this.StoreCode.IsNull() || this.StoreCode.Value.IsEmpty())
                throw new ArgumentException("Loja não informada!");

            if (this.DataBaseName.IsNullorEmpty())
                throw new ArgumentException("Banco de dados não informado!");
        }

    }
}

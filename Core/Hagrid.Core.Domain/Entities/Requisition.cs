using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using Hagrid.Infra.Contracts;
using Newtonsoft.Json;
using DTO = Hagrid.Core.Domain.DTO;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.Entities
{
    public abstract class Requisition : IEntity
    {
        public Requisition()
        {
            this.Status = RequisitionStatus.Pending;
            this.RequisitionErrors = new List<RequisitionError>();
            this.Store = new Store();
            this.SaveDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
        }
        
        [JsonIgnore]
        public Guid Code { get; set; }

        //[JsonIgnore]
        //public Guid? StoreCode { get; set; }

        [JsonIgnore]
        public Store Store { get; set; }

        [JsonIgnore]
        public RequisitionStatus Status { get; set; }

        [JsonIgnore]
        public RequisitionType RequisitionType { get; set; }

        [JsonIgnore]
        public List<RequisitionError> RequisitionErrors { get; set; }

        [JsonIgnore]
        public bool Removed { get; set; }

        [JsonIgnore]
        public DateTime SaveDate { get; set; }

        [JsonIgnore]
        public DateTime UpdateDate { get; set; }

        public virtual string ObjectSerialized { get; set; }

        public Requisition(DTO.Requisition requisition) : this()
        {
            this.Code = requisition.Code;
            this.RequisitionType = requisition.RequisitionType;
            this.Status = requisition.Status.ToEnum<RequisitionStatus>();
            
            if (requisition.StoreCode.HasValue)
                this.Store.Code = requisition.StoreCode.Value;
        }

        public abstract DTO.Requisition GetResult();
    }
}

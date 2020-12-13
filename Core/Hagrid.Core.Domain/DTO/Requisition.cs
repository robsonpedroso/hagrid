using Newtonsoft.Json;
using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Hagrid.Infra.Utils;

namespace Hagrid.Core.Domain.DTO
{
    [JsonObject("requisition")]
    public class Requisition
    {
        [JsonProperty("code")]
        public Guid Code { get; set; }

        [JsonProperty("store_code")]
        public Guid? StoreCode { get; set; }

        [JsonProperty("errors")]
        public List<RequisitionError> Errors { get; set; }

        [JsonProperty("type_requisition")]
        public RequisitionType RequisitionType { get; set; }

        [JsonProperty("type_description")]
        public string RequisitionDescription { get { return this.RequisitionType.GetDescription(); } }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("status_description")]
        public string StatusDescription { get { return this.Status.ToEnum<RequisitionStatus>().GetDescription(); } }

        [JsonProperty("save_date")]
        public virtual DateTime SaveDate { get; set; }

        [JsonProperty("update_date")]
        public DateTime UpdateDate { get; set; }

        public Requisition()
        {

        }

        public Requisition(Entities.Requisition requisition)
        {
            Code = requisition.Code;
            StoreCode = requisition.Store.Code;
            RequisitionType = requisition.RequisitionType;
            Status = requisition.Status.AsInt();
            SaveDate = requisition.SaveDate;
            UpdateDate = requisition.UpdateDate;
            
            if (requisition.RequisitionErrors.Count() > 0)
                Errors = requisition.RequisitionErrors.Select(e => new RequisitionError(e)).ToList();
        }

    }
}

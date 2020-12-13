using Hagrid.Infra.Utils;
using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Contracts;
using Newtonsoft.Json;

namespace Hagrid.Core.Domain.Entities
{
    public class RequisitionError : IEntity
    {
        [JsonIgnore]
        public Guid Code { get; set; }

        [JsonIgnore]
        public Requisition Requisition { get; set; }

        public int Line { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public List<String> ErrorMessages { get; set; }

        public RequisitionError() => this.Code = Guid.NewGuid();


        [JsonIgnore]
        public string ObjectSerialized
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
            set
            {
                RequisitionError savedRequisition = string.IsNullOrEmpty(value)
                        ? new RequisitionError()
                        : JsonConvert.DeserializeObject<RequisitionError>(value);

                this.Line = savedRequisition.Line;
                this.ErrorMessages = savedRequisition.ErrorMessages;
                this.Email = savedRequisition.Email;
                this.Name = savedRequisition.Name;
            }
        }
    }
}

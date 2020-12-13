using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Entities
{
    public class DBRequisition : Requisition
    {
        public DBRequisition()
        {

        }

        [NotMapped]
        public string LinkedServerName { get; set; }

        [NotMapped]
        public string DataBaseName { get; set; }

        [JsonIgnore]
        public override string ObjectSerialized
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
            set
            {
                DBRequisition  savedRequisition = string.IsNullOrEmpty(value)
                        ? new DBRequisition()
                        : JsonConvert.DeserializeObject<DBRequisition>(value);

                this.LinkedServerName = savedRequisition.LinkedServerName;
                this.DataBaseName = savedRequisition.DataBaseName;
            }
        }

        public DBRequisition(DTO.DBRequisition requisition)
            : base(requisition)
        {
            this.DataBaseName = requisition.DataBaseName;
            this.LinkedServerName = requisition.LinkedServerName;
        }

        public override DTO.Requisition GetResult()
        {
            return new DTO.DBRequisition(this);
        }
    }
}

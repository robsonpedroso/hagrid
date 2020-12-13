using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;
using System.Configuration;
using Newtonsoft.Json;
using Hagrid.Core.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hagrid.Core.Domain.Entities
{
    public class Application : IEntity<Guid?>, IStatus
    {
        public Guid? Code { get; set; }
        public String Name { get; set; }
        
        public AuthType AuthType { get; set; }
        public MemberType MemberType { get; set; }
        public virtual int RefreshTokenLifeTimeInMinutes { get; internal set; }
        
        public bool Status { get; set; }
        public DateTime SaveDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string ObjectSerialized {
            get
            {
                return JsonConvert.SerializeObject(this.Informations);
            }
            set
            {
                this.Informations = string.IsNullOrEmpty(value)
                        ? new ApplicationInformation()
                        : JsonConvert.DeserializeObject<ApplicationInformation>(value);
            }
        }

        [NotMapped]
        public ApplicationInformation Informations { get; set; }

        public ICollection<ApplicationStore> ApplicationsStore { get; set; }

        public ICollection<Resource> Resources { get; set; }

        public Application(){}

        public Application(int refreshTokenLifeTimeInMinutes)
        {
            this.RefreshTokenLifeTimeInMinutes = refreshTokenLifeTimeInMinutes;
        }
    }
}

using Hagrid.Core.Domain.ValueObjects;
using Hagrid.Infra.Utils;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Entities
{
    public class TransferToken : SmallToken, IEntity<string>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid ClientId { get; set; }

        protected TransferToken()
        {

        }

        public TransferToken(Guid ownerCode, string name, Guid clientId)
        {
            if (ownerCode.Equals(Guid.Empty))
                throw new ArgumentException("O parâmetro ownerCode não pode ser nulo");

            this.Code = Guid.NewGuid().EncryptDES();
            this.OwnerCode = ownerCode;
            this.Name = name;
            this.ClientId = clientId;
            this.ExpiresUtc = DateTime.UtcNow.AddSeconds(Config.TransferTokenExpiresUtc); 
        }

        public bool Validate()
        {
            if (DateTime.UtcNow > ExpiresUtc || OwnerCode.Value.IsEmpty() || ClientId.IsEmpty() || Name.IsNullOrWhiteSpace())
                return false;

            return true;
        }
    }
}

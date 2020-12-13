using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Entities
{
    public class Address : IEntity, IStatus, IDate
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public Guid Code { get; set; }

        /// <summary>
        /// Address Identifier
        /// </summary>
        public string AddressIdentifier { get; set; }

        /// <summary>
        /// Contact Name
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Zip Code
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Address Street
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Address Number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Address Complement
        /// </summary>
        public string Complement { get; set; }

        /// <summary>
        // District
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Primary Phone Number
        /// </summary>
        public string PhoneNumber1 { get; set; }

        /// <summary>
        /// Aditional Phone Number
        /// </summary>
        public string PhoneNumber2 { get; set; }

        /// <summary>
        /// Aditional Phone Number
        /// </summary>
        public string PhoneNumber3 { get; set; }

        /// <summary>
        /// Address Status
        /// </summary>
        public bool Status { get; set; }
        
        /// <summary>
        /// Save Date for Address
        /// </summary>
        public DateTime SaveDate { get; set; }

        /// <summary>
        /// Update Date for Address
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Address()
        {
            if (this.Code.IsEmpty())
            {
                Code = Guid.NewGuid();
                Status = true;
                SaveDate = DateTime.Now;
                UpdateDate = DateTime.Now;
            }
        }
    }
}

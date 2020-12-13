using System;
//using Hagrid.Core.Domain;

namespace Hagrid.Core.Domain.Entities
{
    public class StoreAddress :  Address
    {
        public Guid StoreCode { get; set; }
     
        public Store Store { get; set; }
    }
}
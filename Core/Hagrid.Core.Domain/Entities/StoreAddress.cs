using Hagrid.Infra.Utils;
using System;
//using Hagrid.Core.Domain;

namespace Hagrid.Core.Domain.Entities
{
    public class StoreAddress :  Address
    {
        public Guid StoreCode { get; set; }
     
        public Store Store { get; set; }

        public StoreAddress(DTO.StoreAddress storeAddress)
        {
            this.AddressIdentifier = storeAddress.AddressIdentifier;
            this.City = storeAddress.City;
            this.Code = storeAddress.Code;
            this.Complement = storeAddress.Complement;
            this.ContactName = storeAddress.ContactName;
            this.District = storeAddress.District;
            this.Number = storeAddress.Number;
            this.PhoneNumber1 = !storeAddress.PhoneNumber1.ToNumber().ToString().IsNullOrWhiteSpace() ?
                                storeAddress.PhoneNumber1.ToNumber().ToString() :
                                storeAddress.Phone.ToNumber().ToString();
            this.PhoneNumber2 = storeAddress.PhoneNumber2.ToNumber().ToString();
            this.PhoneNumber3 = storeAddress.PhoneNumber3.ToNumber().ToString();
            this.State = storeAddress.State;
            this.Status = true;
            this.Street = storeAddress.Street;
            this.ZipCode = storeAddress.ZipCode;
            this.SaveDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
        }
    }
}
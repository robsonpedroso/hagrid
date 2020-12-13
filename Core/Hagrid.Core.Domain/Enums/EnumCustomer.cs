using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Enums
{
    
    public enum CustomerType
    {
        [JsonClass(
            typeof(DTO.Person))]
        Person = 0,

        [JsonClass(
            typeof(DTO.Company))]
        Company = 1
    }

    public enum AddressType
    {
        None = 0,
        HomeAddress = 1,
        Comercial = 2,
        Other = 3
    }

    public enum AddressPurposeType
    {
        None = 0,
        Contact = 1,
        Shipping = 2
    }

    public enum ExternalType
    {
        AddressExternal = 1,
        CustomerExternal = 2
    }

    public enum PhoneType
    {
        Comercial = 1,
        Residencial = 2,
        Fax = 3,
        Celular = 4,
        Outros = 5
    }

    [DataContract]
    public enum LocalLogin
    {
        [EnumMember]
        Main = 0,

        [EnumMember]
        Jump = 1
    }

    public enum GlobalIdType
    {
        Local = 0,
        Export = 1,
        Import = 2,
    }
}

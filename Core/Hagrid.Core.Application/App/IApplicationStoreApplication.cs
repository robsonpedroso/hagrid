using Hagrid.Core.Domain.Entities;
using DTO = Hagrid.Core.Domain.DTO;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Application.Contracts
{
    public interface IApplicationStoreApplication
    {
        bool Authenticate(string clientId, string clientSecret, string reqOrigin, out string message, out ApplicationStore applicationStore);

        bool Exists(string clientId);

        ApplicationStore Get(Guid code);

        ApplicationStore GetByClientId(Guid clientId);

        IEnumerable<DTO.AccountApplicationStore> GetByAccount(Guid accountCode, Guid clientId);

        DTO.ApplicationStore GetApplicationStoreByStore(Guid storeCode, string nameApplication);

        DTO.ApplicationStore GetApplicationStoreByStoreTypeMain(string nameApplication);

        DTO.Store GetByStore(Guid storeCode, Guid clientId);
    }
}

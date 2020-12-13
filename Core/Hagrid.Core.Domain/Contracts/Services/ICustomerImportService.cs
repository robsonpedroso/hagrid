using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface ICustomerImportService : IDomainService
    {
        bool Exists(string username, Guid storeCode);

        CustomerImport Get(string username, Guid storeCode);

        void RemoveMember(string email, string document);
    }
}

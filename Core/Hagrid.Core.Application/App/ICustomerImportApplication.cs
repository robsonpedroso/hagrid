using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Application.Contracts
{
    public interface ICustomerImportApplication
    {
        void Save(CustomerImport customerImport);

        bool Exists(string username, Guid storeCode);

        void CreatePassword(Guid memberCode, string tokenCode, string newPassword, Guid? clientId = null);

        object ValidatePassword(Guid memberCode, string password);

        void RemoveMember(string email, string document);
    }
}

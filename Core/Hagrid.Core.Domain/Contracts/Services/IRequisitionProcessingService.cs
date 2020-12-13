using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IRequisitionProcessingService : IDomainService
    {
        void ImportCustomerToImportDb(Requisition requisition);

        void ClearImportDb(Requisition requisition);

        object[] GetAccounts(Requisition requisition, int skip, int take);

        ApplicationStore GetApplicationStore(Guid storeCode);

        bool SaveAccount(Requisition requisition, Object account, ApplicationStore applicationStore, IConnection conn);
    }
}

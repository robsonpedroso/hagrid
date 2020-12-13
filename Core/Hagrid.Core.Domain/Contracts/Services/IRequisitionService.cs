using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts;
using System;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IRequisitionService : IDomainService
    {
        Requisition Save(Requisition requisition);

        void SaveFile(Requisition requisition);

        string SaveCSVFile(Guid requisitionCode);

        void Delete(Guid pCode);

        void UpdateStatus(Requisition fileImport, RequisitionStatus status);
    }
}

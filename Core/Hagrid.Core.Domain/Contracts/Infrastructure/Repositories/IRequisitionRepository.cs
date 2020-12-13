using Hagrid.Core.Domain.Entities;
using Hagrid.Core.Domain.Enums;
using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Repositories
{
    public interface IRequisitionRepository : IRepository, IRepositorySave<Requisition>, IRepositoryUpdate<Requisition>
    {
        IEnumerable<Requisition> GetByStore(Guid storeCode, bool withErrors = false);

        Requisition Get(Guid code);

        IEnumerable<Requisition> GetByStatus(RequisitionStatus status);

        IEnumerable<Requisition> GetStatusAndSetProcessing(RequisitionStatus status);

        RequisitionError SaveError(RequisitionError error);

        Requisition Get(Guid code, bool withErrors = false);
    }
}
    
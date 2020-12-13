using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts;
using System;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IRoleService : IDomainService
    {
        Role Save(Role role);
        void SaveAll(Guid storeCode);
    }
}

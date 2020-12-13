using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO = Hagrid.Core.Domain.Entities;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Services
{
    public interface ICustomerImportDBInfraService
    {
        bool ImportCustomer(DO.Requisition requisition);
        List<DTO.Account> GetCustomers(DO.Requisition requisition, int skip, int take);
        void Clear(DO.Requisition requisition);
    }
}

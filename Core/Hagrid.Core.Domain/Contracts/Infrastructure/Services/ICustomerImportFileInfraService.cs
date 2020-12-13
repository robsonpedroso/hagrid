using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Services
{
    public interface ICustomerImportFileInfraService
    {
        Tuple<bool, List<string>, CustomerImport> ValidCustomer(string[] properties, FileRequisition fileRequisition);
    }
}

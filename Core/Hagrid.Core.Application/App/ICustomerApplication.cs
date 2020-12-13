using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface ICustomerApplication
    {
        bool IsMemberExistsByEmail(Guid clientId, string email);
        bool IsMemberExistsByDocument(Guid clientId, string CPF);
    }
}

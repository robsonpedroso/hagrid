using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IApplicationApplication
    {
        DTO.Application GetByName(string name);

        object GetApplications();
    }
}

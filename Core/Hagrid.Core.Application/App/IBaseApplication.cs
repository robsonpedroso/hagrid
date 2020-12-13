using Hagrid.Infra.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Application.Contracts
{

    public interface IBaseApplication
    {
        IConnection Connection { get; set; }
    }
}

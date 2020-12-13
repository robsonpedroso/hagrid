using Hagrid.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Application.Contracts
{
    public interface ITransferTokenApplication
    {
        void Save(TransferToken token);

        TransferToken Get(string code);
    }
}

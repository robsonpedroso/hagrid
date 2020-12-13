using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IBlacklistApplication: IBaseApplication
    {
        DTO.Blacklist Get(Guid code);
        DTO.Blacklist Block(DTO.Blacklist blacklist);
        DTO.Blacklist Unlock(DTO.Blacklist blacklist);
    }
}

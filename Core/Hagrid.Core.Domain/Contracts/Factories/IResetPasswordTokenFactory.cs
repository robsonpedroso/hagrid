using Hagrid.Core.Domain.Contracts.Entities;
using Hagrid.Core.Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Factories
{
    public interface IResetPasswordTokenFactory
    {
        IResetPasswordTokenService GetResetPasswordTokenService(IResetPasswordTokenOwner owner);
    }
}

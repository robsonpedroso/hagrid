﻿using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IRefreshTokenService : IDomainService
    {
        RefreshToken Get(string code);
    }
}

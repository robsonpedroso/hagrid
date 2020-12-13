using Hagrid.Core.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Hagrid.Core.Domain.Contracts.Infrastructure.Services
{
    public interface IAccountInfraService
    {
        DTO.AccountToken GetToken(string transfertoken, Guid clientId, string secret);

        object GetChangePasswordToken(string token, Guid clientId, string secret);

        object ChangePassword(string token, string password, string newPassword, Guid clientId, string secret);
    }
}
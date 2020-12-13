using Hagrid.Core.Domain.ValueObjects;
using System;

namespace Hagrid.Core.Application.Contracts
{
    public interface IAccountResetPasswordTokenApplication : IResetPasswordTokenApplication { }
    public interface ICustomerImportResetPasswordTokenApplication : IResetPasswordTokenApplication { }

    public interface IResetPasswordTokenApplication
    {
        string GenerateResetPasswordToken(string email, Guid clientId, string urlBack = "", int emailTemplateCode = 0);
        void DeleteResetPasswordToken(ResetPasswordToken token);
        ResetPasswordToken GetResetPasswordToken(string code);
        ChangePasswordToken CreateChangePasswordToken(Guid accountCode, Guid clientId, string urlBack, bool showMessage);
    }
}

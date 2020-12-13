
namespace Hagrid.Core.Domain.Contracts.Policies
{
    public interface IPasswordPolicy
    {
        bool Validate(string email, string password, bool throwEx = true);
    }
}

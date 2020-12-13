using DTO = Hagrid.Core.Domain.DTO;

namespace Hagrid.Core.Application.Contracts
{
    public interface IStoreCreditCardApplication
    {
        void Save(DTO.StoreCreditCard credicard);

        string GetSecretPhrase(string phrase);
    }
}

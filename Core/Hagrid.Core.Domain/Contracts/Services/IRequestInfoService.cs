using System.Collections.Generic;

namespace Hagrid.Core.Domain.Contracts.Services
{
    public interface IRequestInfoService
    {
        T GetInfoRequest<T>(string key);
        void SetInfoRequest(object key, object value);
    }
}

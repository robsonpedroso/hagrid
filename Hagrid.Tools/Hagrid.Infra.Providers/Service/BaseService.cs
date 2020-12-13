using Hagrid.Infra.Providers.Entities;
using Hagrid.Infra.Utils;
using RestSharp;
using RestSharp.Authenticators;

namespace Hagrid.Infra.Providers.Service
{
    public class BaseService
    {
        protected RestClient _client;

        protected RKConfiguration _configuration;

        public BaseService(RKConfiguration configuration)
        {
            this._configuration = configuration;

            _client = new RestClient(_configuration.BaseURL);

            _client.Authenticator = new HttpBasicAuthenticator(_configuration.UserName, _configuration.Password);

            if (_configuration.StoreCode.IsNull())
                _client.AddDefaultHeader("X-StoreCode", _configuration.StoreCode.AsString());

            if (_configuration.IntegrationKey.IsNull())
                _client.AddDefaultHeader("X-IntegrationKey", _configuration.IntegrationKey.AsString());
        }

    }
}

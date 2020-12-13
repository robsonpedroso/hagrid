using Hagrid.Infra.Providers.Contracts;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Infra.Providers.RestServicesAuthConfiguration
{
    /// <summary>
    /// Represents a Configuration for non Authenticated Rest API
    /// </summary>
    public class NoAuthRestConfiguration : IRestServiceConfiguration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">Base URl for API</param>
        public NoAuthRestConfiguration(String url)
        {
            this.BaseURL = url;
        }

        /// <summary>
        /// Base URL
        /// </summary>
        public string BaseURL { get; set; }

        /// <summary>
        /// Create Rest Client for non Authenticated Rest Api
        /// </summary>
        /// <returns></returns>
        public RestClient CreateRestClient()
        {
            RestClient client = new RestClient(BaseURL);

            return client;
        }
    }
}

using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Infra.Providers.Contracts
{
    /// <summary>
    /// Default contract for all Rest Services
    /// </summary>
    public interface IRestServiceConfiguration
    {
        /// <summary>
        /// Create Rest Client based on Configuration
        /// </summary>
        /// <returns></returns>
        RestClient CreateRestClient();

        /// <summary>
        /// Base URL for API Service
        /// </summary>
        string BaseURL { get; set; }
    }
}

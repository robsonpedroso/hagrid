using Newtonsoft.Json.Linq;
using Hagrid.Infra.Providers.Contracts;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hagrid.Infra.Utils;

namespace Hagrid.Infra.Providers.Base
{
    /// <summary>
    /// Base class for REST Services  
    /// </summary>
    public class BaseRestService
    {
        /// <summary>
        /// Rest Client
        /// </summary>
        private RestClient client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Rest Configuration for API</param>
        public BaseRestService(IRestServiceConfiguration config)
        {
            this.client = config.CreateRestClient();
        }

        /// <summary>
        /// Get the object from the service
        /// </summary>
        /// <param name="url">Method URL to be read</param>
        /// <param name="parameters">Aditional request parameters</param>
        /// <returns>JObject with the current response</returns>
        protected JObject Get(string url, Dictionary<string, string> parameters = null)
        {
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("Content-Type", "application/json");

            AddParametersToRequest(request, parameters);

            var response = client.Execute<JObject>(request);

            if (response.IsNull())
                throw new Exception("Problemas na comunicação com a api de Global Search.");

            return response.Content.JsonTo<JObject>();
        }

        /// <summary>
        /// Update the object from the service
        /// </summary>
        /// <param name="url">Method URL to update the object</param>
        /// <param name="jsonObjectToBeUpdated">Object to be update on remote API</param>
        /// <param name="parameters">Aditional request parameters</param>
        /// <returns>JObject with the current response</returns>
        protected JObject Put(string url, string jsonObjectToBeUpdated, Dictionary<string, string> parameters = null)
        {
            var request = new RestRequest(url, Method.Put);
            request.AddHeader("Content-Type", "application/json");

            //Adding Body
            request.AddParameter("application/json", jsonObjectToBeUpdated, ParameterType.RequestBody);

            AddParametersToRequest(request, parameters);

            var response = client.Execute<JObject>(request);

            if (response.IsNull())
                throw new Exception("Problemas na comunicação com a api.");

            return response.Content.JsonTo<JObject>();
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url">Url for Post</param>
        /// <param name="bodyContent">Body content</param>
        /// <param name="parameters">Parameters for post</param>
        /// <returns>JObject with result</returns>
        protected JObject Post(string url, string bodyContent = null, Dictionary<string, string> parameters = null)
        {
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");

            AddParametersToRequest(request, parameters);

            //Adding Body
            if(!bodyContent.IsNullorEmpty())
                request.AddParameter("application/json", bodyContent, ParameterType.RequestBody);

            var response = client.Execute<JObject>(request);

            if (response.IsNull())
                throw new Exception("Problemas na comunicação com a api.");

            return response.Content.JsonTo<JObject>();
        }

        /// <summary>
        /// Add parameters to request
        /// </summary>
        /// <param name="request">Request to be invoked</param>
        /// <param name="parameters">Aditional parameters to request</param>
        private void AddParametersToRequest(RestRequest request, Dictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }
        }
    }
}

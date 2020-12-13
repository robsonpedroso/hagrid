using System;

namespace Hagrid.Infra.Providers.Entities
{
    public class RKConfiguration
    {
        internal string BaseURL { get; private set; }

        /// <summary>
        /// The username to base auth
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// The password to base auth
        /// </summary>
        public string Password { get; private set; }

        public Guid? StoreCode { get; private set; }

        public Guid? IntegrationKey { get; private set; }

        public RKConfiguration(string baseURL, string userName, string password, Guid? storeCode = null, Guid? integrationKey = null)
        {
            BaseURL = baseURL;
            UserName = userName;
            Password = password;
            StoreCode = storeCode;
            IntegrationKey = integrationKey;
        }
     }
}

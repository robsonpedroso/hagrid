using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hagrid.Infra.Providers.Contracts
{
    /// <summary>
    /// Contract for Basic Authentication Rest Service
    /// </summary>
    internal interface IBasicAuthRestServiceConfiguration : IRestServiceConfiguration
    {
        /// <summary>
        /// User name for authentication
        /// </summary>
        string username { get; set; }

        /// <summary>
        /// Password for authentication
        /// </summary>
        string password { get; set; }
    }
}

using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxyContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual bool IsProxy(Type type)
        {
            var proxyClasses = new string[]
            {
                "NHibernate.Proxy.DynamicProxy.IProxy",
                "NHibernate.Proxy.INHibernateProxy"
            };

            return type.GetInterfaces()
                .Any(i => proxyClasses.Contains(i.FullName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        protected override JsonContract CreateContract(Type objectType)
        {
            if (IsProxy(objectType))
                return base.CreateContract(objectType.BaseType);
            else
                return base.CreateContract(objectType);
        }
    }
}

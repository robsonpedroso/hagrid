using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class AttributeContractResolver<TAttribute> : ProxyContractResolver where TAttribute : Attribute
    {
        private readonly bool ignore;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignore"></param>
        public AttributeContractResolver(bool ignore = true)
            => this.ignore = ignore;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var hasAttribute = member.CustomAttributes.Any(p => p.AttributeType == typeof(TAttribute));

            if (hasAttribute)
                return ignore ? null : base.CreateProperty(member, memberSerialization);

            return ignore ? base.CreateProperty(member, memberSerialization) : null;
        }
    }
}

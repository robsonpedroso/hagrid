using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PropertiesConverter : JsonConverter
    {
        private IEnumerable<string> properties;
        private bool allow;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertiesString"></param>
        /// <param name="allow"></param>
        public PropertiesConverter(string propertiesString, bool allow = true)
        {
            this.allow = allow;

            if (!propertiesString.IsNullOrWhiteSpace())
                this.properties = propertiesString.Replace(" ", "").Split(',');
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead { get { return false; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsClass;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueType = value.GetType();

            if (valueType.IsConstructedGenericType)
                valueType = valueType.GetGenericArguments().FirstOrDefault();

            var propertiesToAllow = properties.Select(name =>
            {
                if (value.IsNull())
                    return name;

                var property = valueType.GetProperty(name);

                if (property.IsNull() || property.DeclaringType.IsNull())
                    return name;

                return string.Format("{0}.{1}", property.DeclaringType, name);
            });

            var resolver = allow
                ? new AllowPropertyContractResolver(propertiesToAllow.ToArray()) as PropertyContractResolver
                : new IgnorePropertyContractResolver(propertiesToAllow.ToArray()) as PropertyContractResolver;

            var serializerNew = new JsonSerializer
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.None,
                FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.DefaultValue,
                FloatParseHandling = Newtonsoft.Json.FloatParseHandling.Double,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling  = Newtonsoft.Json.PreserveReferencesHandling.None,
                DateFormatString = "yyyy-MM-ddTHH:mm:ss",
                ContractResolver = resolver
            };

            serializerNew.Serialize(writer, value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IgnorePropertiesConverter : PropertiesConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertiesString"></param>
        public IgnorePropertiesConverter(string propertiesString) : base(propertiesString, false) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AllowPropertiesConverter : PropertiesConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertiesString"></param>
        public AllowPropertiesConverter(string propertiesString) : base(propertiesString, true) { }
    }
}

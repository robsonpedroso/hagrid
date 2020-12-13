using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Hagrid.Infra.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class DiscriminatorConverter : JsonConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool CanWrite { get { return false; } }

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
            try
            {
                var discriminatorProperty = objectType.GetProperties()
                    .FirstOrDefault(p => p.GetCustomAttributes(typeof(JsonDiscriminatorAttribute), false).Length > 0);

                if (discriminatorProperty != null && discriminatorProperty.PropertyType.IsEnum)
                {
                    JObject jsonValue = JObject.Load(reader);

                    string stringValue = string.Empty;

                    if (jsonValue[discriminatorProperty.Name] != null)
                    {
                        stringValue = jsonValue[discriminatorProperty.Name].ToString();
                    }
                    else if (jsonValue[discriminatorProperty.Name.ToLower()] != null)
                    {
                        stringValue = jsonValue[discriminatorProperty.Name.ToLower()].ToString();
                    }

                    if (!stringValue.IsNullOrWhiteSpace())
                    {
                        var enumValue = Enum.Parse(discriminatorProperty.PropertyType, stringValue, true);

                        if (enumValue != null)
                        {
                            var attribute = enumValue.GetType()
                                .GetField(enumValue.ToString())
                                .GetCustomAttributes(typeof(JsonClassAttribute), false)
                                .FirstOrDefault() as JsonClassAttribute;

                            if (attribute != null)
                            {
                                var target = Activator.CreateInstance(attribute.ClassTypes.FirstOrDefault(t => t.Equals(objectType) || t.IsSubclassOf(objectType)));
                                serializer.Populate(jsonValue.CreateReader(), target);
                                return target;
                            }
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonDiscriminatorAttribute : Attribute { }

    /// <summary>
    /// 
    /// </summary>
    public class JsonClassAttribute : Attribute
    {
        private Type[] classTypes;

        /// <summary>
        /// 
        /// </summary>
        public Type[] ClassTypes { get { return classTypes; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classTypes"></param>
        public JsonClassAttribute(params Type[] classTypes)
        {
            this.classTypes = classTypes;
        }
    }
}

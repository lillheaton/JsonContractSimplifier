using EOls.Serialization.Services.ConverterLocator;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace EOls.Serialization
{
    public class SimplifyJsonConverter : JsonConverter
    {
        private readonly IConverterLocatorService _converterLocatorService;

        public SimplifyJsonConverter() : 
            this(new ConverterLocatorService())
        {
        }

        public SimplifyJsonConverter(IConverterLocatorService converterLocatorService)
        {
            _converterLocatorService = converterLocatorService;
        }

        private static bool ImplementsGenericInterface(object target, Type genericInterfaceType)
        {
            return target
                .GetType()
                .GetInterfaces()
                .Any(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition() == genericInterfaceType);
        }
        
        private static object ConvertObject(IConverter converter, object target)
        {
            Type converterType = converter.GetType();

            if (!ImplementsGenericInterface(converter, typeof(IObjectConverter<>)))
            {
                throw new ArgumentException($"Converter type {converterType.Name} does not implement IObjectConverter");
            }

            var method = converterType.GetMethod("Convert");
            return method.Invoke(converter, new[] { target });
        }
        
            
        public override bool CanConvert(Type objectType)
        {
            return _converterLocatorService.TryFindConverterFor(objectType, out IConverter converter);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type targetType = value.GetType();

            if(!_converterLocatorService.TryFindConverterFor(targetType, out IConverter converter))
            {
                throw new ArgumentException($"Could not find targetType {targetType.Name}");
            }

            object result = ConvertObject(converter, value);
            Type resultType = result.GetType();

            if (resultType.IsValueType || result is string)
            {
                writer.WriteValue(resultType);                
            }
            else if (resultType.IsArray)
            {
                writer.WriteStartArray();
                serializer.Serialize(writer, result);
                writer.WriteEndArray();                
            }
            else if (resultType.IsClass)
            {
                serializer.Serialize(writer, result);                
            }
        }

        #region JsonRead
        public override bool CanRead => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { throw new NotImplementedException(); }
        #endregion
    }
}

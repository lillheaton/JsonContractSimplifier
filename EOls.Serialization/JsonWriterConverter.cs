using EOls.Serialization.Services.ConverterLocator;
using Newtonsoft.Json;
using System;

namespace EOls.Serialization
{
    public class JsonWriterConverter : JsonConverter
    {
        private readonly IConverterLocatorService _converterLocatorService;

        public JsonWriterConverter() : 
            this(new ConverterLocatorService())
        {
        }

        public JsonWriterConverter(IConverterLocatorService converterLocatorService)
        {
            _converterLocatorService = converterLocatorService;
        }
        
        private static object ConvertObject(IConverter converter, object target)
        {
            Type converterType = converter.GetType();

            if (converterType != typeof(IObjectConverter<>))
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
                return;
            }

            if (resultType.IsArray)
            {
                writer.WriteStartArray();
                serializer.Serialize(writer, result);
                writer.WriteEndArray();
            }

            if (resultType.IsClass)
            {
                writer.WriteStartObject();
                serializer.Serialize(writer, result);
                writer.WriteEndObject();
            }
        }

        #region JsonRead
        public override bool CanRead => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { throw new NotImplementedException(); }
        #endregion
    }
}

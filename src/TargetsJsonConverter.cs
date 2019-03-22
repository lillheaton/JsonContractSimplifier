using EOls.Serialization.Services.ConverterLocator;
using Newtonsoft.Json;
using System;
using System.Collections;

namespace EOls.Serialization
{
    public class TargetsJsonConverter : JsonConverter
    {
        private readonly IConverterLocatorService _converterLocatorService;

        public TargetsJsonConverter(IConverterLocatorService converterLocatorService)
        {
            _converterLocatorService = converterLocatorService;
        }

        public TargetsJsonConverter() :
            this(new ConverterLocatorService())
        {
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

            object result = _converterLocatorService.Convert(value, converter);
            Type resultType = result.GetType();

            switch (resultType)
            {
                case object _ when resultType.IsValueType || resultType == typeof(string):
                    writer.WriteValue(result);
                    break;

                case IEnumerable enumerable
                when targetType.IsArray || (targetType.IsGenericType && result is IEnumerable):
                    writer.WriteStartArray();
                    serializer.Serialize(writer, enumerable);
                    writer.WriteEndArray();
                    break;

                case object _ when resultType.IsClass:
                    serializer.Serialize(writer, result);
                    break;

                default:
                    serializer.Serialize(writer, result);                    
                    break;
            }            
        }

        #region JsonRead
        public override bool CanRead => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { throw new NotImplementedException(); }
        #endregion
    }
}

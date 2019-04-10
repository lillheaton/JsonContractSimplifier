using JsonContractSimplifier.Services.ConverterLocator;
using Newtonsoft.Json;
using System;
using System.Collections;

namespace JsonContractSimplifier
{
    public class TargetsJsonConverter : JsonConverter
    {
        private readonly IConverter _converter;
        private readonly IConverterLocatorService _converterLocatorService;

        public TargetsJsonConverter(IConverter converter, IConverterLocatorService converterLocatorService)
        {
            _converter = converter;
            _converterLocatorService = converterLocatorService;
        }        
            
        public override bool CanConvert(Type objectType)
        {
            var converterType = _converter.GetType();
            return converterType.GetGenericArguments()[0] == objectType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type targetType = value.GetType();            
            object result = _converterLocatorService.Convert(value, _converter);
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

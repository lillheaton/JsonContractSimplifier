using JsonContractSimplifier.Services.ConverterLocator;
using Newtonsoft.Json;
using System;

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
            object convertedValue = _converterLocatorService.Convert(value, _converter);
            Type convertedValueType = convertedValue.GetType();

            if (convertedValueType.IsValueType || convertedValueType == typeof(string))
            {
                writer.WriteValue(convertedValue);
            }
            else
            {
                serializer.Serialize(writer, convertedValue);
            }
        }

        #region JsonRead
        public override bool CanRead => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { throw new NotImplementedException(); }
        #endregion
    }
}

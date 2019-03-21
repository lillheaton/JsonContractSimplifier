using EOls.Serialization.Services.ConverterLocator;
using EOls.Serialization.Tests.Models;

namespace EOls.Serialization.Tests.Converters
{
    public class ModelAConverter : IObjectConverter<ModelA>
    {
        public object Convert(ModelA target)
        {
            return "Converted";
        }
    }
}

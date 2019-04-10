using JsonContractSimplifier.Services.ConverterLocator;
using JsonContractSimplifier.Tests.Models;

namespace JsonContractSimplifier.Tests.Converters
{
    public class ModelAConverter : IObjectConverter<ModelA>
    {
        public object Convert(ModelA target)
        {
            return "Converted";
        }
    }
}

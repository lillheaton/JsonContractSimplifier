using JsonContractSimplifier.Services.ConverterLocator;
using JsonContractSimplifier.Tests.Models;

namespace JsonContractSimplifier.Tests.Converters
{
    public class ModelBConverter : IObjectConverter<ModelB>
    {
        public object Convert(ModelB target)
        {
            return new ModelA { Foo = "foo" };
        }
    }
}

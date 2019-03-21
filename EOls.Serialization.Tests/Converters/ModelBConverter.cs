using EOls.Serialization.Services.ConverterLocator;
using EOls.Serialization.Tests.Models;

namespace EOls.Serialization.Tests.Converters
{
    public class ModelBConverter : IObjectConverter<ModelB>
    {
        public object Convert(ModelB target)
        {
            return new ModelA { Foo = "foo" };
        }
    }
}

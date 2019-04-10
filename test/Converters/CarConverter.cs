using JsonContractSimplifier.Services.ConverterLocator;
using JsonContractSimplifier.Tests.Models;

namespace JsonContractSimplifier.Tests.Converters
{
    public class CarConverter : IObjectConverter<Car>
    {
        public object Convert(Car target)
        {
            return new Volvo(target) { A = "foo", C = "bar" };            
        }
    }
}

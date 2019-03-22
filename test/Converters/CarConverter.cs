using EOls.Serialization.Services.ConverterLocator;
using EOls.Serialization.Tests.Models;

namespace EOls.Serialization.Tests.Converters
{
    public class CarConverter : IObjectConverter<Car>
    {
        public object Convert(Car target)
        {
            return new Volvo(target) { A = "foo", C = "bar" };            
        }
    }
}

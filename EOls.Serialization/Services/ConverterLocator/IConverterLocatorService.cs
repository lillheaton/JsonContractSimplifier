using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOls.Serialization.Services.ConverterLocator
{
    public interface IConverterLocatorService
    {
        Type[] LoadConverters();
        bool TryFindConverterFor(Type type, out IConverter converter);
    }
}

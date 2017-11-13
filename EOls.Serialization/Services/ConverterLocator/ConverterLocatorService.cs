using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOls.Serialization.Services.ConverterLocator
{
    public class ConverterLocatorService : IConverterLocatorService
    {
        public Type[] LoadConverters()
        {
            throw new NotImplementedException();
        }

        public bool TryFindConverterFor(Type type, out IConverter converter)
        {
            throw new NotImplementedException();
        }
    }
}

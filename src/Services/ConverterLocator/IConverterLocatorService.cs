using System;

namespace JsonContractSimplifier.Services.ConverterLocator
{
    public interface IConverterLocatorService
    {
        Type[] LoadConverters();
        bool TryFindConverterFor(Type type, out IConverter converter);
        object Convert(object target, IConverter converter);
    }
}

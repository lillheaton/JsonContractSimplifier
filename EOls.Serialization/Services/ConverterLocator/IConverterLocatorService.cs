using System;

namespace EOls.Serialization.Services.ConverterLocator
{
    public interface IConverterLocatorService
    {
        Type[] LoadConverters();
        bool TryFindConverterFor(Type type, out IConverter converter);
        object Convert(object target, IConverter converter);
    }
}

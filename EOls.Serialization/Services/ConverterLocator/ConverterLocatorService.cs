using EOls.Serialization.Services.Reflection;
using System;
using System.Linq;

namespace EOls.Serialization.Services.ConverterLocator
{
    public class ConverterLocatorService : IConverterLocatorService
    {
        private readonly (IConverter Converter, Type Target)[] _converters;
        
        public ConverterLocatorService()
        {
            _converters = LoadConverters()
                .Select(x => 
                    (
                        Activator.CreateInstance(x) as IConverter,
                        ReflectionService.GetGenericTypeOfInterface(x)
                    ))
                .ToArray();
        }

        public Type[] LoadConverters()
        {
            return ReflectionService
                .GetAssemblyClassesInheritGenericInterface(
                    typeof(IObjectConverter<>), 
                    AppDomain.CurrentDomain.GetAssemblies())
                .ToArray();
        }

        public bool TryFindConverterFor(Type type, out IConverter converter)
        {
            bool exist = _converters.Any(x => x.Target == type);

            if (!exist)
            {
                converter = null;
                return false;
            }

            converter = _converters.First(x => x.Target == type).Converter;
            return true;
        }
    }
}

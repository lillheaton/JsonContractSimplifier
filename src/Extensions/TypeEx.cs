using System;
using System.Linq;

namespace JsonContractSimplifier
{
    public static class TypeEx
    {
        public static bool ImplementsGenericInterface(this Type type, Type genericInterfaceType)
        {
            return type
                .GetInterfaces()
                .Any(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition() == genericInterfaceType);
        }

        public static bool IsClassObject(this Type type)
        {
            if (type == null)
                return false;
            
            return type.IsClass && type != typeof(string) && type.IsValueType == false;
        }
    }
}

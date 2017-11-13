using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EOls.Serialization.Services.Reflection
{
    public static class ReflectionService
    {
        /// <summary>
        /// Get all classes that inherit interface type in assembly
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblyClassesInheritGenericInterface(Type type, Assembly assembly)
        {
            return
                assembly
                .GetTypes()
                .Where(
                    x => x.IsClass &&
                    x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type));
        }

        /// <summary>
        /// Get all classes that inherit interface type in assemblies
        /// </summary>
        /// <typeparam name="type"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAssemblyClassesInheritGenericInterface(Type type, IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(x => GetAssemblyClassesInheritGenericInterface(type, x));
        }

        /// <summary>
        /// Gets generic type of interface<T>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetGenericTypeOfInterface(Type type)
        {
            return type.GetInterfaces().FirstOrDefault(i => i.IsGenericType)?.GetGenericArguments()[0];
        }
    }
}
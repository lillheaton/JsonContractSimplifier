using System;

namespace JsonContractSimplifier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class JsonCacheGetterAttribute : Attribute
    {
        public TimeSpan CacheTime { get; }

        public JsonCacheGetterAttribute(string timespan)
        {
            CacheTime = TimeSpan.Parse(timespan);
        }
    }
}

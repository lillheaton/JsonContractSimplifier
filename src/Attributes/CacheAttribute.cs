using System;

namespace JsonContractSimplifier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false)]
    public class CacheAttribute : Attribute
    {
        public TimeSpan CacheTime { get; }

        public CacheAttribute(string timespan)
        {
            CacheTime = TimeSpan.Parse(timespan);
        }
    }
}

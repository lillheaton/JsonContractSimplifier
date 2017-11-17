using System;

namespace EOls.Serialization.Attributes
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

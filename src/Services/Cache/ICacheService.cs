using System;

namespace JsonContractSimplifier.Services.Cache
{
    public interface ICacheService
    {
        bool TryGet<T>(string key, out T value);
        void Set(string key, object value, TimeSpan timespan);

        T HandleCache<T>(string key, Func<T> func);
        T HandleCache<T>(string key, TimeSpan timeSpan, Func<T> func);

        void ClearAll();
    }
}

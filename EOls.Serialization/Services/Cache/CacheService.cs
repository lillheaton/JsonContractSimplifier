using System;
using System.Runtime.Caching;

namespace EOls.Serialization.Services.Cache
{
    public class CacheService : ICacheService
    {
        public T HandleCache<T>(string key, Func<T> func)
        {
            return HandleCache(key, TimeSpan.FromMinutes(10), func);
        }
        public T HandleCache<T>(string key, TimeSpan timeSpan, Func<T> func)
        {
            T target;
            if (TryGet(key, out target))
            {
                return target;
            }

            target = func();

            Set(key, target, timeSpan);
            return target;
        }

        public void Set(string key, object value, TimeSpan timeSpan)
        {
            MemoryCache.Default.Set(
                key,
                value,
                new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now + timeSpan
                });
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);

            if (!MemoryCache.Default.Contains(key))
                return false;

            value = (T)MemoryCache.Default.Get(key);
            return true;
        }
    }
}

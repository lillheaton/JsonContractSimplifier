using System;

namespace JsonContractSimplifier.Services.Cache
{
    public class NotImplementedCache : ICacheService
    {
        public void ClearAll()
        {
            throw new NotImplementedException();
        }

        public T HandleCache<T>(string key, Func<T> func)
        {
            throw new NotImplementedException();
        }
        public T HandleCache<T>(string key, TimeSpan timeSpan, Func<T> func)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public bool TryGet<T>(string key, out T value)
        {
            throw new NotImplementedException();
        }
    }
}

using EOls.Serialization.Attributes;
using EOls.Serialization.Services.Cache;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace EOls.Serialization.ValueProviders
{
    public class CacheValueProvider : IValueProvider
    {
        private const string CACHE_KEY = "EOls.Serialization.Cache_{0}";

        private readonly MemberInfo _memberInfo;
        private readonly IValueProvider _innerValueProvider;
        private readonly ICacheService _cacheService;
        
        public CacheValueProvider(
            MemberInfo memberInfo,
            IValueProvider valueProvider,
            ICacheService cacheService)
        {
            _memberInfo = memberInfo;
            _innerValueProvider = valueProvider;
            _cacheService = cacheService;
        }        

        private string GetCacheKey() =>
            string.Format(CACHE_KEY, $"{_memberInfo.DeclaringType.ToString()}_{_memberInfo.Name}");

        public object GetValue(object target)
        {
            var prop = target.GetType().GetProperty(_memberInfo.Name);
            var cacheAttribute = prop.GetCustomAttribute<CacheAttribute>();

            if (cacheAttribute == null)
                return _innerValueProvider.GetValue(target);            

            return _cacheService.HandleCache<object>(
                GetCacheKey(),
                cacheAttribute.CacheTime,
                () =>
                {
                    return _innerValueProvider.GetValue(target);
                });
        }

        public void SetValue(object target, object value)
        {
            _innerValueProvider.SetValue(target, value);
        }
    }
}
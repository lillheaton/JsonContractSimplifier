using JsonContractSimplifier.Attributes;
using JsonContractSimplifier.Services.Cache;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace JsonContractSimplifier.ValueProviders
{
    public class CacheValueProvider : IValueProvider
    {
        private const string ROOT_CACHE_KEY = "JsonContractSimplifier.Cache_{0}";

        private readonly string _cacheKey;
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

            _cacheKey = string.Format(ROOT_CACHE_KEY, $"{_memberInfo.DeclaringType.ToString()}_{_memberInfo.Name}");
        }        
        
        public object GetValue(object target)
        {
            var prop = target.GetType().GetProperty(_memberInfo.Name);
            var cacheAttribute = prop.GetCustomAttribute<JsonCacheGetterAttribute>();

            if (cacheAttribute == null)
                return _innerValueProvider.GetValue(target);            

            return _cacheService.HandleCache<object>(
                _cacheKey,
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
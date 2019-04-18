using JsonContractSimplifier.Attributes;
using JsonContractSimplifier.Services.Cache;
using JsonContractSimplifier.Services.ConverterLocator;
using JsonContractSimplifier.ValueProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonContractSimplifier
{
    public class ContractResolver : DefaultContractResolver
    {
        private readonly ICacheService _cacheService;
        private readonly IConverterLocatorService _converterLocatorService;

        public Type[] ExtraOptInAttributes { get; set; }
        public bool ShouldCache { get; set; } = true;

        public ContractResolver(
            IConverterLocatorService converterLocatorService, 
            ICacheService cacheService)
        {
            _cacheService = cacheService;
            _converterLocatorService = converterLocatorService;

            this.NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = true,
                OverrideSpecifiedNames = true
            };
        }

        public ContractResolver(ICacheService cacheService) : this(
            new ConverterLocatorService(),
            cacheService)
        {
        }

        public ContractResolver() : this(
            new ConverterLocatorService(), 
            new NotImplementedCache())
        {
        }

        private IValueProvider GetValueProvider(JsonProperty jsonProperty, MemberInfo memberInfo)
        {
            if (!ShouldCache)
                return jsonProperty.ValueProvider;

            var cacheAttribute = memberInfo.GetCustomAttribute<JsonCacheGetterAttribute>(false);
            if (cacheAttribute != null)
                return new CacheValueProvider(memberInfo, jsonProperty.ValueProvider, _cacheService);

            return jsonProperty.ValueProvider;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            var contact = base.CreateContract(objectType);

            // this will only be called once and then cached        
            if (_converterLocatorService.TryFindConverterFor(objectType, out IConverter converter))
            {
                contact.Converter = new TargetsJsonConverter(converter, _converterLocatorService);
            }

            return contact;
        }

        protected virtual bool ShouldIgnore(JsonProperty jsonProperty, MemberInfo member, MemberSerialization memberSerialization)
        {
            if (ExtraOptInAttributes == null)
                return jsonProperty.Ignored;

            if (memberSerialization == MemberSerialization.OptIn && jsonProperty.Ignored)
            {
                var attributes = jsonProperty
                    .AttributeProvider
                    .GetAttributes(false);
                
                if(attributes.Any(attr => attr.GetType() == typeof(JsonIgnoreAttribute)))
                {
                    return jsonProperty.Ignored;
                }
                
                return !attributes
                    .Any(attr => 
                        ExtraOptInAttributes.Any(x => attr.GetType().Equals(x))
                    );
            }

            return jsonProperty.Ignored;
        }
        
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {            
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            jsonProperty.Ignored = ShouldIgnore(jsonProperty, member, memberSerialization);
            jsonProperty.ValueProvider = GetValueProvider(jsonProperty, member);
            return jsonProperty;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {            
            return base.CreateProperties(type, memberSerialization);
        }
    }
}

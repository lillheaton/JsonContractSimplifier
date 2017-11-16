using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace EOls.Serialization
{

    //https://www.newtonsoft.com/json/help/html/Performance.htm
    //https://www.newtonsoft.com/json/help/html/CustomContractResolver.htm
    //https://stackoverflow.com/questions/39816736/json-net-custom-valueprovider-to-convert-objects-into-guid
    //https://stackoverflow.com/questions/33148957/replace-sensitive-data-value-on-json-serialization

    public class ContractResolver : DefaultContractResolver
    {
        public Type[] ExtraOptInAttributes { get; set; }

        public ContractResolver()
        {
            this.NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = true,
                OverrideSpecifiedNames = true
            };
        }

        private bool ShouldIgnore(JsonProperty prop, MemberSerialization memberSerialization)
        {
            if (ExtraOptInAttributes == null)
                return prop.Ignored;

            if (memberSerialization == MemberSerialization.OptIn)
            {
                return prop.AttributeProvider
                    .GetAttributes(false)
                    .Any(attr => 
                        ExtraOptInAttributes
                        .Any(x => attr.Equals(x)));
            }

            return prop.Ignored;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            prop.Ignored = ShouldIgnore(prop, memberSerialization);
            return prop;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {            
            return base.CreateProperties(type, memberSerialization);
        }
    }
}

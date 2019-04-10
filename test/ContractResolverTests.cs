using EOls.Serialization.Attributes;
using EOls.Serialization.Tests.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace EOls.Serialization.Tests
{
    [TestClass]
    public class ContractResolverTests
    {
        private static JsonSerializerSettings _jsonSettings;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new ContractResolver(new TestCacheService())
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            ((ContractResolver)_jsonSettings.ContractResolver).ExtraOptInAttributes = new Type[0];
        }

        [TestMethod]
        public void ContractResolver_Without_Custom_OptIn_Attributes_Should_Be_Null()
        {
            var model = new Foo { Bar = "bar", Test = "test" };

            var json = JsonConvert.SerializeObject(model, _jsonSettings);
            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.IsNull(obj.bar);
        }

        [TestMethod]
        public void ContractResolver_With_Custom_OptIn_Attributes_Should_Not_Be_Null()
        {
            var model = new Bar { Foo = "foo", Foobar = "test", Ignored = "ignored" };

            ((ContractResolver)_jsonSettings.ContractResolver).ExtraOptInAttributes = new[] { typeof(DisplayAttribute) };
            var json = JsonConvert.SerializeObject(model, _jsonSettings);
            var obj = JsonConvert.DeserializeObject<JObject>(json);
            
            Assert.IsNotNull(obj["foo"]);
            Assert.IsTrue(obj["ignored"] == null);
        }

        [TestMethod]
        public void ContractResolver_CacheAttribute_Should_Cache_Property()
        {
            var target = DateTime.Now.AddDays(-1);
            var model = new Foo();            

            var json1 = JsonConvert.SerializeObject(model, _jsonSettings);
            var obj1 = JsonConvert.DeserializeObject<dynamic>(json1);

            Thread.Sleep(1000);

            var json2 = JsonConvert.SerializeObject(model, _jsonSettings);
            var obj2 = JsonConvert.DeserializeObject<dynamic>(json2);

            Assert.AreNotEqual(obj1.noCacheDate, obj2.noCacheDate);
            Assert.AreEqual(obj1.cacheDate, obj2.cacheDate);
        }

        [JsonObject(MemberSerialization.OptIn)]
        internal class Foo
        {
            [Display]
            public string Bar { get; set; }

            [JsonProperty]
            public string Test { get; set; }

            [JsonProperty]
            public DateTime NoCacheDate { get => DateTime.Now; }

            [Cache("00:01")]
            [JsonProperty]
            public DateTime CacheDate { get => DateTime.Now; }
        }

        [JsonObject(MemberSerialization.OptIn)]
        internal class Bar
        {
            [Display]
            public string Foo { get; set; }

            [JsonProperty]
            public string Foobar { get; set; }
            
            public string Ignored { get; set; }            
        }
    }
}

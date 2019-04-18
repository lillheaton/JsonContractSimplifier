using JsonContractSimplifier.Services.ConverterLocator;
using JsonContractSimplifier.Tests.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace JsonContractSimplifier.Tests
{
    public class SubModelBItemConverter : IObjectConverter<SubModelBItem>
    {
        public object Convert(SubModelBItem target)
        {
            return "converted";
        }
    }

    [TestClass]
    public class ComplexObjectsTests
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
        public void Should_Handle_Complex_Structures()
        {
            var model = new ComplexObjectsTestsModelA
            {
                DictModels = new Dictionary<string, object>()
                {
                    {
                        "foo",
                        new SubModelModelA
                        {
                            SubModelModelB = new SubModelModelB
                            {
                                Items = new SubModelBItem[]{ new SubModelBItem { Foo = "foo", Bar = "bar" } }
                            }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(model, _jsonSettings);
            var obj = JsonConvert.DeserializeObject<JObject>(json);

            Assert.AreEqual("converted", obj["dictModels"]["foo"]["subModelModelB"]["items"][0]);
        }
    }

    public class ComplexObjectsTestsModelA
    {
        public Dictionary<string, object> DictModels { get; set; }
    }

    public class SubModelModelA
    {
        public SubModelModelB SubModelModelB { get; set; }
    }

    public class SubModelModelB
    {
        public SubModelBItem[] Items { get; set; }
    }

    public class SubModelBItem
    {
        public string Foo { get; set; }
        public string Bar { get; set; }
    }
}

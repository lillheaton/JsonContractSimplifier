using EOls.Serialization.Tests.Models;
using EOls.Serialization.Tests.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EOls.Serialization.Tests
{
    [TestClass]
    public class JsonWriterConverterTests
    {
        private static JsonSerializerSettings _jsonSettings;
        private CarCompany _carCompany;

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
               _carCompany = new CarCompany
            {
                Cars = new[] {
                    new Car
                    {
                        Brand = "Saab",
                        Passengers = new[] {
                            new Person
                            {
                                Name = "Foo",
                                Age = 10
                            }
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void Converter_Should_Result_Valid_Json()
        {
            var json = JsonConvert.SerializeObject(_carCompany, _jsonSettings);
            var obj = JsonConvert.DeserializeObject<JObject>(json);

            Assert.AreEqual("Saab", obj["cars"][0]["brand"].ToString());
        }

        [TestMethod]
        public void CarConverter_Should_Transform_To_Volvo()
        {
            var json = JsonConvert.SerializeObject(_carCompany, _jsonSettings);
            var obj = JsonConvert.DeserializeObject<JObject>(json);

            Assert.AreEqual("bar", obj["cars"][0]["c"].ToString());
        }

        [TestMethod]
        public void CarConverter_Should_Transform_To_Volvo_But_Keep_Newtonsoft_Setting()
        {
            var json = JsonConvert.SerializeObject(_carCompany, _jsonSettings);
            var obj = JsonConvert.DeserializeObject<JObject>(json);

            Assert.AreEqual("foo", obj["cars"][0]["b"].ToString());
        }

        [TestMethod]
        public void Should_Be_Able_To_Handle_Dictionaries()
        {
            var json = JsonConvert.SerializeObject(new
            {
                DictTest1 = new Dictionary<string, object>
                {
                    { "A", new { DictTest1Foo = "foo" } }
                },
                DictTest2 = new Dictionary<string, object>
                {
                    { "B", new ModelA { Foo = "will get removed" } }
                }
            }, _jsonSettings);

            var obj = JsonConvert.DeserializeObject<JObject>(json);
            Assert.AreEqual("Converted", obj["dictTest2"]["b"].ToString());
        }

        [TestMethod]
        public void Should_Be_Able_To_Handle_KeyValuePair()
        {
            var json = JsonConvert.SerializeObject(new
            {
                DictTest1 = new Dictionary<string, object>
                {
                    { "A", new { DictTest1Foo = "foo" } }
                },
                DictTest2 = new Dictionary<string, object>
                {
                    { "B", new KeyValuePair<string, object>("Test", new ModelA { Foo = "will get removed" }) }
                }
            }, _jsonSettings);
            
            var obj = JsonConvert.DeserializeObject<JObject>(json);
            Assert.AreEqual("Converted", obj["dictTest2"]["b"]["value"].ToString());
        }

        [TestMethod]
        public void Should_Be_Able_To_Handle_IEnumerable()
        {
            var json = JsonConvert.SerializeObject(new
            {
                Enumerable = new Dictionary<string, object>
                {
                    { "A", new { Test = new List<ModelA> { new ModelA { Foo = "will get removed" } } } }
                }
            }, _jsonSettings);

            var obj = JsonConvert.DeserializeObject<JObject>(json);
            Assert.AreEqual("Converted", obj["enumerable"]["a"]["test"][0].ToString());
        }

        [TestMethod]
        public void Should_Deep_Transform()
        {
            var json = JsonConvert.SerializeObject(new
            {
                Test = new ModelB { Bar = "Should be converted to ModelA then to string" },
                Test2 = new [] { new ModelB { Bar = "Should be converted" } }
            }, _jsonSettings);

            var obj = JsonConvert.DeserializeObject<JObject>(json);
            Assert.AreEqual("Converted", obj["test"].ToString());
            Assert.AreEqual("Converted", obj["test2"][0].ToString());
        }
    }
}

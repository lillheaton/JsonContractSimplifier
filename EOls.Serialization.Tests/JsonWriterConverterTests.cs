using EOls.Serialization.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EOls.Serialization.Tests
{
    [TestClass]
    public class JsonWriterConverterTests
    {
        private CarCompany _carCompany;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = new[] { new TargetsJsonConverter() }
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
            var json = JsonConvert.SerializeObject(_carCompany);
            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.AreEqual("Saab", obj.Cars[0].Brand.ToString());
        }

        [TestMethod]
        public void CarConverter_Should_Transform_To_Volvo()
        {
            var json = JsonConvert.SerializeObject(_carCompany);
            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.AreEqual("bar", obj.Cars[0].C.ToString());
        }

        [TestMethod]
        public void CarConverter_Should_Transform_To_Volvo_But_Keep_Newtonsoft_Setting()
        {
            var json = JsonConvert.SerializeObject(_carCompany);
            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.AreEqual("foo", obj.Cars[0].B.ToString());
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
            });

            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            Assert.AreEqual("Converted", obj.DictTest2.B.ToString());
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
            });
            
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            Assert.AreEqual("Converted", obj.DictTest2.B.Value.ToString());
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
            });

            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            Assert.AreEqual("Converted", obj.Enumerable.A.Test[0].ToString());
        }

        [TestMethod]
        public void Should_Deep_Transform()
        {
            var json = JsonConvert.SerializeObject(new
            {
                Test = new ModelB { Bar = "Should be converted to ModelA then to string" },
                Test2 = new [] { new ModelB { Bar = "Should be converted" } }
            });

            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            Assert.AreEqual("Converted", obj.Test.ToString());
            Assert.AreEqual("Converted", obj.Test2[0].ToString());
        }
    }
}

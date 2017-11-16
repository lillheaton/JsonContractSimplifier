using EOls.Serialization.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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
                Converters = new[] { new SimplifyJsonConverter() }
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
    }
}

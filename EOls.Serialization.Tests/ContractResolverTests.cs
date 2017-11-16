using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EOls.Serialization.Tests
{
    [TestClass]
    public class ContractResolverTests
    {        
        [TestMethod]
        public void ContractResolver_Without_Custom_OptIn_Attributes_Should_Be_Null()
        {
            var model = new Foo { Bar = "bar", Test = "test" };

            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new ContractResolver()
            });
            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.IsNull(obj.bar);
        }

        [TestMethod]
        public void ContractResolver_With_Custom_OptIn_Attributes_Should_Not_Be_Null()
        {
            var model = new Foo { Bar = "bar", Test = "test" };

            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new ContractResolver()
                {
                    ExtraOptInAttributes = new[] { typeof(DisplayAttribute) }
                }
            });
            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.IsNotNull(obj.bar);
        }

        [JsonObject(MemberSerialization.OptIn)]
        internal class Foo
        {
            [Display]
            public string Bar { get; set; }

            [JsonProperty]
            public string Test { get; set; }
        }
    }
}

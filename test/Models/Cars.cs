using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonContractSimplifier.Tests.Models
{
    public class CarCompany
    {
        public Car[] Cars { get; set; }        
    }

    public class Car
    {
        public string Brand { get; set; }
        public Person[] Passengers { get; set; }
    }
    
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Person
    {
        [JsonProperty]
        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class Volvo : Car
    {
        [JsonProperty(PropertyName = "B")]
        public string A { get; set; }

        public string C { get; set; }

        public Volvo(Car car)
        {
            this.Brand = car.Brand;
            this.Passengers = car.Passengers;
        }
    }
}

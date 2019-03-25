[![Build status](https://ci.appveyor.com/api/projects/status/0o92jsl3gutgi2lu?svg=true)](https://ci.appveyor.com/project/lillheaton/eols-serialization)
[![NuGet](https://img.shields.io/nuget/v/EOls.Serialization.svg)](https://www.nuget.org/packages/EOls.Serialization/)


# Eols.Serialization
A shorthand tool built on top of Newtonsoft [Json.NET](https://www.newtonsoft.com/json) that allows you to transform specific types during serialization. 
It builds on top of the default JsonConverter as well as the DefaultContractResolver.

### Installation
    PM> Install-Package EOls.Serialization

### Usage

##### Setup
```C#
var json = JsonConvert.SerializeObject(yourObject, new JsonSerializerSettings
{
    ContractResolver = new ContractResolver() { ShouldCache = false },
    Converters = new[] { new TargetsJsonConverter() }
});

```

##### Converter
```C#
public class AppleConverter : IObjectConverter<Apple>
{
    public object Convert(Apple target)
    {
        return new Pear(target) { A = "foo", C = "bar" };
    }
}

```

##### Cache
TODO - more info

During serialization - by using the contract resolver, we create a new value provider based on the property attribute.
This attribute allows you to cache that specific property in X time.

```C#
internal class Foo
{
    [Cache("00:01")]    
    public DateTime CacheDate { get => DateTime.Now; }
}

```

To be able to cache you need to implement ICacheService and pass to the contract resolver.

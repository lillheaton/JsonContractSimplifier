[![Build status](https://ci.appveyor.com/api/projects/status/wx2hg990jha97yx7?svg=true)](https://ci.appveyor.com/project/lillheaton/jsoncontractsimplifier)

[![NuGet](https://img.shields.io/nuget/v/JsonContractSimplifier.svg)](https://www.nuget.org/packages/JsonContractSimplifier/)


# JsonContractSimplifier
A shorthand tool built on top of Newtonsoft [Json.NET](https://www.newtonsoft.com/json) that allows you to transform specific types during serialization. 
It builds on top of the default JsonConverter as well as the DefaultContractResolver.

### Installation
    PM> Install-Package JsonContractSimplifier

### Usage

##### Setup

```C#
var json = JsonConvert.SerializeObject(yourObject, new JsonSerializerSettings
{
    ContractResolver = new ContractResolver() { ShouldCache = false }
});

```

##### Converter
If you have a specific type you want to convert you can create a IObjectConverter<>. During serialization it will find all classes in the current AppDomain assemblies and try to find a matching target type.

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
During serialization - by using the contract resolver, we create a new value provider based on if the property is decorated with the "Cache attribute".
This attribute allows you to cache that specific property for X time.

```C#
public class Foo
{
    [Cache("00:01")]    
    public DateTime CacheDate { get => DateTime.Now; }
}

```

<b>Note!</b> To be able to cache you need to implement <b>ICacheService</b> and pass to the contract resolver.
Once you have implemented the ICacheService you can pass it to the ContractResolver and set ShouldCache = true.

##### Attributes
Json.NET allow you to decorate a class with [JsonObject(MemberSerialization.OptIn)] to specify that only properties that have been explicitly specified with JsonPropertyAttribute should be serialized.

This tool allows you to extend this feature by adding more attributes that will also be optIn attributes.

```C#
new ContractResolver(new YourCacheService())
{
    ExtraOptInAttributes = new[] { typeof(YourAttribute) } // If property is decorated with YourAttribute it will serialize this as well.
}

```
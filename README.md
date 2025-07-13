# CsDictMapper
Convert between `IDictionary<string,object?>` and C# object.

AOT friendly, Free of reflection.

**⚠This project has no release version yet and everything may change. If you want to use it, we recommand you to clone the code and reference the source code directly.**

# Usage

## Config the mapper

```cs
using Tsinswreng.CsDictMapper;

namespace MyProjectNamespace; // don't put things in global namespace otherwise it may not work

public class Person{
	public int Age{get;set;}
	public string Name {get;set;}
}

[DictType(typeof(Person))]
//[DictType(typeof(MoreTypes))]
public partial class AppDictMapper{
	public static AppDictMapper Inst{get;} = new();

}
```

## Use the mapper

```cs
using Tsinswreng.CsDictMapper;
// object to dictionary:
var person = new Person{Age=25, Name="Tsinswreng"};
var dict = AppDictMapper.Inst.ToDictShallowT(person);
// the dict will be {"Age":25, "Name":"Tsinswreng"}

// assign the dictionary to object:
var person2 = new Person();
AppDictMapper.Inst.AssignShallowT(person2, dict);
// the content of `person2` will be the same as `person`

```

# Reference this project by source code
+ Clone the repo
+ In your .csproj file:
```xml
<ItemGroup>
	<!-- reference the source generator -->
	<ProjectReference Include="../Tsinswreng.CsDictMapper/proj/Tsinswreng.CsDictMapper.SrcGen/Tsinswreng.CsDictMapper.SrcGen.csproj"
		OutputItemType="Analyzer" ReferenceOutputAssembly="false"
	/>

	<!-- reference the API, including Attributes, interfaces etc. -->
	<ProjectReference Include="../Tsinswreng.CsDictMapper/proj/Tsinswreng.CsDictMapper/Tsinswreng.CsDictMapper.csproj" />

</ItemGroup>
```

# Documentation


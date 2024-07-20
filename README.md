# RSCG_Templating

Templating for generating everything from classes, methods from a Roslyn Code Generator

Templating is in SCRIBAN form

## How to use

Add reference to 

```xml
  <ItemGroup>
    <PackageReference Include="RSCG_Templating" Version="2024.720.1603" OutputItemType="Analyzer"  ReferenceOutputAssembly="false"   />
    <PackageReference Include="RSCG_TemplatingCommon" Version="2024.720.1603" />
  </ItemGroup>
<!-- this is just for debug purposes -->
<PropertyGroup>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\GX</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
<!-- those are the templates files, see IGenerateDataFromClass -->
  <ItemGroup>
    <AdditionalFiles Include="ClassTypeName.txt" />
    <AdditionalFiles Include="ClassPropByName.txt" />
  </ItemGroup>

```

Then add additional files , for example 
```scriban
//autogenerated by RSCG_Templating version {{data.Version}} from file {{fileName}}
namespace {{data.nameSpace}} {
	 
	partial class {{data.className}} {
		public string MyTypeName = "{{data.nameSpace}}.{{data.className}}";		
	}//end class

}//end namespace
```

Now add 

```csharp
//can have multiple attributes on partial classes
[IGenerateDataFromClass("ClassTypeName")]
public partial class Person
```

## Advanced uses

For the moment , RSCG_Templating generates definition for a class with properties + methods .
See example for generating enum from properties and setting properties by name

```csharp
var x = new Person();
Console.WriteLine("The generated string type is "+x.MyTypeName);
x.FirstName = "Andrei";
//set last name via prop
x.SetPropValue(ePerson_Properties.LastName, "Ignat");
Console.WriteLine("called directly first name : " + x.FirstName);
Console.WriteLine("called via enum of prop first name : " + x.GetPropValue(ePerson_Properties.FirstName));
Console.WriteLine("called get property :" + x.GetPropValue(ePerson_Properties.Name));
```

See example at https://github.com/ignatandrei/RSCG_Templating/tree/main/src/RSCG_Templating

## More templates


10. Template for having the class type name: ClassTypeName
20. Template for having the class properties as enum : ClassPropByName
30. Template for setting properties after name : ClassPropByName
40. Template from DebuggerDisplay for properties: ClassDebuggerDisplay
50. Template for generating interface from class : ClassInterface

# More Roslyn Source Code Generators

You can find more RSCG with examples at [Roslyn Source Code Generators](https://ignatandrei.github.io/RSCG_Examples/v2/)

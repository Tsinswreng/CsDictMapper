#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Tsinswreng.SrcGen.Tools;

namespace Tsinswreng.SrcGen.Dict.CodeGenerator.Ctx;


public class Ctx_TargetType{

	public Ctx_DictCtx Ctx_DictCtx{get;set;}
	public INamedTypeSymbol TypeSymbol{get;set;}
	public Ctx_TargetType(
		Ctx_DictCtx Ctx_DictCtx
		,INamedTypeSymbol TypeSymbol
	){
		this.Ctx_DictCtx = Ctx_DictCtx;
		this.TypeSymbol = TypeSymbol;
	}

	bool _Inited = false;
	public Ctx_TargetType Init(){
		if(_Inited){return this;}
		var typeSymbol = TypeSymbol;
		// PublicProps = typeSymbol.GetMembers()
		// 	.OfType<IPropertySymbol>()
		// 	.Where(
		// 		p => p.DeclaredAccessibility == Accessibility.Public
		// 		&& !p.IsStatic
		// 	)
		// ;//需有父類成員
		PublicProps = Tools.CodeTool.GetPropsWithParent(typeSymbol);
		_Inited = true;
		return this;
	}

	public IEnumerable<IPropertySymbol> PublicProps{get;set;} = null!;

}


public class GenTargetType{

	public Ctx_TargetType Ctx_TargetType{get;set;}

	public GenTargetType(){}
	public GenTargetType(Ctx_TargetType Ctx_TargetType){
		Init(Ctx_TargetType);
	}

	public GenTargetType Init(Ctx_TargetType Ctx_TargetType){
		this.Ctx_TargetType = Ctx_TargetType;
		Ctx_TargetType.Init();
		return this;
	}

	public str MkMethod_ToDict(){
		var typeSymbol = Ctx_TargetType.TypeSymbol;
		var properties = Ctx_TargetType.PublicProps;
		// Logger.Append((properties==null)+"");
		// Logger.Append((properties.Count())+"");
		var dictEntries = properties.Select(p =>
			$"""["{p.Name}"] = obj.{p.Name},""")
		;
		var N = Const_Name.Inst;
		var MethodCode =
$$"""
		public static Dictionary<string, object> {{N.ToDict}} ({{typeSymbol}} obj){
			return new Dictionary<string, object>{
{{string.Join("\n",dictEntries)}}
			};
		}
""";
		return MethodCode;
	}


	public str MkMethod_Assign(){
		var typeSymbol = Ctx_TargetType.TypeSymbol;
		var properties = Ctx_TargetType.PublicProps;
		var dictEntries = properties.Select(p =>
			//$""" o.{p.Name} = d["{p.Name}"] as {p.Type.ToDisplayString()}; """)
			$""" o.{p.Name} = ({p.Type.ToDisplayString()})d["{p.Name}"]; """)
		;
		var N = Const_Name.Inst;
		var MethodCode =
$$"""
		public static {{typeSymbol}} {{N.Assign}} ({{typeSymbol}} o, Dictionary<string, object> d){
{{string.Join("\n",dictEntries)}}
			return o;
		}
""";
		return MethodCode;
	}

	public str MkTypeCacheElseIf(){
		var TypeSymbol = Ctx_TargetType.TypeSymbol;
		var N = Const_Name.Inst;
		return
$$"""
else if(typeof(T) == typeof({{TypeSymbol}})){
	Fn_ToDict = (obj) => {
		return {{N.ToDict}}(({{TypeSymbol}})(object)obj);
	};
	Fn_Assign = (obj, dict) => {
		var o = ({{TypeSymbol}})(object)obj;
		{{N.Assign}}(o, dict);
		return obj;
	};
}
""";
	}

}

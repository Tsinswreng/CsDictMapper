#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Tsinswreng.CsSrcGen.Tools;

namespace Tsinswreng.CsSrcGen.Dict.CodeGenerator.Ctx;


public class CtxTargetType{

	public CtxDictCtx Ctx_DictCtx{get;set;}
	public INamedTypeSymbol TypeSymbol{get;set;}
	public CtxTargetType(
		CtxDictCtx Ctx_DictCtx
		,INamedTypeSymbol TypeSymbol
	){
		this.Ctx_DictCtx = Ctx_DictCtx;
		this.TypeSymbol = TypeSymbol;
	}

	bool _Inited = false;
	public CtxTargetType Init(){
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

	public CtxTargetType CtxTargetType{get;set;}

	public GenTargetType(){}
	public GenTargetType(CtxTargetType Ctx_TargetType){
		Init(Ctx_TargetType);
	}

	public GenTargetType Init(CtxTargetType Ctx_TargetType){
		this.CtxTargetType = Ctx_TargetType;
		Ctx_TargetType.Init();
		return this;
	}

	public str MkMethod_ToDict(){
		var typeSymbol = CtxTargetType.TypeSymbol;
		var properties = CtxTargetType.PublicProps;
		// Logger.Append((properties==null)+"");
		// Logger.Append((properties.Count())+"");
		var dictEntries = properties.Select(p =>
			$"""["{p.Name}"] = obj.{p.Name},""")
		;
		var N = Const_Name.Inst;
		var S = SymbolWithNamespace.Inst;
		var MethodCode =
$$"""
		public static {{S.IDictionary}}<string, {{S.ObjectN}}> {{N.ToDict}} ({{typeSymbol}} obj){
			return new {{S.Dictionary}}<string, {{S.ObjectN}}>{
{{string.Join("\n",dictEntries)}}
			};
		}
""";
		return MethodCode;
	}


	public str MkMethod_Assign(){
// 		IDictionary<str, int?> d = null;
// {var s = d.TryGetValue("v", out var v)? v : default;}
// { var s  = (long)(d.TryGetValue("FKeyType", out var Got)? Got: default) ; }

		var typeSymbol = CtxTargetType.TypeSymbol;
		var properties = CtxTargetType.PublicProps;
		var dictEntries = properties.Select(p =>
			//$""" o.{p.Name} = d["{p.Name}"] as {p.Type.ToDisplayString()}; """)
			//$""" o.{p.Name} = ({p.Type.ToDisplayString()})d["{p.Name}"]; """)
"{"+$""" o.{p.Name} = ({p.Type.ToDisplayString()})(d.TryGetValue("{p.Name}", out var Got)? Got: o.{p.Name}) ; """+"}")
		;
		var N = Const_Name.Inst;
		var S = SymbolWithNamespace.Inst;
		var MethodCode =
$$"""
		public static {{typeSymbol}} {{N.Assign}} ({{typeSymbol}} o, {{S.IDictionary}}<string, {{S.ObjectN}}> d){
{{string.Join("\n",dictEntries)}}
			return o;
		}
""";
		return MethodCode;
	}

	public str MkTypeCacheElseIf(){
		var TypeSymbol = CtxTargetType.TypeSymbol;
		var N = Const_Name.Inst;
		var S = SymbolWithNamespace.Inst;
		return
$$"""
else if(typeof(T) == typeof({{TypeSymbol}})){
	Fn_ToDict = (obj) => {
		return {{N.ToDict}}(({{TypeSymbol}})({{S.ObjectN}})obj);
	};
	Fn_Assign = (obj, dict) => {
		var o = ({{TypeSymbol}})({{S.ObjectN}})obj;
		{{N.Assign}}(o, dict);
		return obj;
	};
}
""";
	}

}

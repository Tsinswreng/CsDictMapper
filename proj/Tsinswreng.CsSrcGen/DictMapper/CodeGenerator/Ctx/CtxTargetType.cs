#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Tsinswreng.CsSrcGen.Tools;

namespace Tsinswreng.CsSrcGen.DictMapper.CodeGenerator.Ctx;


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




	public str MkClsBody_DictMapperForOneType(){
		var typeSymbol = CtxTargetType.TypeSymbol;
		var properties = CtxTargetType.PublicProps;
var N = ConstName.Inst;
var S = SymbolWithNamespace.Inst;
//var ClsName = "DictMapperForOneType";
// public class {{ClsName}} : {{N.NsDict}}.{{N.IDictForOneType}}{
// protected static {{ClsName}}? _Inst = null;
// public static {{ClsName}} Inst => _Inst??= new {{ClsName}}();
var R = $$"""


	public virtual Type {{N.TargetType}} {get;} = typeof({{CodeTool.ResolveFullTypeFitsTypeof(CtxTargetType.TypeSymbol)}});

	public virtual {{S.IDictionary}}<string, object?> ToDict(object obj){
		if(obj is not {{typeSymbol}} o){
			throw new ArgumentException("obj is not of type {{typeSymbol}}");
		}
		return Static.{{N.ToDict}}(o);
	}

	public virtual {{S.IDictionary}} {{N.GetTypeDict}}(){
		return Static.{{N.GetTypeDict}}(null);
	}

	public virtual object {{N.Assign}}(object obj, {{S.IDictionary}}<string, object?> dict){
		if(obj is not {{typeSymbol}} o){
			throw new ArgumentException("obj is not of type {{typeSymbol}}");
		}
		return Static.{{N.Assign}}(o, dict);
	}

	public class Static{
		{{MkStaticMethod_GetTypeDictWithParam()}}
		{{MkStaticMethod_ToDict()}}
		{{MkStaticMethod_Assign()}}
	}

""";
		return R;
	}

	public str MkStaticMethod_GetTypeDictWithParam(){
		var typeSymbol = CtxTargetType.TypeSymbol;
		var properties = CtxTargetType.PublicProps;
		// Logger.Append((properties==null)+"");
		// Logger.Append((properties.Count())+"");
		var dictEntries = properties.Select(p =>{
			var TypeName = CodeTool.ResolveFullTypeFitsTypeof(p.Type);
			return $"""["{p.Name}"] = typeof({TypeName}),""";
		});

		var N = ConstName.Inst;
		var S = SymbolWithNamespace.Inst;
		var MethodCode =
$$"""
		public static {{S.IDictionary}}<string, {{S.Type}}> {{N.GetTypeDict}}({{typeSymbol}}? obj){//只蔿函數褈載、實非需傳此參數
			return new {{S.Dictionary}}<string, {{S.Type}}>{
{{string.Join("\n",dictEntries)}}
			};
		}
""";
		return MethodCode;
	}

	public str MkStaticMethod_ToDict(){
		var typeSymbol = CtxTargetType.TypeSymbol;
		var properties = CtxTargetType.PublicProps;
		// Logger.Append((properties==null)+"");
		// Logger.Append((properties.Count())+"");
		var dictEntries = properties.Select(p =>
			$"""["{p.Name}"] = obj.{p.Name},""")
		;
		var N = ConstName.Inst;
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


	public str MkStaticMethod_Assign(){
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
		var N = ConstName.Inst;
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

	public str MkTypeCacheDictAdd(){
		var TypeSymbol = CtxTargetType.TypeSymbol;
		var TypeName = CodeTool.ResolveFullTypeFitsTypeof(TypeSymbol);
		var TargetNs = CtxTargetType.TypeSymbol.ContainingNamespace;//TODO 測試頂層命名空間
		var N = ConstName.Inst;
		var S = SymbolWithNamespace.Inst;
		return
$$"""
{{N.Type_Mapper}}.Add(
	typeof({{TypeName}})
	,{{TargetNs}}.{{N.DictMapper}}.Inst
);
""";
	}

[Obsolete("見MkTypeCacheDictAdd")]
	public str MkTypeCacheElseIf(){
		var TypeSymbol = CtxTargetType.TypeSymbol;
		var N = ConstName.Inst;
		var S = SymbolWithNamespace.Inst;
		return
$$"""
else if(typeof(T) == typeof({{TypeSymbol}})){
	{{N.FnToDict}} = (obj) => {
		return {{N.ToDict}}(({{TypeSymbol}})({{S.ObjectN}})obj);
	};
	{{N.FnAssign}} = (obj, dict) => {
		var o = ({{TypeSymbol}})({{S.ObjectN}})obj;
		{{N.Assign}}(o, dict);
		return obj;
	};
	{{N.FnGetTypeDict}} = ()=>{
		var o = ({{TypeSymbol}})({{S.ObjectN}})null!;
		return {{N.GetTypeDict}}(o);
	};
}
""";
	}

}


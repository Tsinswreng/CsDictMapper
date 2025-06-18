#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Tsinswreng.CsDictMapper.Tools;

namespace Tsinswreng.CsDictMapper.Dict.CodeGenerator.Ctx;


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

/// <summary>
/// 解析完整ʹ類型名芝可置于typeof()中者
/// typeof()對于引用類型 則不支持帶可空問號 如typeof(string)合法洏typeof(string?)非法
/// 例:T 爲 int? 即返 System.Int32? ; T 潙 int 即返 System.Int32
/// T 潙 string 抑 string? 皆返 System.String
/// </summary>
/// <param name="T"></param>
/// <returns></returns>
	public static str ResolveFullTypeFitsTypeof(ITypeSymbol T){
		//if (T == null) return string.Empty;
		if (T.IsValueType){
			// 判断是否是 Nullable<T>
			if (T.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T){
				var namedType = (INamedTypeSymbol)T;
				var innerType = namedType.TypeArguments[0];
				// 返回可空值类型比如 "System.Int32?"
				return innerType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) + "?";
			}else{
				// 普通值类型，比如 "System.Int32"
				return T.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			}
		}//~if (T.IsValueType)
		else{
			// 引用类型，不加问号，比如 "System.String"
			return T.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
		}
	}


// 	public str MkCls_DictMapperForOneType(){
// 		var typeSymbol = CtxTargetType.TypeSymbol;
// 		var properties = CtxTargetType.PublicProps;
// var N = ConstName.Inst;
// var S = SymbolWithNamespace.Inst;
// var s = $$"""
// public class TODO : {{N.NsDict}}.{{N.IDictForOneType}}{
// 	public virtual Type {{N.TargetType}} {get;} = typeof({{ResolveFullTypeFitsTypeof(CtxTargetType.TypeSymbol)}});

// 	public virtual {{S.IDictionary}}<string, object?> ToDict(object obj){

// 	}

// 	public virtual {{S.IDictionary}} {{N.GetTypeDict}}(){

// 	}

// 	public object {{N.Assign}}(object obj, {{S.IDictionary}}<string, object?> dict){

// 	}
// }
// """;
// 	}

	public str MkStaticMethod_GetTypeDictWithParam(){
		var typeSymbol = CtxTargetType.TypeSymbol;
		var properties = CtxTargetType.PublicProps;
		// Logger.Append((properties==null)+"");
		// Logger.Append((properties.Count())+"");
		var dictEntries = properties.Select(p =>{
			var TypeName = ResolveFullTypeFitsTypeof(p.Type);
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


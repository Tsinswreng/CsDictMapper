// #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.CodeAnalysis;
// using Tsinswreng.CsDictMapper.Tools;

// namespace Tsinswreng.CsDictMapper.Ctor.CodeGen;


// public  partial class CtxTargetType{
// 	public CtxCtor CtxCtor{get;set;}
// 	public INamedTypeSymbol TypeSymbol{get;set;}
// 	public CtxTargetType(
// 		CtxCtor CtxCtor
// 		,INamedTypeSymbol TypeSymbol
// 	){
// 		this.CtxCtor = CtxCtor;
// 		this.TypeSymbol = TypeSymbol;
// 	}

// 	bool _Inited = false;
// 	public CtxTargetType Init(){
// 		if(_Inited){return this;}
// 		var typeSymbol = TypeSymbol;
// 		PublicProps = Tools.CodeTool.GetPropsWithParent(typeSymbol);//TODO where()
// 		_Inited = true;
// 		return this;
// 	}

// 	public IEnumerable<IPropertySymbol> PublicProps{get;set;} = null!;

// }


// public  partial class GenTargetType{

// 	public CtxTargetType CtxTargetType{get;set;}

// 	public GenTargetType(){}
// 	public GenTargetType(CtxTargetType Ctx_TargetType){
// 		Init(Ctx_TargetType);
// 	}

// 	public GenTargetType Init(CtxTargetType Ctx_TargetType){
// 		this.CtxTargetType = Ctx_TargetType;
// 		Ctx_TargetType.Init();
// 		return this;
// 	}

// 	public str MkMethod_ToDict(){
// 		var typeSymbol = CtxTargetType.TypeSymbol;
// 		var properties = CtxTargetType.PublicProps;
// 		// Logger.Append((properties==null)+"");
// 		// Logger.Append((properties.Count())+"");
// 		var dictEntries = properties.Select(p =>
// 			$"""["{p.Name}"] = obj.{p.Name},""")
// 		;
// 		var N = Const_Name.Inst;
// 		var S = SymbolWithNamespace.Inst;
// 		var MethodCode =
// $$"""
// 		public static {{S.IDictionary}}<string, {{S.ObjectN}}> {{N.ToDict}} ({{typeSymbol}} obj){
// 			return new {{S.Dictionary}}<string, {{S.ObjectN}}>{
// {{string.Join("\n",dictEntries)}}
// 			};
// 		}
// """;
// 		return MethodCode;
// 	}


// 	public str MkMethod_Assign(){
// 		var typeSymbol = CtxTargetType.TypeSymbol;
// 		var properties = CtxTargetType.PublicProps;
// 		var dictEntries = properties.Select(p =>
// 			//$""" o.{p.Name} = d["{p.Name}"] as {p.Type.ToDisplayString()}; """)
// 			$""" o.{p.Name} = ({p.Type.ToDisplayString()})d["{p.Name}"]; """)
// 		;
// 		var N = Const_Name.Inst;
// 		var S = SymbolWithNamespace.Inst;
// 		var MethodCode =
// $$"""
// 		public static {{typeSymbol}} {{N.Assign}} ({{typeSymbol}} o, {{S.IDictionary}}<string, {{S.ObjectN}}> d){
// {{string.Join("\n",dictEntries)}}
// 			return o;
// 		}
// """;
// 		return MethodCode;
// 	}

// 	public str MkTypeCacheElseIf(){
// 		var TypeSymbol = CtxTargetType.TypeSymbol;
// 		var N = Const_Name.Inst;
// 		var S = SymbolWithNamespace.Inst;
// 		return
// $$"""
// else if(typeof(T) == typeof({{TypeSymbol}})){
// 	Fn_ToDict = (obj) => {
// 		return {{N.ToDict}}(({{TypeSymbol}})({{S.ObjectN}})obj);
// 	};
// 	Fn_Assign = (obj, dict) => {
// 		var o = ({{TypeSymbol}})({{S.ObjectN}})obj;
// 		{{N.Assign}}(o, dict);
// 		return obj;
// 	};
// }
// """;
// 	}

// }

// using System;
// using System.Collections.Generic;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Tsinswreng.CsDictMapper.Dict.Attributes;
// using Tsinswreng.CsDictMapper.Tools;

// namespace Tsinswreng.CsDictMapper.Ctor.CodeGen;

// public  partial class CtxCtor{

// 	/// <summary>
// 	/// 包含[Ctor]特性的类(DictCtx)
// 	/// </summary>
// 	public ClassDeclarationSyntax CtorAttrClass { get;set; }
// 	public GeneratorExecutionContext ExeCtx{get;set;}
// 	public CtxCtor(
// 		GeneratorExecutionContext ExeCtx
// 		,ClassDeclarationSyntax CtorAttrClass
// 	){
// 		this.CtorAttrClass = CtorAttrClass;
// 		this.ExeCtx = ExeCtx;
// 		Init();
// 	}
// 	bool _Inited = false;
// 	public CtxCtor? Init(){
// 		if(_Inited){
// 			return this;
// 		}
// 		SemanticModel = ExeCtx.Compilation.GetSemanticModel(CtorAttrClass.SyntaxTree);
// 		DictTypeClassSymbol = SemanticModel.GetDeclaredSymbol(CtorAttrClass) as INamedTypeSymbol;
// 		if(DictTypeClassSymbol == null){
// 			return null;
// 		}
// 		DictCtxNamespaceStr = DictTypeClassSymbol?.ContainingNamespace?.ToDisplayString()??"";
// 		// TargetTypes = GenDictCtx.YieldTypeWithDictTypeAttr(DictTypeClassSymbol!);
// 		// foreach (var TargetType in TargetTypes) {
// 		// 	var Ctx_TargetType = new CtxTargetType(
// 		// 		Ctx_DictCtx:this
// 		// 		,TypeSymbol:TargetType
// 		// 	).Init();
// 		// 	Ctx_TargetTypes.Add(Ctx_TargetType);
// 		// }
// 		_Inited = true;
// 		return this;
// 	}

// 	public SemanticModel SemanticModel{get;set;} = null!;
// 	public INamedTypeSymbol? DictTypeClassSymbol{get;set;} = null!;
// 	public str DictCtxNamespaceStr{get;set;} = "";
// 	public IEnumerable<INamedTypeSymbol> TargetTypes{get;set;} = null!;
// 	public IList<CtxTargetType> Ctx_TargetTypes{get;set;} = new List<CtxTargetType>();
// 	public IList<str> TypesElseIfs{get;set;} = new List<str>();

// }


// public  partial class GenDictCtx{

// 	/// <summary>
// 	///
// 	/// </summary>
// 	public CtxCtor Ctx_DictCtx{get;set;}

// 	public GenDictCtx(CtxCtor Ctx_DictCtx){
// 		this.Ctx_DictCtx = Ctx_DictCtx;
// 	}

// 	public str MkClass(CtxTargetType Ctx_TargetType){
// 		var ClsName = Ctx_DictCtx.CtorAttrClass.Identifier.Text;
// 		//var methods = MethodCodes.Select(m=>m);
// 		var GenTargetType = new GenTargetType(Ctx_TargetType);
// 		List<str> methods = [
// 			GenTargetType.MkMethod_ToDict()
// 			,GenTargetType.MkMethod_Assign()
// 		];
// 		var cls =
// $$"""
// 	public partial class {{ClsName}}{

// {{str.Join("\n",methods)}}

// 	}
// """;
// 		return cls;
// 	}

// 	public str MkGeneric(){
// 		var N = Const_Name.Inst;
// 		var S = SymbolWithNamespace.Inst;
// return
// //TODO 用完整限定名
// $$"""
// 	public static {{S.IDictionary}}<string, {{S.ObjectN}}> {{N.ToDict}}T<T>(T obj){
// 		var fn = {{N.TypeFnSaver}}<T>.Fn_ToDict;
// 		return fn(obj);
// 	}
// 	public static T AssignT<T>(T obj, {{S.IDictionary}}<string, {{S.ObjectN}}> dict){
// 		var fn = {{N.TypeFnSaver}}<T>.Fn_Assign;
// 		return fn(obj, dict);
// 	}
// """;
// 	}

// 	public str MkTypeFnSaver(){
// 		IEnumerable<CtxTargetType> Ctx_TargetTypes = Ctx_DictCtx.Ctx_TargetTypes;
// 		var GenTargetType = new GenTargetType();
// 		IList<str> ElseIfs = Ctx_DictCtx.TypesElseIfs;
// 		foreach (var Ctx_TargetType in Ctx_TargetTypes){
// 			//GenTargetType.Ctx_TargetType = Ctx_TargetType;
// 			GenTargetType.Init(Ctx_TargetType);
// 			var elseIf = GenTargetType.MkTypeCacheElseIf();
// 			ElseIfs.Add(elseIf);
// 		}
// 		var S = SymbolWithNamespace.Inst;
// 		var N = Const_Name.Inst;
// return
// $$"""
// 	public partial class {{N.TypeFnSaver}}<T>{
// 		public static System.Func<T, {{S.IDictionary}}<string, {{S.ObjectN}}>> Fn_ToDict;
// 		public static System.Func<T, {{S.IDictionary}}<string, {{S.ObjectN}}>, T> Fn_Assign;

// 		static {{N.TypeFnSaver}}(){
// 			if(false){}
// 			{{str.Join("\n",ElseIfs)}}
// 		}
// 	}
// """;
// 	}

// 	public str MkFile_TypeFnSaver(){
// 		var Code_MkTypeFnSaver = MkTypeFnSaver();
// 		var ClsName = Ctx_DictCtx.CtorAttrClass.Identifier.Text;
// 		var ClsCode =
// $$"""
// public partial class {{ClsName}}{
// 	{{Code_MkTypeFnSaver}}
// 	{{MkGeneric()}}
// }
// """;

// 		return MkNs(ClsCode);
// 	}

// 	public str MkNs(str ClassCode){
// 		return
// $$"""
// {{CodeTool.SurroundWithNamespace(
// 	Ctx_DictCtx.DictCtxNamespaceStr
// 	,ClassCode
// )}}
// """;
// 	}


// /// <summary>
// /// 取類型芝有[DictType]者
// /// </summary>
// /// <param name="classSymbol"></param>
// /// <returns></returns>
// 	public static IEnumerable<INamedTypeSymbol> YieldTypeWithDictTypeAttr(INamedTypeSymbol classSymbol) {
// 		return CodeTool.YieldTypeWithAttr(classSymbol, nameof(DictType));
// 	}

// 	public str MkFile(CtxTargetType Ctx_TargetType){
// 		var ClsCode = MkClass(Ctx_TargetType);
// 		var NsCode = MkNs(ClsCode);
// 		return NsCode;
// 	}

// 	public nil Run(){
// 		str FileCode = "";
// 		str FileName = "";
// 		var NoWarn = "#pragma warning disable CS8618, CS8600, CS8601, CS8604, CS8605\n";
// 		try{
// 			var DictCtxName = (Ctx_DictCtx.DictTypeClassSymbol?.ToString()??"");
// 			var i = 0;
// 			foreach(var Ctx_TargetType in Ctx_DictCtx.Ctx_TargetTypes){
// 				FileCode = MkFile(Ctx_TargetType);
// 				FileName = DictCtxName
// 					+"-"+ Ctx_TargetType.TypeSymbol.ToString()
// 					+ ".cs"
// 				;

// 				var guid = Guid.NewGuid().ToString();
// 				Ctx_DictCtx.ExeCtx.AddSource(FileName, NoWarn+FileCode);
// 				i++;
// 			}
// 			var Code_MkTypeFnSaver = MkFile_TypeFnSaver();
// 			Ctx_DictCtx.ExeCtx.AddSource(DictCtxName+"-TypeFnSaver.cs", NoWarn+Code_MkTypeFnSaver);
// 			return Nil;
// 		}
// 		catch (System.Exception e){
// 			// Logger.Append(FileName);
// 			// Logger.Append(e.ToString());
// 			//Logger.Append(FileCode);
// 			throw e;
// 		}
// 		//return Nil;
// 	}
// }

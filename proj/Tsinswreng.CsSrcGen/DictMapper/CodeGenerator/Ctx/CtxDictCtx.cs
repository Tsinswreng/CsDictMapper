using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tsinswreng.CsSrcGen.DictMapper.Attributes;
using Tsinswreng.CsSrcGen.Tools;

namespace Tsinswreng.CsSrcGen.DictMapper.CodeGenerator.Ctx;

public class CtxDictCtx{

	/// <summary>
	/// 包含[DictType(typeof(TargetType))]特性的类(DictCtx)
	/// </summary>
	public ClassDeclarationSyntax DictCtxClass { get;set; }
	public GeneratorExecutionContext ExeCtx{get;set;}
	public CtxDictCtx(
		GeneratorExecutionContext ExeCtx
		,ClassDeclarationSyntax DictCtxClass
	){
		this.DictCtxClass = DictCtxClass;
		this.ExeCtx = ExeCtx;
		Init();
	}
	bool _Inited = false;
	public CtxDictCtx? Init(){
		if(_Inited){
			return this;
		}
		SemanticModel = ExeCtx.Compilation.GetSemanticModel(DictCtxClass.SyntaxTree);
		DictTypeClassSymbol = SemanticModel.GetDeclaredSymbol(DictCtxClass) as INamedTypeSymbol;
		if(DictTypeClassSymbol == null){
			return null;
		}
		DictCtxNsStr = DictTypeClassSymbol?.ContainingNamespace?.ToDisplayString()??"";
		// TargetTypes = GenDictCtx.YieldTypeWithDictTypeAttr(DictTypeClassSymbol!);
		// foreach (var TargetType in TargetTypes) {
		// 	var CtxTargetType = new CtxTargetType(
		// 		Ctx_DictCtx:this
		// 		,TypeSymbol:TargetType
		// 	).Init();
		// 	CtxTargetTypes.Add(CtxTargetType);
		// }
		DictTypeAttrParams = GenDictCtx.GetDictTypeAttrParams(DictTypeClassSymbol!);
		foreach(var DictTypeAttrParam in DictTypeAttrParams){
			var CtxTargetType = new CtxTargetType(
				CtxDictCtx:this
				,DictTypeAttrParam: DictTypeAttrParam
			).Init();
		}
		_Inited = true;
		return this;
	}

	public SemanticModel SemanticModel{get;set;} = null!;
	public INamedTypeSymbol? DictTypeClassSymbol{get;set;} = null!;
	public str DictCtxNsStr{get;set;} = "";
	[Obsolete("此ʹ參數ˋ不全。改用DictTypeAttrParams")]
	public IEnumerable<INamedTypeSymbol> TargetTypes{get;set;} = null!;
	public IEnumerable<IParamDictType> DictTypeAttrParams{get;set;}
	public IList<CtxTargetType> CtxTargetTypes{get;set;} = new List<CtxTargetType>();
	[Obsolete]
	public IList<str> TypesElseIfs{get;set;} = new List<str>();

}


public class GenDictCtx{

	/// <summary>
	///
	/// </summary>
	public CtxDictCtx CtxDictCtx{get;set;}

	public GenDictCtx(CtxDictCtx Ctx_DictCtx){
		this.CtxDictCtx = Ctx_DictCtx;
	}

// 	[Obsolete]
// 	public str OldMkClass(CtxTargetType CtxTargetType){
// 		var ClsName = CtxDictCtx.DictCtxClass.Identifier.Text;
// 		//var methods = MethodCodes.Select(m=>m);
// 		var GenTargetType = new GenTargetType(CtxTargetType);
// 		List<str> methods = [
// 			GenTargetType.MkStaticMethod_ToDict()
// 			,GenTargetType.MkStaticMethod_Assign()
// 			,GenTargetType.MkStaticMethod_GetTypeDictWithParam()
// 		];
// 		var cls =
// $$"""
// 	public partial class {{ClsName}}{

// {{str.Join("\n",methods)}}

// 	}
// """;
// 		return cls;
// 	}

	public str MkClass(CtxTargetType CtxTargetType){
		//var ClsName = CtxDictCtx.DictCtxClass.Identifier.Text;
		var GenTargetType = new GenTargetType(CtxTargetType);
		var N = ConstName.Inst;
		var S = SymbolWithNamespace.Inst;
		var ClsName = N.__TsinswrengDictMapper;
		//DictCtxNsStr
		var ClsBody = GenTargetType.MkClsBody_DictMapperForOneType();
return $$"""
public partial class {{ClsName}} : global::{{N.NsDictMapper}}.{{N.IDictMapperForOneType}}{
	protected static {{ClsName}}? _Inst = null;
	public static {{ClsName}} Inst => _Inst??= new {{ClsName}}();
	{{ClsBody}}
}
""";

	}

// 	[Obsolete]
// 	public str MkGeneric(){
// 		var N = ConstName.Inst;
// 		var S = SymbolWithNamespace.Inst;
// return
// //TODO 用完整限定名
// $$"""
// 	public static {{S.IDictionary}}<string, {{S.ObjectN}}> {{N.ToDictT}}<T>(T obj){
// 		var fn = {{N.TypeFnSaver}}<T>.{{N.FnToDict}};
// 		return fn(obj);
// 	}
// 	public static T {{N.AssignT}}<T>(T obj, {{S.IDictionary}}<string, {{S.ObjectN}}> dict){
// 		var fn = {{N.TypeFnSaver}}<T>.{{N.FnAssign}};
// 		return fn(obj, dict);
// 	}
// 	public static {{S.IDictionary}}<string, {{S.Type}}> {{N.GetTypeDictT}} <T>(){
// 		var fn = {{N.TypeFnSaver}}<T>.{{N.FnGetTypeDict}};
// 		return fn();
// 	}
// """;
// 	}

	public str MkType_MapperAdd(){
		IEnumerable<CtxTargetType> CtxTargetTypes = CtxDictCtx.CtxTargetTypes;
		var GenTargetType = new GenTargetType();
		//IList<str> ElseIfs = CtxDictCtx.TypesElseIfs;
		List<str> R = [];
		foreach (var CtxTargetType in CtxTargetTypes){
			GenTargetType.Init(CtxTargetType);
			var U = GenTargetType.MkTypeCacheDictAdd(CtxDictCtx);
			R.Add(U);
		}
		return str.Join("\n",R);
	}

// 	[Obsolete]
// 	public str MkTypeFnSaver(){
// 		IEnumerable<CtxTargetType> CtxTargetTypes = CtxDictCtx.CtxTargetTypes;
// 		var GenTargetType = new GenTargetType();
// 		IList<str> ElseIfs = CtxDictCtx.TypesElseIfs;
// 		foreach (var CtxTargetType in CtxTargetTypes){
// 			//GenTargetType.Ctx_TargetType = Ctx_TargetType;
// 			GenTargetType.Init(CtxTargetType);
// 			var elseIf = GenTargetType.MkTypeCacheElseIf();
// 			ElseIfs.Add(elseIf);
// 		}
// 		var S = SymbolWithNamespace.Inst;
// 		var N = ConstName.Inst;
// return
// $$"""
// 	public partial class {{N.TypeFnSaver}}<T>{
// 		public static {{S.Func}}<T, {{S.IDictionary}}<string, {{S.ObjectN}}>> {{N.FnToDict}};
// 		public static {{S.Func}}<T, {{S.IDictionary}}<string, {{S.ObjectN}}>, T> {{N.FnAssign}};
// 		public static {{S.Func}}<{{S.IDictionary}}<string, {{S.Type}}>> {{N.FnGetTypeDict}};

// 		static {{N.TypeFnSaver}}(){
// 			if(false){}
// 			{{str.Join("\n",ElseIfs)}}
// 		}
// 	}
// """;
// 	}

	public str MkFile_Main(){
		var ClsName = CtxDictCtx.DictCtxClass.Identifier.Text;
		var N = ConstName.Inst;
		var Ns = CtxDictCtx.DictCtxNsStr;
var Cls = $$"""
public partial class {{ClsName}}: {{N.NsDictMapper}}.{{N.DictMapper}} {
	public {{ClsName}}(){
		{{MkType_MapperAdd()}}
	}
}
""";
		return CodeTool.SurroundWithNamespace(Ns,Cls);
	}

// 	[Obsolete("MkFile_Main")]
// 	public str MkFile_TypeFnSaver(){
// 		var Code_MkTypeFnSaver = MkTypeFnSaver();
// 		var ClsName = CtxDictCtx.DictCtxClass.Identifier.Text;
// 		var ClsCode =
// $$"""
// public partial class {{ClsName}}{
// 	{{Code_MkTypeFnSaver}}
// 	{{MkGeneric()}}
// }
// """;

// 		return MkNs(ClsCode);
// 	}

	/// <summary>
	/// 見Example.cs之namespace UserDictCtxNs{ namespace NsA{ ...}}...
	/// </summary>
	/// <param name="ClassCode"></param>
	/// <param name="TargetNs"></param>
	/// <returns></returns>
	public str MkNs(str ClassCode, ITypeSymbol Target){
		var R = "";
		R = CodeTool.SurroundWithNamespace(
			CtxDictCtx.DictCtxNsStr+"._."+Target
			,ClassCode
		);
		// R = CodeTool.SurroundWithNamespace(
		// 	CtxDictCtx.DictCtxNsStr
		// 	,R
		// );
		return R;
	}

// 	public str MkNs(str ClassCode){
// 		return
// $$"""
// {{CodeTool.SurroundWithNamespace(
// 	CtxDictCtx.DictCtxNsStr
// 	,ClassCode
// )}}
// """;
// 	}


/// <summary>
/// 取類型芝有[DictType]者
/// </summary>
/// <param name="classSymbol"></param>
/// <returns></returns>
[Obsolete("改用GetDictTypeAttrParam 緣此不支持Recursive參數")]
	public static IEnumerable<INamedTypeSymbol> YieldTypeWithDictTypeAttr(INamedTypeSymbol classSymbol) {
		return CodeTool.YieldTypeWithAttr(classSymbol, nameof(DictType));
	}

	public static IEnumerable<IParamDictType> GetDictTypeAttrParams(INamedTypeSymbol classSymbol){
		var literalAttrName = nameof(DictType);
		var attributes = classSymbol.GetAttributes()
			.Where(attr => attr.AttributeClass?.Name == literalAttrName);

		foreach (var attr in attributes){
			// 先取构造函数第一个参数 TargetType（必有）
			var typeArg = attr.ConstructorArguments.FirstOrDefault();

			INamedTypeSymbol? targetType = typeArg.Value as INamedTypeSymbol;
			if (targetType == null){continue;}
			// 默认递归参数为 false
			bool recursive = false;
			// NamedArguments 里找是否传入了 Recursive 参数
			var recursiveArg = attr.NamedArguments
				.FirstOrDefault(kv => kv.Key == nameof(ParamDictType.Recursive));
			if (!recursiveArg.Equals(default(KeyValuePair<string, TypedConstant>))){
				recursive = recursiveArg.Value.Value is bool b && b;
			}
			yield return new ParamDictType(targetType, recursive);
		}//~foreach (var attr in attributes)
	}

	public str MkFile(CtxTargetType CtxTargetType){
		var ClsCode = MkClass(CtxTargetType);
		var NsCode = MkNs(ClsCode, CtxTargetType.TypeSymbol);
		return NsCode;
	}

	protected nil AddSrc(GeneratorExecutionContext ExeCtx, str FileName, str Code){
		Logger.Debug(FileName, Code);
		ExeCtx.AddSource(FileName, Code);
		return NIL;
	}

	public nil Run(){
		//Logger.Append("Run");//t
		str FileCode = "";
		str FileName = "";
		var NoWarn = "#pragma warning disable CS8618, CS8600, CS8601, CS8604, CS8605\n";
		try{
			var DictCtxName = (CtxDictCtx.DictTypeClassSymbol?.ToString()??"");
			var i = 0;
			foreach(var CtxTargetType in CtxDictCtx.CtxTargetTypes){
				FileCode = MkFile(CtxTargetType);
				FileName = DictCtxName
					+"-"+ CtxTargetType.TypeSymbol.ToString()
					+ ".cs"
				;
				//var guid = Guid.NewGuid().ToString();
				AddSrc(CtxDictCtx.ExeCtx, FileName, NoWarn+FileCode);
				i++;
			}
			//var Code_MkTypeFnSaver = MkFile_TypeFnSaver();
			var Code_Main = MkFile_Main();
			AddSrc(CtxDictCtx.ExeCtx, DictCtxName+"-.cs", NoWarn+Code_Main);
			return NIL;
		}
		catch (System.Exception e){
			// Logger.Append(FileName);
			// Logger.Append(e.ToString());
			//Logger.Append(FileCode);
			throw e;
		}
		//return Nil;
	}
}

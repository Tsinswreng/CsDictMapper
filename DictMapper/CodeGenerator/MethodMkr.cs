#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Tsinswreng.CsSrcGen.Tools;

namespace Tsinswreng.CsSrcGen.DictMapper.CodeGenerator;


public class MethodMkr{
	public Ctx_DictGen Ctx{get;set;}
	public IEnumerable<IPropertySymbol> PublicProps{get;set;} = null!;
	public IList<str> MethodCodes{get;set;} = new List<str>();

	public SemanticModel SemanticModel{get;set;}
	public ISymbol? DictTypeClassSymbol{get;set;}

	public str TargetNamespaceStr{get;set;} = "";
	public str DictCtxNamespaceStr{get;set;} = "";

	public MethodMkr(Ctx_DictGen Ctx){
		this.Ctx = Ctx;
		this.Init();
	}

	public static str MkFileCode(Ctx_DictGen Ctx){
		var Mkr = new MethodMkr(Ctx);
		return Mkr.MkFile();
	}

	nil Init(){
		var typeSymbol = Ctx.TypeSymbol;
		PublicProps = typeSymbol.GetMembers()
			.OfType<IPropertySymbol>()
			.Where(p => p.DeclaredAccessibility == Accessibility.Public)
		;
		TargetNamespaceStr = typeSymbol.ContainingNamespace.ToDisplayString();
		SemanticModel = Ctx.GenExeCtx.Compilation.GetSemanticModel(Ctx.DictCtxClass.SyntaxTree);
		DictTypeClassSymbol = SemanticModel.GetDeclaredSymbol(Ctx.DictCtxClass);
		DictCtxNamespaceStr = DictTypeClassSymbol?.ContainingNamespace?.ToDisplayString()??"";

		return NIL;
	}

	str MkMethod_ToDict(){
		var typeSymbol = Ctx.TypeSymbol;
		var properties = PublicProps;
		var dictEntries = properties.Select(p =>
			$"""["{p.Name}"] = obj.{p.Name},""")
		;

		var MethodCode =
$$"""
		public static Dictionary<string, object> ToDict({{typeSymbol}} obj){
			return new Dictionary<string, object>{
{{string.Join("\n",dictEntries)}}
			};
		}
""";
		MethodCodes.Add(MethodCode);
		return MethodCode;
	}

	str MkMethod_ToObj(){
		var typeSymbol = Ctx.TypeSymbol;
		var properties = PublicProps;
		var dictEntries = properties.Select(p =>
			//$""" o.{p.Name} = d["{p.Name}"] as {p.Type.ToDisplayString()}; """)
			$""" o.{p.Name} = ({p.Type.ToDisplayString()})d["{p.Name}"]; """)
		;

		var MethodCode =
$$"""
		public static {{typeSymbol}} AssignObj({{typeSymbol}} o, Dictionary<string, object> d){
{{string.Join("\n",dictEntries)}}
			return o;
		}
""";
		MethodCodes.Add(MethodCode);
		return MethodCode;
	}

	str MkClass(){
		var ClsName = Ctx.DictCtxClass.Identifier.Text;
		//var methods = MethodCodes.Select(m=>m);
		List<str> methods = [
			MkMethod_ToDict()
			,MkMethod_ToObj()
		];
		var cls =
$$"""
	public partial class {{ClsName}}{

{{str.Join("\n",methods)}}

	}
""";
		return cls;
	}

	str MkFile(){
		var ClassCode = MkClass();
		// Logger.Append("ClassCode");//t
		// Logger.Append(ClassCode);

		var File =  $$"""
using System.Collections.Generic;
{{CodeTool.SurroundWithNamespace(
	DictCtxNamespaceStr
	,ClassCode
)}}
""";
		return File;
	}

	str MkTypeCacheElseIf(){
		return
$$"""
else if(typeof(T) == typeof({{Ctx.TypeSymbol}})){
	Fn_ToDict = (obj) => {
		return ToDict(({{Ctx.TypeSymbol}})(object)obj);
	};
	Fn_Assign = (obj, dict) => {
		var o = ({{Ctx.TypeSymbol}})(object)obj;
		AssignObj(o, dict);
		return obj;
	};
}
""";
	}

	str MkTypeFnSaver(){
return
$$"""
	public partial class TypeFnSaver<T>{
		public static Func<T, Dictionary<string, object>> Fn_ToDict;
		public static Func<T, Dictionary<string, object>, T> Fn_Assign;

		static TypeFnSaver(){
			if(false){}

		}
	}
""";
	}
}

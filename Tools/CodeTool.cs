using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tsinswreng.SrcGen.Tools;

public class CodeTool{


	public static str SurroundWithNamespace(str NsStr, string Code){
		var strNs = NsStr;
		if(strNs == "<global namespace>"){
			return Code;
		}
		return $"namespace {strNs} {{\n{Code}\n}}";
	}

	public static str SurroundWithNamespace(INamespaceSymbol Ns, string Code){
		var strNs = Ns.ContainingNamespace.ToDisplayString();
		return SurroundWithNamespace(strNs, Code);
	}

	public static string GenUsingStatement(string Namespace){
		if(Namespace == "<global namespace>"){
			return "";
		}
		return $"using {Namespace};\n";
	}


/// <summary>
/// 取類型芝有指定特性者
/// </summary>
/// <param name="classSymbol"></param>
/// <param name="LiteralAttrName">
/// !須同於名芝實際用旹者洏非定義Attr旹者
/// 如[Obsolete]public class MyClass{}、則LiteralAttrName當傳入 "Obsolete"洏非"ObsoleteAttribute"、否則取不到
/// </param>
/// <returns></returns>
	public static IEnumerable<INamedTypeSymbol> YieldTypeWithAttr(
		INamedTypeSymbol classSymbol
		,str LiteralAttrName
	) {
		// 提取所有 DictType 特性
		var attributes = classSymbol.GetAttributes()
			.Where(attr => attr.AttributeClass?.Name == LiteralAttrName);

		// 解析特性中的 Type 参数
		foreach (var attr in attributes) {
			var typeArg = attr.ConstructorArguments.FirstOrDefault();
			if (typeArg.Value is INamedTypeSymbol targetType) {
				yield return targetType;
			}
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Tsinswreng.CsDictMapper.Tools;

public static class CodeTool{


	public static str CombineNsEtSymbol(INamespaceSymbol NsSymbol, str Symbol){
		if(NsSymbol.IsGlobalNamespace){
			return Symbol;
		}
		return NsSymbol.ToDisplayString() + "." + Symbol;
	}


	public static str SurroundWithNamespace(str NsStr, string Code){
		var strNs = NsStr;
		if(strNs == "" || strNs == "<global namespace>"){
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
			if (typeArg.Value is INamedTypeSymbol TargetType) {
				yield return TargetType;
			}
		}
	}


	// public static IEnumerable<INamedTypeSymbol> GetAttrArgs(
	// 	INamedTypeSymbol classSymbol
	// 	,str LiteralAttrName
	// ) {
	// 	// 提取所有 DictType 特性
	// 	var attributes = classSymbol.GetAttributes()
	// 		.Where(attr => attr.AttributeClass?.Name == LiteralAttrName);

	// 	// 解析特性中的 Type 参数
	// 	foreach (var attr in attributes) {
	// 		var typeArg = attr.ConstructorArguments.FirstOrDefault();
	// 		if (typeArg.Value is INamedTypeSymbol TargetType) {
	// 			yield return TargetType;
	// 		}
	// 	}
	// }

	public static IEnumerable<IPropertySymbol> GetPropsWithParent(
		INamedTypeSymbol classSymbol
	){
		var properties = new List<IPropertySymbol>();
		var currentType = classSymbol;
		while (currentType != null) {
			properties.AddRange(currentType.GetMembers()
				.OfType<IPropertySymbol>()
				.Where(p => p.DeclaredAccessibility == Accessibility.Public && !p.IsStatic));
			currentType = currentType.BaseType;
		}
		return properties;
	}


	public static IEnumerable<IPropertySymbol> GetPropsWithParent(
		this INamedTypeSymbol classSymbol
		,Func<IPropertySymbol,bool> predicate
	){
		var properties = new List<IPropertySymbol>();
		var currentType = classSymbol;
		while (currentType != null) {
			properties.AddRange(currentType.GetMembers()
				.OfType<IPropertySymbol>()
				.Where(predicate));
			currentType = currentType.BaseType;
		}
		return properties;
	}


/// <summary>
/// 解析完整ʹ類型名芝可置于typeof()中者
/// 帶global::
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



}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsinswreng.SrcGen.Dict.Attributes;
using Tsinswreng.SrcGen.Tools;


namespace Tsinswreng.SrcGen.Dict.CodeGenerator;

[Generator]
public class DictGenerator : ISourceGenerator {
	public Ctx_DictGen Ctx { get;set; } = new();
	public void Initialize(GeneratorInitializationContext context) {
		// 注册语法接收器来捕获标记了DictTypeAttribute的类
		context.RegisterForSyntaxNotifications(() => new DictTypeSyntaxReceiver());
	}

	public void Execute(GeneratorExecutionContext context) {
		if (context.SyntaxReceiver is not DictTypeSyntaxReceiver receiver){
			return;
		}

		// 处理每个标记了DictType的类
		foreach (var classDecl in receiver.DictCtxClasses) {
			var model = context.Compilation.GetSemanticModel(classDecl.SyntaxTree);
			var classSymbol = model.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;

			// 收集所有DictTypeAttribute中指定的类型
			if(classSymbol == null){
				continue;
			}
			var targetTypes = GetTargetTypes(classSymbol);

			// 为每个目标类型生成扩展方法
			GenerateExtensionMethods(context, targetTypes);
		}
	}

	private void GenerateExtensionMethods(
		GeneratorExecutionContext context
		,IEnumerable<INamedTypeSymbol> targetTypes
	){
		foreach (var typeSymbol in targetTypes) {
			var code = BuildExtensionMethodCode(typeSymbol);
			context.AddSource($"{typeSymbol.Name}_DictExtensions.g.cs", code);
		}
	}

	public static string GenUsingStatement(string Namespace){
		if(Namespace == "<global namespace>"){
			return "";
		}
		return $"using {Namespace};\n";
	}

	private string BuildExtensionMethodCode(INamedTypeSymbol typeSymbol) {
		var properties = typeSymbol.GetMembers()
			.OfType<IPropertySymbol>()
			.Where(p => p.DeclaredAccessibility == Accessibility.Public);

		var Namespace = typeSymbol.ContainingNamespace.ToDisplayString();


		var dictEntries = properties.Select(p =>
			$"""["{p.Name}"] = obj.{p.Name},""");

		return $$"""
using System.Collections.Generic;
{{GenUsingStatement(Namespace)}}
namespace YourNamespace{
	public static partial class DictExtensions{
		public static Dictionary<string, object> ToDict(this DictCtx ctx, {{typeSymbol}} obj){
			return new Dictionary<string, object>{
				{{string.Join("\n",dictEntries)}}
			};
		}
	}
}
""";
	}


	private IEnumerable<INamedTypeSymbol> GetTargetTypes(INamedTypeSymbol classSymbol) {
		// 提取所有 DictType 特性
		var attributes = classSymbol.GetAttributes()
			.Where(attr => attr.AttributeClass?.Name == nameof(DictType));

		// 解析特性中的 Type 参数
		foreach (var attr in attributes) {
			var typeArg = attr.ConstructorArguments.FirstOrDefault();
			if (typeArg.Value is INamedTypeSymbol targetType) {
				yield return targetType;
			}
		}
	}


}

// 语法接收器，用于定位所有包含DictType特性的类
class DictTypeSyntaxReceiver : ISyntaxReceiver {
	public List<ClassDeclarationSyntax> DictCtxClasses { get; } = new();

	public void OnVisitSyntaxNode(SyntaxNode syntaxNode) {
		if (syntaxNode is ClassDeclarationSyntax classDecl &&
			classDecl.AttributeLists.Any(a =>
				a.Attributes.Any(attr =>
					attr.Name.ToString() ==  nameof(DictType) //"DictType"
				)
			)
		) {
			DictCtxClasses.Add(classDecl);
			//Logger.Append("DictTypeSyntaxReceiver");+
			//Logger.Append(syntaxNode.ToString()); 完整ʹ類定義 源碼
		}
	}
}

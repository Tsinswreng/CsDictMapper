using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsinswreng.CsSrcGen.DictMapper.Attributes;
using Tsinswreng.CsSrcGen.Tools;

namespace Tsinswreng.CsSrcGen.DictMapper.CodeGenerator;

//[Generator]
public class DictGenerator : ISourceGenerator {
	//public Ctx_DictGen Ctx { get;set; } = new();
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
			//Logger.Append($"Processing class {classSymbol.Name}");//t
			var targetTypes = YieldTypeWithDictTypeAttr(classSymbol);

			// 为每个目标类型生成扩展方法
			GenerateExtensionMethods(context, targetTypes, classDecl);
		}
	}

	private void GenerateExtensionMethods(
		GeneratorExecutionContext context
		,IEnumerable<INamedTypeSymbol> targetTypes
		,ClassDeclarationSyntax classDecl
	){
		foreach (var typeSymbol in targetTypes) {
			var Ctx = new Ctx_DictGen{
				DictCtxClass = classDecl
				,TypeSymbol = typeSymbol
				,GenExeCtx = context
			};
			var code = MethodMkr.MkFileCode(Ctx);
			//Logger.Append(code);//t
			context.AddSource($"{typeSymbol.Name}_DictExtensions.g.cs", code);
		}
	}



/// <summary>
/// 取類型芝有[DictType]者
/// </summary>
/// <param name="classSymbol"></param>
/// <returns></returns>
	public static IEnumerable<INamedTypeSymbol> YieldTypeWithDictTypeAttr(INamedTypeSymbol classSymbol) {
		return CodeTool.YieldTypeWithAttr(classSymbol, nameof(DictType));
	}

}

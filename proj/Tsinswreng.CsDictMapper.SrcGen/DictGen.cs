using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Tsinswreng.CsDictMapper.SrcGen.Ctx;
namespace Tsinswreng.CsDictMapper.SrcGen;

[Generator]
public partial class DictGen: ISourceGenerator{
	public void Initialize(GeneratorInitializationContext context) {
		// 注册语法接收器来捕获标记了DictTypeAttribute的类
		//context.RegisterForSyntaxNotifications(() => new DictTypeSyntaxReceiver());
	}

	public void Execute(GeneratorExecutionContext ExeCtx) {
		try {
			// 直接獲取所有類型
			var allTypes = ExeCtx.Compilation.SourceModule.GlobalNamespace.GetAllNamedTypes();
			foreach (var type in allTypes) {
				// 判斷是否有 [DictType] 特性
				if (!type.GetAttributes().Any(attr => attr.AttributeClass?.Name == nameof(DictType))) {
					continue;
				}
				var CtxDictCtx = new CtxDictCtx(
					ExeCtx: ExeCtx,
					DictTypeClassSymbol: type
				).Init();

				if (CtxDictCtx == null) {
					continue;
				}
				var GenDictCtx = new GenDictCtx(
					Ctx_DictCtx: CtxDictCtx
				);
				GenDictCtx.Run();
			}
		}
		catch (System.Exception e) {
			Logger.Debug("debug", e + "\n" + e.StackTrace + "");
			Logger.Append(e + "");
			throw;
		}
	}


}




//TODO 抽取至共ʹ庫
internal static class SymbolExtensions{
	/// <summary>
	/// 递归拿到某个命名空间下所有命名类型（包括嵌套命名空间、嵌套类型）。
	/// </summary>
	internal static IEnumerable<INamedTypeSymbol> GetAllNamedTypes(this INamespaceSymbol ns){
		foreach (var member in ns.GetMembers()){
			switch (member){
				case INamespaceSymbol nestedNs:
					foreach (var t in nestedNs.GetAllNamedTypes()){
						yield return t;
					}
				break;

				case INamedTypeSymbol type:
					yield return type;
					// 再往下递归嵌套类型
					foreach (var nested in type.GetAllNestedTypes()){
						yield return nested;
					}
				break;
			}
		}
	}

	/// <summary>
	/// 递归拿到某个类型及其所有嵌套类型。
	/// </summary>
	internal static IEnumerable<INamedTypeSymbol> GetAllNestedTypes(this INamedTypeSymbol type){
		foreach (var nested in type.GetTypeMembers()){
			yield return nested;
			foreach (var deeper in nested.GetAllNestedTypes()){
				yield return deeper;
			}
		}
	}
}

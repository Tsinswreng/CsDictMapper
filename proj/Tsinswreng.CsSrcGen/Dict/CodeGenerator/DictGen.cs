using System.Reflection;
using Microsoft.CodeAnalysis;
using Tsinswreng.CsSrcGen.Dict.CodeGenerator.Ctx;
using Tsinswreng.CsSrcGen.Tools;
namespace Tsinswreng.CsSrcGen.Dict.CodeGenerator;


[Generator]
public class DictGen: ISourceGenerator{
	public void Initialize(GeneratorInitializationContext context) {
		// 注册语法接收器来捕获标记了DictTypeAttribute的类
		context.RegisterForSyntaxNotifications(() => new DictTypeSyntaxReceiver());
	}
	public void Execute(GeneratorExecutionContext ExeCtx) {
		try{
			if (ExeCtx.SyntaxReceiver is not DictTypeSyntaxReceiver Receiver){
				return;
			}
			//每個目標類型生成一個文件、另加一個泛型緩存
			//勿全寫于一個文件中
			// Logger.Append("Receiver.DictCtxClasses.Count");
			// Logger.Append(Receiver.DictCtxClasses.Count+"");
			foreach(var DictCtxClass in Receiver.DictCtxClasses){
				var Ctx_DictCtx = new CtxDictCtx(
					ExeCtx: ExeCtx
					,DictCtxClass: DictCtxClass
				).Init();

				if(Ctx_DictCtx == null){
					continue;
				}
				var GenDictCtx = new GenDictCtx(
					Ctx_DictCtx:Ctx_DictCtx
				);
				GenDictCtx.Run();
			}
		}
		catch (System.Exception e){
			Logger.Append(e+"");
			throw;
		}

	}


}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tsinswreng.CsDictMapper.SrcGen;

public  partial class CtxDictGen{
	/// <summary>
	/// 包含[DictType(typeof(TargetType))]特性的类(DictCtx)
	/// </summary>
	#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public CtxDictGen(){

	}
	public ClassDeclarationSyntax DictCtxClass { get;set; }

	/// <summary>
	/// TargetType
	/// </summary>
	public INamedTypeSymbol TypeSymbol{get;set;}

	public GeneratorExecutionContext GenExeCtx{get;set;}
}

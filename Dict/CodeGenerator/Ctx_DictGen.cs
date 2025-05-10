using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsinswreng.SrcGen.Dict.Attributes;
using Tsinswreng.SrcGen.Tools;

namespace Tsinswreng.SrcGen.Dict.CodeGenerator;

public class Ctx_DictGen{
	/// <summary>
	/// 包含[DictType(typeof(TargetType))]特性的类(DictCtx)
	/// </summary>
	#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public Ctx_DictGen(){

	}
	public ClassDeclarationSyntax DictCtxClass { get;set; }

	/// <summary>
	/// TargetType
	/// </summary>
	public INamedTypeSymbol TypeSymbol{get;set;}

	public GeneratorExecutionContext GenExeCtx{get;set;}
}

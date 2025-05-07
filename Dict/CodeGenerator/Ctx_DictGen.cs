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
	/// 包含DictType特性的类(DictCtx)
	/// </summary>
	public ClassDeclarationSyntax DictCtxClasses { get;set; }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tsinswreng.CsSrcGen.DictMapper.Attributes;

namespace Tsinswreng.CsSrcGen.DictMapper.CodeGenerator;
// 语法接收器，用于定位所有包含DictType特性的类(一般只有一個)
public class DictTypeSyntaxReceiver : ISyntaxReceiver {
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

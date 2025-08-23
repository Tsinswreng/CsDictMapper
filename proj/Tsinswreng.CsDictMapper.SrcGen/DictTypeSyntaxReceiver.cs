using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tsinswreng.CsDictMapper.SrcGen;
// 语法接收器，用于定位所有包含DictType特性的类(一般只有一個)
public partial class DictTypeSyntaxReceiver : ISyntaxReceiver {
	public IList<ClassDeclarationSyntax> DictCtxClasses { get; } = new List<ClassDeclarationSyntax>();

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

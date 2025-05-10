using System;

namespace Tsinswreng.SrcGen.Dict.CodeGenerator;

public class Const_Name{
	protected static Const_Name? _Inst = null;
	public static Const_Name Inst => _Inst??= new Const_Name();

	public str ToDict = nameof(ToDict);
	public str Assign = nameof(Assign);
}

public class SymbolWithNamespace{
	protected static SymbolWithNamespace? _Inst = null;
	public static SymbolWithNamespace Inst => _Inst??= new SymbolWithNamespace();

	public str Func = "System.Func";
	public str Dictionary = "System.Collections.Generic.Dictionary";
}

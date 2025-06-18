using System;

namespace Tsinswreng.CsDictMapper.Dict.CodeGenerator;

public class ConstName{
	protected static ConstName? _Inst = null;
	public static ConstName Inst => _Inst??= new ConstName();

	public str ToDict = nameof(ToDict);
	public str ToDictT = nameof(ToDictT);
	public str GetTypeDict = nameof(GetTypeDict);
	public str GetTypeDictT = nameof(GetTypeDictT);
	public str Assign = nameof(Assign);
	public str AssignT = nameof(AssignT);
	public str TypeFnSaver = nameof(TypeFnSaver);
	public str FnToDict = nameof(FnToDict);
	public str FnAssign = nameof(FnAssign);
	public str FnGetTypeDict = nameof(FnGetTypeDict);
	public str NsDict = nameof(Tsinswreng)+"."+nameof(CsDictMapper)+"."+nameof(Dict);
	public str IDictForOneType = nameof(IDictForOneType);
	public str TargetType = nameof(TargetType);
}

public class SymbolWithNamespace{
	protected static SymbolWithNamespace? _Inst = null;
	public static SymbolWithNamespace Inst => _Inst??= new SymbolWithNamespace();

	public str Func = "System.Func";
	public str IDictionary = "System.Collections.Generic.IDictionary";
	public str Dictionary = "System.Collections.Generic.Dictionary";
	public str ObjectN = "System.Object?";
	public str Type = "System.Type";
}


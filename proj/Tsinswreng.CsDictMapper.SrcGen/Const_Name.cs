using System;

namespace Tsinswreng.CsDictMapper.SrcGen;

public class ConstName{
	protected static ConstName? _Inst = null;
	public static ConstName Inst => _Inst??= new ConstName();

	public str ToDict = nameof(ToDict);
	public str ToDictT = nameof(ToDictT);
	public str ToDictShallow = nameof(ToDictShallow);

	public str GetTypeDict = nameof(GetTypeDict);
	public str GetTypeDictT = nameof(GetTypeDictT);
	public str GetTypeDictShallow = nameof(GetTypeDictShallow);

	public str Assign = nameof(Assign);
	public str AssignT = nameof(AssignT);
	public str AssignShallow = nameof(AssignShallow);
	public str TypeFnSaver = nameof(TypeFnSaver);
	public str FnToDict = nameof(FnToDict);
	public str FnAssign = nameof(FnAssign);
	public str FnGetTypeDict = nameof(FnGetTypeDict);
	//public str NsDictMapper = nameof(Tsinswreng)+"."+nameof(CsDictMapper)+"."+nameof(DictMapper);
	public str NsDictMapper = nameof(Tsinswreng)+"."+nameof(CsDictMapper);
	public str IDictMapperForOneType = nameof(IDictMapperForOneType);
	public str TargetType = nameof(TargetType);
	public str Type_Mapper = nameof(Type_Mapper);
	public str IDictMapper = nameof(IDictMapper);
	public str DictMapper = nameof(DictMapper);
	public str __TsinswrengDictMapper = nameof(__TsinswrengDictMapper);

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


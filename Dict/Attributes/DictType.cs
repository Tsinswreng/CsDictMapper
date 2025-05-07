namespace Tsinswreng.SrcGen.Dict.Attributes{

using System;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class DictType:Attribute{
	public Type TargetType { get; }

	public DictType(Type targetType){
		TargetType = targetType;
	}
}

}//~ns

namespace Tsinswreng.SrcGen.Dict.Attributes{

using System;

/// <summary>
/// 名須潙DictType、叵作DictTypeAttribute
/// 緣用及nameof、斯類ʹ名ˋ 須同於 作特性ⁿ引用旹厎
/// 如引用時用[DictType(...)]則斯類ʹ名則須潙DictType、叵作DictTypeAttribute
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class DictType:Attribute{
	public Type TargetType { get; }

	public DictType(Type targetType){
		TargetType = targetType;
	}
}

}//~ns

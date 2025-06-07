namespace Tsinswreng.CsSrcGen.Ctor.Attributes;

using System;

/// <summary>
/// 名須潙DictType、叵作DictTypeAttribute
/// 緣用及nameof、斯類ʹ名ˋ 須同於 作特性ⁿ引用旹厎
/// 如引用時用[DictType(...)]則斯類ʹ名則須潙DictType、叵作DictTypeAttribute
/// </summary>
[AttributeUsage(
	AttributeTargets.Class|AttributeTargets.Struct
	,AllowMultiple = true
	,Inherited = false
)]
public class Ctor:Attribute{

	public str AppendedCode = "";

	public Ctor(){

	}
}

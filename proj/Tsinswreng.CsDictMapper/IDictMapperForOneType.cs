using System;
using System.Collections.Generic;

namespace Tsinswreng.CsDictMapper;


public partial interface IDictMapperForOneType{
	public Type TargetType{get;}
	public IDictionary<str, obj?> ToDictShallow(obj Obj);
	public IDictionary<str, Type> GetTypeDictShallow();
	public obj AssignShallow(obj Obj, IDictionary<str, obj?> Dict);

}


// public  partial interface I_Type_Mapper{
// 	public IDictionary<Type>
// }

using System;
using System.Collections.Generic;

namespace Tsinswreng.CsDictMapper;


public  partial interface IDictMapperForOneType{
	public Type TargetType{get;}
	public IDictionary<str, object?> ToDictShallow(object Obj);
	public IDictionary<str, Type> GetTypeDictShallow();
	public object AssignShallow(object Obj, IDictionary<str, object?> Dict);

}


// public  partial interface I_Type_Mapper{
// 	public IDictionary<Type>
// }

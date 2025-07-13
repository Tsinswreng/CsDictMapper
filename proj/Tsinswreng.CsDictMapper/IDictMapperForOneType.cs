using System;
using System.Collections.Generic;

namespace Tsinswreng.CsDictMapper;


public interface IDictMapperForOneType{
	public Type TargetType{get;}
	public IDictionary<str, object?> ToDictShallow(object obj);
	public IDictionary<str, Type> GetTypeDictShallow();
	public object AssignShallow(object obj, IDictionary<str, object?> dict);

}


// public interface I_Type_Mapper{
// 	public IDictionary<Type>
// }

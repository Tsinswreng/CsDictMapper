using System;
using System.Collections.Generic;

namespace Tsinswreng.CsSrcGen.Dict;


public interface IDictMapperForOneType{
	public Type TargetType{get;}
	public IDictionary<str, object?> ToDict(object obj);
	public IDictionary<str, Type> GetTypeDict();
	public object Assign(object obj, IDictionary<str, object?> dict);

}


// public interface I_Type_Mapper{
// 	public IDictionary<Type>
// }

using System;
using System.Collections.Generic;

namespace Tsinswreng.CsDictMapper.Dict;

[Obsolete("用DictMapper/")]
public interface IDictMapper{
	public IDictionary<str, object?> ToDictT<T>(T obj);
	public IDictionary<str, Type> GetTypeDictT<T>();
	public T AssignT<T> (T obj, IDictionary<str, object?> dict);

}

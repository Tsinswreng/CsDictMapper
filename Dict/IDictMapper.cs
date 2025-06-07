using System.Collections.Generic;

namespace Tsinswreng.CsSrcGen.Dict;

public interface IDictMapper{
	public IDictionary<str, object?> ToDictT<T>(T obj);
	public T AssignT<T> (T obj, IDictionary<str, object?> dict);
}

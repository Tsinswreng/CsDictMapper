namespace Tsinswreng.CsDictMapper;

using System;
using System.Collections.Generic;

public interface IDictSerializable{
	public IDictionary<str, obj?> SerializeToDict();
	public void DeserializeFromDict(Type Type, IDictionary<str, obj?> dict);
}

using System;
using System.Collections.Generic;
namespace Tsinswreng.CsDictMapper;

public  partial interface IDictMapperDeep{
	public IDictionary<str, object?> ToDictDeepT<T>(T Obj);
	public IDictionary<str, object?> ToDictDeep(Type Type, object? Obj);

	public IDictionary<str, Type> GetTypeDictDeepT<T>();
	public IDictionary<str, Type> GetTypeDictDeep(Type Type);

	public T AssignDeepT<T> (T obj, IDictionary<str, object?> dict);
	public object AssignDeep(Type Type, object? obj, IDictionary<str, object?> dict);
}

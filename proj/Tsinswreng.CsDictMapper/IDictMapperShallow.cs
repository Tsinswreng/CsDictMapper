using System;
using System.Collections.Generic;
namespace Tsinswreng.CsDictMapper;

public interface IDictMapperShallow{
	public IDictionary<Type, IDictMapperForOneType> Type_Mapper{get;set;}
	public IDictionary<str, object?> ToDictShallowT<T>(T Obj);
	public IDictionary<str, object?> ToDictShallow(Type Type, object? Obj);

	public IDictionary<str, Type> GetTypeDictShallowT<T>();
	public IDictionary<str, Type> GetTypeDictShallow(Type Type);

	public T AssignShallowT<T> (T obj, IDictionary<str, object?> dict);
	public object AssignShallow(Type Type, object? obj, IDictionary<str, object?> dict);
}


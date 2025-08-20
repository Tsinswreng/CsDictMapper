using System;
using System.Collections.Generic;
namespace Tsinswreng.CsDictMapper;

public  partial interface IDictMapperShallow{
	public IDictionary<Type, IDictMapperForOneType> Type_Mapper{get;set;}
	public IDictionary<str, obj?> ToDictShallowT<T>(T Obj);
	public IDictionary<str, obj?> ToDictShallow(Type Type, obj? Obj);

	public IDictionary<str, Type> GetTypeDictShallowT<T>();
	public IDictionary<str, Type> GetTypeDictShallow(Type Type);

	public T AssignShallowT<T> (T Obj, IDictionary<str, obj?> Dict);
	public obj AssignShallow(Type Type, obj? Obj, IDictionary<str, obj?> Dict);
}


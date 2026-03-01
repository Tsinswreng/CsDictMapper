using System;
using System.Collections.Generic;
namespace Tsinswreng.CsDictMapper;

public partial interface IDictMapperShallow{
	public IDictionary<Type, IDictMapperForOneType> Type_Mapper{get;set;}
	public IDictionary<str, obj?> ToDictShallowT<T>(T Obj);
	public IDictionary<str, obj?> ToDictShallow(Type Type, obj? Obj);

	public IDictionary<str, Type> GetTypeDictShallowT<T>();
	public IDictionary<str, Type> GetTypeDictShallow(Type Type);

	/// 易發空指針異常。發旹試察 對象中叵潙null之值類型字段 對應在字典中 潙null否
	public T AssignShallowT<T> (T Obj, IDictionary<str, obj?> Dict);
	/// 易發空指針異常。發旹試察 對象中叵潙null之值類型字段 對應在字典中 潙null否
	public obj AssignShallow(Type Type, obj? Obj, IDictionary<str, obj?> Dict);
}


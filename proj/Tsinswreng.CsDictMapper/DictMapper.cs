using System;
using System.Collections.Generic;
namespace Tsinswreng.CsDictMapper;

public partial class DictMapper
	:IDictMapperShallow
{

	public IDictionary<Type, IDictMapperForOneType> Type_Mapper { get; set; }
		= new Dictionary<Type, IDictMapperForOneType>();

	[Impl]
	public IDictionary<str, obj?> ToDictShallow(Type Type, obj? obj) {
		if (!Type_Mapper.TryGetValue(Type, out var Mapper)) {
			throw new ArgumentException($"No mapper found for type {Type}");
		}
		if (obj == null) {
			throw new ArgumentNullException("obj is null");
		}
		return Mapper.ToDictShallow(obj);
	}

	[Impl]
	public IDictionary<str, obj?> ToDictShallowT<T>(T obj) {
		return ToDictShallow(typeof(T), obj);
	}
	// public IDictionary<str, obj?> ToDictShallowT<T>(in T obj) {
	// 	return ToDictShallow(typeof(T), obj);
	// }

	[Impl]
	public IDictionary<str, Type> GetTypeDictShallow(Type Type){
		if(!Type_Mapper.TryGetValue(Type, out var Mapper)){
			throw new ArgumentException($"No mapper found for type {Type}");
		}
		return Mapper.GetTypeDictShallow();
	}
	[Impl]
	public IDictionary<str, Type> GetTypeDictShallowT<T>() {
		return GetTypeDictShallow(typeof(T));
	}

	[Impl]
	public obj AssignShallow(Type Type, obj? obj, IDictionary<str, obj?> dict){
		if(!Type_Mapper.TryGetValue(Type, out var Mapper)){
			throw new ArgumentException($"No mapper found for type {Type}");
		}
		if(obj == null){
			throw new ArgumentNullException("obj is null");
		}
		return Mapper.AssignShallow(obj, dict);
	}

	[Impl]
	public T AssignShallowT<T> (T obj, IDictionary<str, obj?> dict){
		return (T)AssignShallow(typeof(T), obj, dict);
	}

	// public IDictionary<str, obj?> ToDictDeep(Type Type, obj? obj){
	// 	if(!Type_Mapper.TryGetValue(Type, out var Mapper)){
	// 		throw new ArgumentException($"No mapper found for type {Type}");
	// 	}
	// 	if(obj == null){
	// 		throw new ArgumentNullException("obj is null");
	// 	}
	// 	var shallowDict = Mapper.ToDictShallow(obj);

	// 	foreach(var KvP in shallowDict){
	// 		str k = KvP.Key;
	// 		obj? v = KvP.Value;
	// 		if(v == null){continue;}
	// 		var type = v.GetType();


	// 	}
	// }
}

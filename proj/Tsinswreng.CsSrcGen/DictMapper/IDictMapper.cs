using System;
using System.Collections;
using System.Collections.Generic;
using Tsinswreng.CsSrcGen.Dict;

namespace Tsinswreng.CsSrcGen.DictMapper;

public interface IDictMapper{
	public IDictionary<Type, IDictMapperForOneType> Type_Mapper{get;set;}
	public IDictionary<str, object?> ToDictT<T>(T obj);
	public IDictionary<str, Type> GetTypeDictT<T>();
	public T AssignT<T> (T obj, IDictionary<str, object?> dict);
}

public class DictMapper : IDictMapper{
	public virtual IDictionary<Type, IDictMapperForOneType> Type_Mapper{get;set;}
		= new Dictionary<Type, IDictMapperForOneType>();
	public virtual IDictionary<str, object?> ToDictT<T>(T obj){
		if(!Type_Mapper.TryGetValue(typeof(T), out var Mapper)){
			throw new ArgumentException($"No mapper found for type {typeof(T)}");
		}
		if(obj == null){
			throw new ArgumentNullException("obj is null");
		}
		return Mapper.ToDict(obj);
	}
	public virtual IDictionary<str, Type> GetTypeDictT<T>(){
		if(!Type_Mapper.TryGetValue(typeof(T), out var Mapper)){
			throw new ArgumentException($"No mapper found for type {typeof(T)}");
		}
		return Mapper.GetTypeDict();
	}
	public virtual T AssignT<T> (T obj, IDictionary<str, object?> dict){
		if(!Type_Mapper.TryGetValue(typeof(T), out var Mapper)){
			throw new ArgumentException($"No mapper found for type {typeof(T)}");
		}
		if(obj == null){
			throw new ArgumentNullException("obj is null");
		}
		return (T)Mapper.Assign(obj, dict);
	}
}

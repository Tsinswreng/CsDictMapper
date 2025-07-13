using Microsoft.CodeAnalysis;

namespace Tsinswreng.CsDictMapper.SrcGen;

public interface IParamDictType{
	public INamedTypeSymbol TargetTypeSymbol{get;set;}
	public bool Recursive{get;set;}
}


public class ParamDictType: IParamDictType{
	public ParamDictType(INamedTypeSymbol TargetType, bool Recursive=false){
		TargetTypeSymbol = TargetType;
		this.Recursive = Recursive;
	}
	public INamedTypeSymbol TargetTypeSymbol{get;set;}
	public bool Recursive{get;set;} = false;
}

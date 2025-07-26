using Microsoft.CodeAnalysis;

namespace Tsinswreng.CsDictMapper.SrcGen;

public  partial interface IParamDictType{
	public INamedTypeSymbol TargetTypeSymbol{get;set;}
	public bool Recursive{get;set;}
}


public  partial class ParamDictType: IParamDictType{
	public ParamDictType(INamedTypeSymbol TargetType, bool Recursive=false){
		TargetTypeSymbol = TargetType;
		this.Recursive = Recursive;
	}
	public INamedTypeSymbol TargetTypeSymbol{get;set;}
	public bool Recursive{get;set;} = false;
}

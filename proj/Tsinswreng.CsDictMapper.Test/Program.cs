// See https://aka.ms/new-console-template for more information
using Tsinswreng.CsDictMapper.DictMapper.Attributes;
System.Console.WriteLine("Hello, World!");




namespace Root{


namespace NsA{
	public class ClassA{
		public int Int{get;set;} = 1;
	}
}

namespace NsB{
	public class ClassB{
		public string String{get;set;} = "String";
	}
}


[DictType(typeof(NsA.ClassA))]
[DictType(typeof(NsB.ClassB))]
public partial class UserDictCtx{
protected static UserDictCtx? _Inst = null;
public static UserDictCtx Inst => _Inst??= new UserDictCtx();

}


}


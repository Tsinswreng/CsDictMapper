// See https://aka.ms/new-console-template for more information
using Tsinswreng.CsSrcGen.DictMapper.Attributes;
System.Console.WriteLine("Hello, World!");


var D = new Root.UserDictCtx();


namespace Root{


namespace NsA{
	public class ClassA{

	}
}

namespace NsB{
	public class ClassB{

	}
}


[DictType(typeof(NsA.ClassA))]
[DictType(typeof(NsB.ClassB))]
public partial class UserDictCtx{
protected static UserDictCtx? _Inst = null;
public static UserDictCtx Inst => _Inst??= new UserDictCtx();

}


}


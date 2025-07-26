using Tsinswreng.CsDictMapper;

namespace UserDictCtxNs{



namespace NsA{
	public  partial class ClassA{
		public  partial class InnerClassC{

		}

	}
}

namespace NsB{
	public  partial class ClassB{

	}
	public  partial class ClassD{

	}
}


[DictType(typeof(NsA.ClassA))]
[DictType(typeof(NsB.ClassB))]
public partial class UserDictCtx{

}

}//~Ns


//example (deprecated)
#if false
#region Generated
// in UserDictCtxNs.DictCtx-NsA.ClassA.cs

namespace UserDictCtxNs._.NsA.ClassA{
	public  partial class DictMapper :IDictMapperForOneType{
		protected static DictMapper? _Inst = null;
		public static DictMapper Inst => _Inst??= new DictMapper();

		//...
	}
}

// in UserDictCtxNs.DictCtx-NsB.ClassB.cs

namespace UserDictCtxNs{
	namespace NsB{
		public  partial class DictMapper :IDictMapperForOneType{
			protected static DictMapper? _Inst = null;
			public static DictMapper Inst => _Inst??= new DictMapper();
			//...
		}
	}
}

// in UserDictCtxNs.DictCtx-.cs
namespace UserDictCtxNs{
	public partial class UserDictCtx:DictMapper, IDictMapper{
		public UserDictCtx(){
			Type_Mapper.Add(typeof(NsA.ClassA), NsA.DictMapper.Inst);
			Type_Mapper.Add(typeof(NsB.ClassB), NsB.DictMapper.Inst);
			//......
		}
	}

}

#endregion Generated
#endif

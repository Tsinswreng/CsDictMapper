using Tsinswreng.CsSrcGen.Dict;

namespace UserDictCtxNs{
using Tsinswreng.CsSrcGen.DictMapper.Attributes;
using Tsinswreng.CsSrcGen.DictMapper;

namespace NsA{
	public class ClassA{
		public class InnerClassC{

		}

	}
}

namespace NsB{
	public class ClassB{

	}
	public class ClassD{

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
	public class DictMapper :IDictMapperForOneType{
		protected static DictMapper? _Inst = null;
		public static DictMapper Inst => _Inst??= new DictMapper();

		//...
	}
}

// in UserDictCtxNs.DictCtx-NsB.ClassB.cs

namespace UserDictCtxNs{
	namespace NsB{
		public class DictMapper :IDictMapperForOneType{
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

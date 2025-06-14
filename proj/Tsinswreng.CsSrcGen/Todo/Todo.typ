=
[2025-06-13T22:11:54.193+08:00_W24-5]
生成ʹ類ʹ可訪問性修飾符當與用戶定義厎一致
```cs
[DictType(typeof(SchemaHistory))]
internal partial class DictCtx{}
```
目前用internal即報錯、須用public


=
[2025-06-14T19:54:08.635+08:00_W24-6]

- 徂支持多個DictMapper合併
- 模仿TblMgr之泛型字典
- 測試頂層命名空間

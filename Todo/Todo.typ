=
[2025-06-13T22:11:54.193+08:00_W24-5]
生成ʹ類ʹ可訪問性修飾符當與用戶定義厎一致
```cs
[DictType(typeof(SchemaHistory))]
internal partial class DictCtx{}
```
目前用internal即報錯、須用public

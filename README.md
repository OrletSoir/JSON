# JSON
JSON parser for C#

----

This is a very simplistic, yet effective JSON parser for C# that I have written for one of my personal projects a long while ago, when there actually weren't all that many JSON libraries around.

It is tested to work on .NET 3.5 platform, but, theoretically, could be back-ported into 2.0 or even below, with little modification, since it mostly uses base .NET types (collections, dictionaries, lists).

----

Parser is `Json` class itself exports the main static member `Parse`, which generates a set of linked `IJsonVariable` objects, whose type will be stored internally and can be determined via `IJsonVariable.Type` property, and actual values via one of the appropriate `IJsonVariable.As[Type]` methods.

NB: `JsonSet` and `JsonArray` also implement `IJsonVariable` interface, but cannot be easily converted to scalar values, however vice-versa is possible.

Collection is self-stringifying via `IJsonVariable`'s `ToJsonString()` method, should you need it.

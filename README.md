#AtmoSerialize

A library to load and save Atmosphir `.atmo` files.

Currently, it is very low level, with a bare serializer. This means that object properties (like rulebook and triggers) are not type-safe.


##Examples

```csharp
using (var stream = File.OpenRead("draft.atmo")) {
	var map = AtmoConvert.Deserialize(stream);
	Console.WriteLine(map.Version);
}

using (var stream = File.OpenWrite("out.atmo")) {
	var map = new Map(5);
	var item = new MapItem("mi_flag_start", new Vector(0,0,0), new Vector(0,0,0), 1f);
	map.Resources.Add(item.ResourceName);
	map.Items.Add(item);

	AtmoConvert.Serialize(stream, map, true); //boolean for compression
}
```

Asynchronous versions of `AtmoConvert.Serialize` and `AtmoConvert.Deserialize` are also available.
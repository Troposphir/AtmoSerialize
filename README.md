#AtmoSerialize

A library to load and save Atmosphir `.atmo` files. Features type-safe map item properties.

##Examples

```csharp
using (var stream = File.OpenRead("draft.atmo")) {
	var map = AtmoConvert.Deserialize(stream);
	Console.WriteLine(map.First().ResourceName);
}

using (var stream = File.OpenWrite("out.atmo")) {
	var map = new Map(5);
	var item = new Item("mi_flag_start") {
        Position = Vector.Zero,
        Rotation = Vector.Zero,
        Scale = 1f
    };
	map.Add(item);

	AtmoConvert.Serialize(stream, map, true); //boolean for compression
}
```

Asynchronous versions of `AtmoConvert.Serialize` and `AtmoConvert.Deserialize` are also available.
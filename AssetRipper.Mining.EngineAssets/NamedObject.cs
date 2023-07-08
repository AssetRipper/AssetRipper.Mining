namespace AssetRipper.Mining.EngineAssets;

public abstract record class NamedObject : Object
{
	public string Name { get; set; } = "";
}

namespace AssetRipper.Mining.PredefinedAssets;

public abstract record class NamedObject : Object
{
	public string Name { get; set; } = "";
}

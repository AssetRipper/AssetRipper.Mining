namespace AssetRipper.Mining.PredefinedAssets;

public abstract record class NamedObject : Object
{
	public required string Name { get; init; }
}

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Sprite : NamedObject
{
	public required string? Texture { get; init; }
}
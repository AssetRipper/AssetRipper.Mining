namespace AssetRipper.Mining.PredefinedAssets;

public record class Texture2D : NamedObject
{
	public required int Width { get; init; }
	public required int Height { get; init; }
}

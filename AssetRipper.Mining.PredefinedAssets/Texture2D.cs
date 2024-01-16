namespace AssetRipper.Mining.PredefinedAssets;

public record class Texture2D : NamedObject
{
	public required int Width { get; init; }
	public required int Height { get; init; }

	public Texture2D()
	{
	}

	[SetsRequiredMembers]
	public Texture2D(string name, int width, int height) : base(name)
	{
		Width = width;
		Height = height;
	}
}

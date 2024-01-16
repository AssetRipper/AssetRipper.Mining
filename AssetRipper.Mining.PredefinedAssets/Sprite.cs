namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Sprite : NamedObject
{
	public required string? Texture { get; init; }

	public Sprite()
	{
	}

	[SetsRequiredMembers]
	public Sprite(string name, string? texture) : base(name)
	{
		Texture = texture;
	}
}

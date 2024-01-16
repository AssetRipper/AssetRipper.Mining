namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Cubemap : Texture2D
{
	public Cubemap()
	{
	}

	[SetsRequiredMembers]
	public Cubemap(string name, int width, int height) : base(name, width, height)
	{
	}
}

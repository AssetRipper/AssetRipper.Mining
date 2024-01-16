namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Material : NamedObject
{
	public required string? Shader { get; init; }

	public Material()
	{
	}

	[SetsRequiredMembers]
	public Material(string name, string? shader) : base(name)
	{
		Shader = shader;
	}
}

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Mesh : NamedObject
{
	public required uint VertexCount { get; init; }
}

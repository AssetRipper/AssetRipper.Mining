namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Mesh : NamedObject
{
	public required int VertexCount { get; init; }
	public required int SubMeshCount { get; init; }
}

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Mesh : NamedObject
{
	public required int VertexCount { get; init; }
	public required int SubMeshCount { get; init; }
	public required AxisAlignedBoundingBox LocalAABB { get; init; }

	public Mesh()
	{
	}

	[SetsRequiredMembers]
	public Mesh(string name, int vertexCount, int subMeshCount, AxisAlignedBoundingBox localAabb) : base(name)
	{
		VertexCount = vertexCount;
		SubMeshCount = subMeshCount;
		LocalAABB = localAabb;
	}
}

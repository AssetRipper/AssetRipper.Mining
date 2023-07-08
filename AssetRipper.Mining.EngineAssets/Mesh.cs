using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

public sealed record class Mesh : NamedObject
{
	public uint VertexCount { get; set; }

	[JsonIgnore]
	public override int TypeID
	{
		get => 43;
		set { }
	}
}

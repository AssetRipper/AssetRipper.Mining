using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

public sealed record class Material : NamedObject
{
	public string? Shader { get; set; }

	[JsonIgnore]
	public override int TypeID
	{
		get => 21;
		set { }
	}
}

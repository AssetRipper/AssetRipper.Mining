using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

public sealed record class GenericBehaviour : Behaviour
{
	[JsonInclude]
	public override int TypeID { get; set; }
}
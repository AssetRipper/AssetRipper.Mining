using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class GenericComponent : Component
{
	[JsonInclude]
	public override int TypeID { get; set; }
}

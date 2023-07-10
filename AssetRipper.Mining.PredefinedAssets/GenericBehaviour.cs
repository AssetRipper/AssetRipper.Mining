using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class GenericBehaviour : Behaviour
{
	[JsonInclude]
	public new required int TypeID { get; init; }
}
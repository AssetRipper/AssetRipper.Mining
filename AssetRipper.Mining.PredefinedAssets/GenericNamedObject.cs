using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class GenericNamedObject : NamedObject
{
	[JsonInclude]
	public new required int TypeID { get; init; }
}
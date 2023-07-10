using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Cubemap : Texture2D
{
	[JsonIgnore]
	public override int TypeID
	{
		get => 89;
		set { }
	}
}
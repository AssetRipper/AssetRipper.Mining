using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public record class Texture2D : NamedObject
{
	public int Width { get; set; }
	public int Height { get; set; }

	[JsonIgnore]
	public override int TypeID
	{
		get => 28;
		set { }
	}
}

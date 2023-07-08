using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

public sealed record class TextAsset : NamedObject
{
	/// <summary>
	/// The length the text in bytes.
	/// </summary>
	public int Length { get; set; }

	[JsonIgnore]
	public override int TypeID
	{
		get => 49;
		set { }
	}
}
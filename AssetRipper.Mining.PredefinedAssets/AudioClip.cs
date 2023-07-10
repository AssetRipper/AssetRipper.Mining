using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class AudioClip : NamedObject
{
	/// <summary>
	/// The number of channels in the audio clip.<br />
	/// 5.0.0 to Max
	/// </summary>
	public int Channels { get; set; }
	/// <summary>
	/// The sample frequency of the clip in Hertz.<br />
	/// 5.0.0 to Max
	/// </summary>
	public int Frequency { get; set; }
	/// <summary>
	/// The length of the audio clip in seconds.<br />
	/// 5.0.0 to Max
	/// </summary>
	public float Length { get; set; }

	[JsonIgnore]
	public override int TypeID
	{
		get => 83;
		set { }
	}
}

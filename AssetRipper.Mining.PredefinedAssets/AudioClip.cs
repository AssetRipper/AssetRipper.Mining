namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class AudioClip : NamedObject
{
	/// <summary>
	/// The number of channels in the audio clip.<br />
	/// 5.0.0 to Max
	/// </summary>
	public required int Channels { get; init; }

	/// <summary>
	/// The sample frequency of the clip in Hertz.<br />
	/// 5.0.0 to Max
	/// </summary>
	public required int Frequency { get; init; }

	/// <summary>
	/// The length of the audio clip in seconds.<br />
	/// 5.0.0 to Max
	/// </summary>
	public required float Length { get; init; }
}

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class TextAsset : NamedObject
{
	/// <summary>
	/// The length the text in bytes.
	/// </summary>
	public required int Length { get; init; }
}
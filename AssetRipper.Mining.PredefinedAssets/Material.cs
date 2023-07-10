using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Material : NamedObject
{
	public required string? Shader { get; init; }
}

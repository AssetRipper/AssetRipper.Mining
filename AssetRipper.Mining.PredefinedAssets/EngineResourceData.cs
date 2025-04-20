using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public readonly record struct EngineResourceData
{
	public Dictionary<long, Object> DefaultResources { get; }
	public Dictionary<long, Object> ExtraResources { get; }

	public EngineResourceData()
	{
		DefaultResources = new();
		ExtraResources = new();
	}

	[JsonConstructor]
	public EngineResourceData(Dictionary<long, Object>? defaultResources, Dictionary<long, Object>? extraResources)
	{
		DefaultResources = defaultResources ?? new();
		ExtraResources = extraResources ?? new();
	}

	public readonly string ToJson()
	{
		return JsonSerializer.Serialize(this, MiningSerializerContext.Default.EngineResourceData);
	}

	public static EngineResourceData FromJson(string text)
	{
		return string.IsNullOrEmpty(text)
			? new()
			: JsonSerializer.Deserialize(text, MiningSerializerContext.Default.EngineResourceData);
	}
}

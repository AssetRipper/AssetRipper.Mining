using System.Text.Json;

namespace AssetRipper.Mining.PredefinedAssets;

public readonly record struct EngineResourceData(Dictionary<long, Object> DefaultResources, Dictionary<long, Object> ExtraResources)
{
	public EngineResourceData() : this(new(), new())
	{
	}

	public readonly string ToJson()
	{
		return JsonSerializer.Serialize(this, InternalSerializerContext.Default.EngineResourceData);
	}

	public static EngineResourceData FromJson(string text)
	{
		return JsonSerializer.Deserialize(text, InternalSerializerContext.Default.EngineResourceData);
	}
}

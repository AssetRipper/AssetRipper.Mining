using System.Text.Json;

namespace AssetRipper.Mining.EngineAssets;

public record struct EngineAssetsData(Dictionary<long, AssetInfo> DefaultResources, Dictionary<long, AssetInfo> ExtraResources)
{
	public EngineAssetsData() : this(new(), new())
	{
	}

	public readonly string ToJson()
	{
		return JsonSerializer.Serialize(this, EngineAssetsDataSerializerContext.Default.EngineAssetsData);
	}

	public static EngineAssetsData FromJson(string text)
	{
		return JsonSerializer.Deserialize(text, EngineAssetsDataSerializerContext.Default.EngineAssetsData);
	}
}
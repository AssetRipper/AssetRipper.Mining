using System.Text.Json;

namespace AssetRipper.Mining.EngineAssets;

public partial record struct EngineAssetsData(Dictionary<long, Object> DefaultResources, Dictionary<long, Object> ExtraResources)
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
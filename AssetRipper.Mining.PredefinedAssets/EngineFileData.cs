using System.Text.Json;

namespace AssetRipper.Mining.PredefinedAssets;

public partial record struct EngineFileData(Dictionary<long, Object> DefaultResources, Dictionary<long, Object> ExtraResources)
{
	public EngineFileData() : this(new(), new())
	{
	}

	public readonly string ToJson()
	{
		return JsonSerializer.Serialize(this, InternalSerializerContext.Default.EngineFileData);
	}

	public static EngineFileData FromJson(string text)
	{
		return JsonSerializer.Deserialize(text, InternalSerializerContext.Default.EngineFileData);
	}
}
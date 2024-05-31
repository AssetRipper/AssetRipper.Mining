using System.Text.Json;

namespace AssetRipper.Mining.PredefinedAssets;

public readonly record struct ReferenceAssemblyData(List<string> Unity, List<string>? Mono2, List<string>? Mono4, List<string>? CoreCLR)
{
	public string ToJson()
	{
		return JsonSerializer.Serialize(this, InternalSerializerContext.Default.ReferenceAssemblyData);
	}

	public static ReferenceAssemblyData FromJson(string json)
	{
		return JsonSerializer.Deserialize<ReferenceAssemblyData>(json, InternalSerializerContext.Default.ReferenceAssemblyData);
	}
}

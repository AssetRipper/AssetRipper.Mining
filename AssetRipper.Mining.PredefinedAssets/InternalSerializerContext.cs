using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(EngineFileData))]
internal sealed partial class InternalSerializerContext : JsonSerializerContext
{
	public static InternalSerializerContext Get()
	{
		return Default;
	}
}
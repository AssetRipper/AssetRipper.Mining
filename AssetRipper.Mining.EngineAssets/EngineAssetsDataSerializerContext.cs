using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(EngineAssetsData))]
[JsonSerializable(typeof(KeyValuePair<string, string>), TypeInfoPropertyName = "PrimitiveField")]
[JsonSerializable(typeof(KeyValuePair<string, string[]>), TypeInfoPropertyName = "ArrayField")]
internal sealed partial class EngineAssetsDataSerializerContext : JsonSerializerContext
{
}
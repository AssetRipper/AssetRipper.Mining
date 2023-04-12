using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(EngineAssetsData))]
internal sealed partial class EngineAssetsDataSerializerContext : JsonSerializerContext
{
}
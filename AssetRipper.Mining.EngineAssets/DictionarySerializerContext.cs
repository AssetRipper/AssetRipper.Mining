using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Dictionary<long, AssetInfo>))]
internal sealed partial class DictionarySerializerContext : JsonSerializerContext
{
}
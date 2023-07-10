using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(EngineResourceData))]
[JsonSerializable(typeof(UnityPackageData))]
internal sealed partial class InternalSerializerContext : JsonSerializerContext
{
}
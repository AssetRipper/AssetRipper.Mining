using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true)]
[JsonSerializable(typeof(EngineResourceData))]
[JsonSerializable(typeof(UnityPackageData))]
[JsonSerializable(typeof(ReferenceAssemblyData))]
public sealed partial class MiningSerializerContext : JsonSerializerContext
{
}

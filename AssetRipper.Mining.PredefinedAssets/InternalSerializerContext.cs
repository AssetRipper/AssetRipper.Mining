using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true)]
[JsonSerializable(typeof(EngineResourceData))]
[JsonSerializable(typeof(UnityPackageData))]
[JsonSerializable(typeof(ReferenceAssemblyData))]
internal sealed partial class InternalSerializerContext : JsonSerializerContext
{
}

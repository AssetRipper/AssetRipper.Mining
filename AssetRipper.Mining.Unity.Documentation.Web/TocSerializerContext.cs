using System.Text.Json.Serialization;

namespace AssetRipper.Mining.Unity.Documentation.Web;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(TocNode))]
internal partial class TocSerializerContext : JsonSerializerContext
{
}
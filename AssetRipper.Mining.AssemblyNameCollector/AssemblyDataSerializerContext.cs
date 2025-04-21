using AssetRipper.Primitives;
using System.Text.Json.Serialization;

namespace AssetRipper.Mining.AssemblyNameCollector;

[JsonSourceGenerationOptions(WriteIndented = true, IndentCharacter = '\t', IndentSize = 1, NewLine = "\n")]
[JsonSerializable(typeof(AssemblyDataFile))]
internal sealed partial class AssemblyDataSerializerContext : JsonSerializerContext
{
}

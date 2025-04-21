using AssetRipper.Primitives;
using System.Text.Json;

namespace AssetRipper.Mining.AssemblyNameCollector;

public readonly record struct AssemblyDataFile(IReadOnlyList<UnityVersion> Versions, IReadOnlyList<KeyValuePair<UnityVersion, AssemblyData>> Assemblies)
{
	public void Write(string path)
	{
		using FileStream stream = File.Create(path);
		JsonSerializer.Serialize(stream, this, AssemblyDataSerializerContext.Default.AssemblyDataFile);
	}
}

using AssetRipper.Primitives;

namespace AssetRipper.Mining.AssemblyNameCollector;

internal static class Program
{
	static void Main(string[] args)
	{
		// Extract data
		List<UnityVersion> versions = new();
		List<KeyValuePair<UnityVersion, AssemblyData>> list = new();
		foreach ((string directory, UnityVersion version) in GetRootDirectoriesInOrder(args[0]))
		{
			Console.WriteLine(directory);
			versions.Add(version);
			AssemblyData data = AssemblyData.FromFolder(directory);
			if (list.Count is 0 || !list[^1].Value.Equals(data))
			{
				list.Add(new(version, data));
			}
		}

		// Write data
		new AssemblyDataFile(versions, list).Write(args[1]);

		Console.WriteLine("Done!");
	}

	static IEnumerable<(string, UnityVersion)> GetRootDirectoriesInOrder(string parentDirectory)
	{
		return Directory.EnumerateDirectories(parentDirectory)
			.Select(d => (d, UnityVersion.Parse(Path.GetFileName(d))))
			.OrderBy(p => p.Item2);
	}
}

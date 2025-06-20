using AngleSharp;
using AngleSharp.Dom;
using AssetRipper.Primitives;
using System.Diagnostics;
using System.Formats.Tar;
using System.IO.Compression;

namespace AssetRipper.Mining.AssemblyNameCollector;

public readonly record struct AssemblyData(IReadOnlyList<string> Mono2, IReadOnlyList<string> Mono4, IReadOnlyList<string> Unity, IReadOnlyList<KeyValuePair<string, UnityGuid>> UnityExtensions)
{
	public static AssemblyData FromFolder(string root)
	{
		IReadOnlyList<string> mono2 = GetAssemblies_Mono2(root);
		IReadOnlyList<string> mono4 = GetAssemblies_Mono4(root);
		IReadOnlyList<string> unity = GetAssemblies_Unity(root);
		IReadOnlyList<KeyValuePair<string, UnityGuid>> unityExtensions = GetUnityExtensions(root).Concat(GetUnityPackages(root)).OrderBy(pair => pair.Key).ToList();
		return new AssemblyData(mono2, mono4, unity, unityExtensions);
	}

	public bool Equals(AssemblyData other)
	{
		return Mono2.SequenceEqual(other.Mono2) && Mono4.SequenceEqual(other.Mono4) && Unity.SequenceEqual(other.Unity) && UnityExtensions.SequenceEqual(other.UnityExtensions);
	}

	public override int GetHashCode()
	{
		HashCode hash = new();
		foreach (string assembly in Mono2)
		{
			hash.Add(assembly);
		}
		foreach (string assembly in Mono4)
		{
			hash.Add(assembly);
		}
		foreach (string assembly in Unity)
		{
			hash.Add(assembly);
		}
		foreach (KeyValuePair<string, UnityGuid> pair in UnityExtensions)
		{
			hash.Add(pair);
		}
		return hash.ToHashCode();
	}

	static IReadOnlyList<string> GetAssemblies_Mono2(string root)
	{
		return GetAssemblies(root,
			"Editor/Data/Mono/lib/mono/unity",
			"Editor/Data/Mono/lib/mono/2.0",
			"Editor/Data/Frameworks/Mono.framework");
	}

	static IReadOnlyList<string> GetAssemblies_Mono4(string root)
	{
		return GetAssemblies(root,
			"Editor/Data/MonoBleedingEdge/lib/mono/unityjit-win32",
			"Editor/Data/MonoBleedingEdge/lib/mono/unityjit",
			"Editor/Data/MonoBleedingEdge/lib/mono/unity",
			"Editor/Data/MonoBleedingEdge/lib/mono/4.5",
			"Editor/Data/MonoBleedingEdge/lib/mono/4.0");
	}

	static IReadOnlyList<string> GetAssemblies_Unity(string root)
	{
		if (Directory.Exists(Path.Combine(root, "Editor/Data/Frameworks")))
		{
			// Unity 2
			return ["UnityEngine"];
		}

		return GetAssemblies(root,
			"Editor/Data/PlaybackEngines/windowsstandalonesupport/Variations/win64_nondevelopment/Data/Managed",
			"Editor/Data/PlaybackEngines/windowsstandalonesupport/Variations/win64_nondevelopment_mono/Data/Managed",
			"Editor/Data/PlaybackEngines/windowsstandalonesupport/Variations/win64_player_nondevelopment_mono/Data/Managed",
			"Editor/Data/PlaybackEngines/windows64standaloneplayer/Managed",
			"Editor/Data/PlaybackEngines/windowsstandaloneplayer/Managed");
	}

	static IReadOnlyList<string> GetAssemblies(string root, params ReadOnlySpan<string> relativePaths)
	{
		foreach (string relativePath in relativePaths)
		{
			string path = Path.Combine(root, relativePath);
			if (Directory.Exists(path))
			{
				List<string> result = GetManagedAssemblies(path).ToList();
				string facadeDirectory = Path.Combine(path, "Facades");
				if (Directory.Exists(facadeDirectory))
				{
					result.AddRange(GetManagedAssemblies(facadeDirectory).Where(a => !result.Contains(a)));
				}
				return result;
			}
		}
		return [];
	}

	static IEnumerable<string> GetManagedAssemblies(string directory)
	{
		return Directory.EnumerateFiles(directory, "*.dll", SearchOption.AllDirectories)
			.Select(Path.GetFileNameWithoutExtension)
			.Where(s => !string.IsNullOrEmpty(s))
			.Distinct()
			.Order()!;
	}

	static IReadOnlyList<KeyValuePair<string, UnityGuid>> GetUnityExtensions(string root)
	{
		string directory = Path.Combine(root, "Editor/Data/UnityExtensions");
		if (Directory.Exists(directory))
		{
			List<(string, UnityGuid)> extractedArtifacts = new();
			foreach (string file in Directory.EnumerateFiles(directory, "ivy.xml", SearchOption.AllDirectories))
			{
				string xml = File.ReadAllText(file);
				IDocument document = BrowsingContext.New(Configuration.Default.WithXml()).OpenAsync(req => req.Content(xml)).GetAwaiter().GetResult();

				foreach (IElement artifact in document.QuerySelectorAll("artifact"))
				{
					string? name = artifact.GetAttribute("name");
					string? type = artifact.GetAttribute("type");
					string? ext = artifact.GetAttribute("ext");
					string? guidString = artifact.GetAttribute("e:guid"); // namespaced attribute

					if (string.IsNullOrEmpty(name) || type is not "dll" || ext is not "dll" || string.IsNullOrEmpty(guidString) || !TryParseGuid(guidString, out UnityGuid guid))
					{
						continue;
					}

					extractedArtifacts.Add((name, guid));
				}
			}
			Dictionary<string, List<(string, UnityGuid)>> dictionary = new();
			foreach ((string path, UnityGuid guid) in extractedArtifacts)
			{
				string name = Path.GetFileName(path);
				if (!dictionary.TryGetValue(name, out List<(string, UnityGuid)>? newList))
				{
					newList = new();
					dictionary.Add(name, newList);
				}
				newList.Add((path, guid));
			}
			List<KeyValuePair<string, UnityGuid>> finalList = new();
			foreach ((string name, List<(string, UnityGuid)> subList) in dictionary.OrderBy(p => p.Key))
			{
				Debug.Assert(subList.Count > 0);
				UnityGuid guid;
				if (subList.Count == 1)
				{
					guid = subList[0].Item2;
				}
				else
				{
					guid = subList.FirstOrDefault(pair => pair.Item1 == name).Item2;
					if (guid != default)
					{
						goto Found;
					}

					ReadOnlySpan<string> starts =
					[
						"Standalone/", // Preferred over Editor/
						"Runtime/", // Preferred over RuntimeEditor/

						// NUnit
						"portable/",
						"nunit-standalone/",
					];
					foreach (string start in starts)
					{
						foreach ((string path, UnityGuid subGuid) in subList)
						{
							if (path.StartsWith(start, StringComparison.Ordinal))
							{
								guid = subGuid;
								goto Found;
							}
						}
					}
					throw new InvalidOperationException($"Failed to find a guid for {name}");
				}
			Found:
				finalList.Add(new(name, guid));
			}
			return finalList;
		}
		return [];
	}

	static IReadOnlyList<KeyValuePair<string, UnityGuid>> GetUnityPackages(string root)
	{
		// Unity 2017.2 introduced packages, which are stored in a compressed format.

		if (root.EndsWith("2017.4.34f1", StringComparison.Ordinal))
		{
			// Unity 2017.4.34f1 uses a weird compression format for packages.
			// It's the only version that does this, so we redirect to the subsequent version.
			return GetUnityPackages(Path.Combine(Path.GetDirectoryName(root) ?? string.Empty, "2017.4.35f1"));
		}
		else if (!Path.GetFileName(root).StartsWith("2017.", StringComparison.Ordinal))
		{
			// Unity 2018.1 introduced the Package Manager UI, which makes package references no longer automatic.
			return [];
		}

		string directory = Path.Combine(root, "Editor/Data/Resources/PackageManager/Editor");
		if (!Directory.Exists(directory))
		{
			return [];
		}

		List<KeyValuePair<string, UnityGuid>> assemblyGuids = [];

		foreach (string file in Directory.EnumerateFiles(directory, "*.tgz"))
		{
			using FileStream fileStream = File.OpenRead(file);
			using GZipStream gzipStream = new(fileStream, CompressionMode.Decompress);

			MemoryStream tarStream = new();
			gzipStream.CopyTo(tarStream);
			tarStream.Position = 0;

			using TarReader reader = new(tarStream);
			TarEntry? entry;
			while ((entry = reader.GetNextEntry()) is not null)
			{
				string? entryPath = entry.Name;
				if (string.IsNullOrEmpty(entryPath) || !entryPath.EndsWith(".dll.meta", StringComparison.Ordinal))
				{
					continue;
				}

				string assemblyName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(entryPath)); // Remove .meta and .dll

				string text = new StreamReader(entry.DataStream!).ReadToEnd();

				string? guidString = text.Split('\n')
					.FirstOrDefault(line => line.StartsWith("guid: ", StringComparison.Ordinal))?
					.Substring("guid: ".Length)
					.Trim();

				if (!string.IsNullOrEmpty(guidString) && TryParseGuid(guidString, out UnityGuid guid))
				{
					assemblyGuids.Add(new KeyValuePair<string, UnityGuid>(assemblyName, guid));
				}
			}
		}

		return assemblyGuids;
	}

	private static bool TryParseGuid(string guidString, out UnityGuid guid)
	{
		// Todo: implement a proper solution upstream
		try
		{
			guid = UnityGuid.Parse(guidString);
			return true;
		}
		catch
		{
			guid = default;
			return false;
		}
	}
}

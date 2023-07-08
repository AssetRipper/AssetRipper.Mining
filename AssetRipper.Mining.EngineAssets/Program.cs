using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Diagnostics;

namespace AssetRipper.Mining.EngineAssets;

internal static class Program
{
	private const string DefaultResourcesName = "unity default resources";
	private const string ExtraResourcesName = "unity_builtin_extra";
	private const string ResourcesFolderPath = @"Editor/Data/Resources";

	public static void Main(string[] args)
	{
		string unityFolderPath = args[0];
		Dictionary<long, AssetInfo> defaultDictionary = ReadDictionary(Path.Combine(unityFolderPath, ResourcesFolderPath, DefaultResourcesName));
		Dictionary<long, AssetInfo> extraDictionary = ReadDictionary(Path.Combine(unityFolderPath, ResourcesFolderPath, ExtraResourcesName));
		Dictionary<int, int> typeIDs = defaultDictionary.Values.Union(extraDictionary.Values)
			.Select(a => a.TypeID).Distinct().Order()
			.ToDictionary(id => id, id => defaultDictionary.Values.Union(extraDictionary.Values).Count(a => a.TypeID == id));
		foreach ((int typeID, int count) in typeIDs)
		{
			Console.WriteLine($"{typeID,4} : {count,3}");
		}
		File.WriteAllText("engineassets.json", new EngineAssetsData(defaultDictionary, extraDictionary).ToJson());
		Console.WriteLine("Done!");
	}

	private static Dictionary<long, AssetInfo> ReadDictionary(string path)
	{
		return LoadAllAssetInfo(path).OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
	}

	private static IEnumerable<UnityAsset> LoadAllAssets(string path)
	{
		AssetsManager manager = new();
		AssetsFileInstance assetsFileInstance = manager.LoadAssetsFile(path, false);
		foreach (AssetFileInfo assetFileInfo in assetsFileInstance.file.AssetInfos)
		{
			yield return new UnityAsset(manager, assetsFileInstance, assetFileInfo);
		}
	}

	private static IEnumerable<KeyValuePair<long, AssetInfo>> LoadAllAssetInfo(string path)
	{
		foreach (UnityAsset asset in LoadAllAssets(path))
		{
			AssetInfo assetInfo;
			switch (asset.TypeID)
			{
				case 21://Material
					{
						UnityAsset shader = asset.GetAsset("m_Shader");
						KeyValuePair<string, string>[] primitiveFields = new[] { new KeyValuePair<string, string>("Shader", shader.Name) };
						assetInfo = new AssetInfo(asset.TypeID, asset.Name, primitiveFields);
					}
					break;
				case 28://Texture2D
					{
						KeyValuePair<string, string>[] primitiveFields = new[]
						{
							new KeyValuePair<string, string>("Height", asset.GetString("m_Height")),
							new KeyValuePair<string, string>("Width", asset.GetString("m_Width"))
						};
						assetInfo = new AssetInfo(asset.TypeID, asset.Name, primitiveFields);
					}
					break;
				case 43://Mesh
					{
						AssetTypeValueField vertexData = asset.GetBaseField().Get("m_VertexData");
						uint vertexCount = vertexData.IsDummy ? default : vertexData.Get("m_VertexCount").AsUInt;
						KeyValuePair<string, string>[] primitiveFields = new[] { new KeyValuePair<string, string>("VertexCount", vertexCount.ToString()) };
						assetInfo = new AssetInfo(asset.TypeID, asset.Name, primitiveFields);
					}
					break;
				case 48://Shader
					{
						AssetTypeValueField serializedShader = asset.GetBaseField().Get("m_ParsedForm");
						if (serializedShader.IsDummy)
						{
							goto default;
						}
						else
						{
							List<AssetTypeValueField> propsList = serializedShader.Get("m_PropInfo").Get("m_Props").Get("Array").Children;
							string[] propertyNames = propsList.Count == 0 ? Array.Empty<string>() : new string[propsList.Count];
							for (int i = 0; i < propsList.Count; i++)
							{
								propertyNames[i] = propsList[i].Get("m_Name").AsString ?? "";
							}
							KeyValuePair<string, string[]>[] arrayFields = new[]
							{
								new KeyValuePair<string, string[]>("PropertyNames", propertyNames)
							};
							assetInfo = new AssetInfo(asset.TypeID, asset.Name, Array.Empty<KeyValuePair<string, string>>(), arrayFields);
						}
					}
					break;
				case 72://ComputeShader
					goto default;
				case 114://MonoBehaviour
					{
						UnityAsset script = asset.GetAsset("m_Script");
						KeyValuePair<string, string>[] primitiveFields = new[]
						{
							new KeyValuePair<string, string>("AssemblyName", script.GetString("m_AssemblyName")),
							new KeyValuePair<string, string>("Namespace", script.GetString("m_Namespace")),
							new KeyValuePair<string, string>("ClassName", script.GetString("m_ClassName"))
						};
						assetInfo = new AssetInfo(asset.TypeID, asset.Name, primitiveFields);
					}
					break;
				case 115://MonoScript
					{
						KeyValuePair<string, string>[] primitiveFields = new[]
						{
							new KeyValuePair<string, string>("AssemblyName", asset.GetString("m_AssemblyName")),
							new KeyValuePair<string, string>("Namespace", asset.GetString("m_Namespace")),
							new KeyValuePair<string, string>("ClassName", asset.GetString("m_ClassName"))
						};
						assetInfo = new AssetInfo(asset.TypeID, asset.Name, primitiveFields);
					}
					break;
				case 128://Font
					goto default;
				case 213://Sprite
					goto default;
				case 1113://LightmapParameters
					goto default;
				default:
					assetInfo = new AssetInfo(asset.TypeID, asset.Name);
					break;
			}
			yield return new KeyValuePair<long, AssetInfo>(asset.PathID, assetInfo);
		}
	}

	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	private readonly struct UnityAsset
	{
		private readonly AssetsManager manager;
		private readonly AssetsFileInstance file;
		private readonly AssetFileInfo info;

		public UnityAsset(AssetsManager manager, AssetsFileInstance file, AssetFileInfo info)
		{
			this.manager = manager;
			this.file = file;
			this.info = info;
		}

		public int TypeID => info.TypeId;

		public long PathID => info.PathId;

		public string Name
		{
			get
			{
				AssetTypeValueField baseField = GetBaseField();
				string? name = baseField.Get("m_Name").AsString;
				if (string.IsNullOrEmpty(name) && TypeID == 48)//Shader
				{
					name = baseField.Get("m_ParsedForm").Get("m_Name").AsString;
				}
				return name ?? "";
			}
		}

		public string GetString(string fieldName) => GetBaseField().Get(fieldName).AsString ?? "";

		public UnityAsset GetAsset(string fieldName)
		{
			AssetExternal assetExternal = manager.GetExtAsset(file, GetBaseField().Get(fieldName));
			return new UnityAsset(manager, assetExternal.file, assetExternal.info);
		}

		public AssetTypeValueField GetBaseField() => manager.GetBaseField(file, info);

		private string GetDebuggerDisplay()
		{
			return $"{TypeID} : {Name}";
		}
	}
}

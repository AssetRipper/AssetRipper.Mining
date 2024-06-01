using AssetRipper.Assets;
using AssetRipper.Assets.Bundles;
using AssetRipper.Assets.Collections;
using AssetRipper.Assets.Generics;
using AssetRipper.Assets.IO;
using AssetRipper.Assets.Metadata;
using AssetRipper.Import.AssetCreation;
using AssetRipper.Import.Structure.Assembly.Serializable;
using AssetRipper.Import.Structure.Assembly.TypeTrees;
using AssetRipper.IO.Endian;
using AssetRipper.IO.Files.SerializedFiles.Parser;
using AssetRipper.Mining.PredefinedAssets;
using Object = AssetRipper.Mining.PredefinedAssets.Object;

namespace AssetRipper.Mining.EngineFileExtractor;

internal static class Program
{
	private const string DefaultResourcesName = "unity default resources";
	private const string ExtraResourcesName = "unity_builtin_extra";
	private const string ResourcesFolderPath = @"Editor/Data/Resources";

	public static void Main(string[] args)
	{
		string unityFolderPath = args[0];
		bool silent = args.Length > 1 && args[1] == "--silent";
		Dictionary<long, Object> defaultDictionary = ReadDictionary(Path.Combine(unityFolderPath, ResourcesFolderPath, DefaultResourcesName));
		Dictionary<long, Object> extraDictionary = ReadDictionary(Path.Combine(unityFolderPath, ResourcesFolderPath, ExtraResourcesName));

		File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "engineassets.json"), new EngineResourceData(defaultDictionary, extraDictionary).ToJson());

		if (!silent)
		{
			foreach ((string name, Dictionary<long, Object> dictionary) in new[] { ("Default", defaultDictionary), ("Extra", extraDictionary) })
			{
				Dictionary<int, int> typeIDs = dictionary.Values
					.Select(a => a.TypeID).Distinct().Order()
					.ToDictionary(id => id, id => dictionary.Values.Count(a => a.TypeID == id));
				Console.WriteLine(name);
				foreach ((int typeID, int count) in typeIDs)
				{
					Console.WriteLine($"\t{typeID,4} : {count,3}");
				}
			}
			Console.WriteLine("Done!");
		}
	}

	private static Dictionary<long, Object> ReadDictionary(string path)
	{
		return LoadAllAssetInfo(path).OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
	}

	private static AssetCollection LoadAllAssets(string path)
	{
		return GameBundle.FromPaths([path], TypeTreeAssetFactory.Instance).FetchAssetCollections().Single();
	}

	private sealed class TypeTreeAssetFactory : AssetFactoryBase
	{
		public static TypeTreeAssetFactory Instance { get; } = new();
		public override IUnityObjectBase? ReadAsset(AssetInfo assetInfo, ReadOnlyArraySegment<byte> assetData, SerializedType? assetType)
		{
			ArgumentNullException.ThrowIfNull(assetType);
			ArgumentOutOfRangeException.ThrowIfZero(assetType.OldType.Nodes.Count);
			if (!TypeTreeNodeStruct.TryMakeFromTypeTree(assetType.OldType, out TypeTreeNodeStruct root))
			{
				throw new NotSupportedException("Type tree node struct creation failed");
			}
			else
			{
				TypeTreeObject asset = TypeTreeObject.Create(assetInfo, root);
				EndianSpanReader reader = new(assetData, assetInfo.Collection.EndianType);
				asset.Read(ref reader);
				return asset;
			}
		}
	}

	private static IEnumerable<KeyValuePair<long, Object>> LoadAllAssetInfo(string path)
	{
		foreach (TypeTreeObject asset in LoadAllAssets(path).Cast<TypeTreeObject>())
		{
			Object obj;
			switch (asset.ClassID)
			{
				case 21://Material
					{
						obj = new Material()
						{
							Name = asset.Name(),
							Shader = asset.TryGetAsset("m_Shader")?.Name()
						};
					}
					break;
				case 28://Texture2D
					{
						obj = new Texture2D()
						{
							Name = asset.Name(),
							Height = asset.GetInt32("m_Height"),
							Width = asset.GetInt32("m_Width")
						};
					}
					break;
				case 43://Mesh
					{
						obj = new Mesh()
						{
							Name = asset.Name(),
							VertexCount = asset.ReleaseFields["m_VertexData"].AsStructure["m_VertexCount"].AsInt32,
							SubMeshCount = asset.ReleaseFields["m_SubMeshes"].AsAssetArray.Length,
							LocalAABB = asset.ReleaseFields["m_LocalAABB"].AsAxisAlignedBoundingBox(),
						};
					}
					break;
				case 48://Shader
					{
						string[] propertyNames;
						SerializableStructure? serializedShader = asset.ReleaseFields.TryGetField("m_ParsedForm")?.AsStructure;
						if (serializedShader is null)
						{
							propertyNames = Array.Empty<string>();
						}
						else
						{
							IUnityAssetBase[] propsList = serializedShader["m_PropInfo"].AsStructure["m_Props"].AsAssetArray;
							if (propsList.Length == 0)
							{
								propertyNames = Array.Empty<string>();
							}
							else
							{
								propertyNames = new string[propsList.Length];
								for (int i = 0; i < propsList.Length; i++)
								{
									propertyNames[i] = ((SerializableStructure)propsList[i])["m_Name"].AsString;
								}
							}
						}

						obj = new Shader()
						{
							Name = asset.Name(),
							PropertyNames = propertyNames
						};
					}
					break;
				case 72://ComputeShader
					goto default;
				case 89://Cubemap
					{
						obj = new Cubemap()
						{
							Name = asset.Name(),
							Height = asset.GetInt32("m_Height"),
							Width = asset.GetInt32("m_Width")
						};
					}
					break;
				case 114://MonoBehaviour
					{
						TypeTreeObject? script = asset.TryGetAsset("m_Script");
						obj = new MonoBehaviour()
						{
							Name = asset.Name(),
							AssemblyName = script?.TryGetString("m_AssemblyName") ?? "",
							Namespace = script?.GetString("m_Namespace") ?? "",
							ClassName = script?.GetString("m_ClassName") ?? "",
							GameObject = asset.TryGetAsset("m_GameObject")?.Name(),
							Enabled = asset.GetBoolean("m_Enabled")
						};
					}
					break;
				case 115://MonoScript
					{
						obj = new MonoScript()
						{
							AssemblyName = asset.TryGetString("m_AssemblyName") ?? "",
							Namespace = asset.GetString("m_Namespace"),
							ClassName = asset.GetString("m_ClassName")
						};
					}
					break;
				case 128://Font
					goto default;
				case 213://Sprite
					{
						obj = new Sprite()
						{
							Name = asset.Name(),
							Texture = asset.ResolveAsset(asset.ReleaseFields["m_RD"].AsStructure["texture"].AsPPtr)?.Name(),
						};
					}
					break;
				case 1113://LightmapParameters
					goto default;
				default:
					obj = new GenericNamedObject()
					{
						TypeID = asset.ClassID,
						Name = asset.Name()
					};
					break;
			}
			yield return new KeyValuePair<long, Object>(asset.PathID, obj);
		}
	}
}

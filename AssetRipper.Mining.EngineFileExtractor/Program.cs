﻿using AssetRipper.Mining.PredefinedAssets;
using AssetRipper.Primitives;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Diagnostics;
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
		Dictionary<long, Object> defaultDictionary = ReadDictionary(Path.Combine(unityFolderPath, ResourcesFolderPath, DefaultResourcesName));
		Dictionary<long, Object> extraDictionary = ReadDictionary(Path.Combine(unityFolderPath, ResourcesFolderPath, ExtraResourcesName));

		foreach ((string name, Dictionary<long, Object> dictionary) in new[]{ ("Default", defaultDictionary), ("Extra", extraDictionary) })
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

		File.WriteAllText("engineassets.json", new EngineResourceData(defaultDictionary, extraDictionary).ToJson());
		Console.WriteLine("Done!");
	}

	private static Dictionary<long, Object> ReadDictionary(string path)
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

	private static IEnumerable<KeyValuePair<long, Object>> LoadAllAssetInfo(string path)
	{
		foreach (UnityAsset asset in LoadAllAssets(path))
		{
			Object obj;
			switch (asset.TypeID)
			{
				case 21://Material
					{
						obj = new Material()
						{
							Name = asset.Name,
							Shader = asset.TryGetAsset("m_Shader")?.Name
						};
					}
					break;
				case 28://Texture2D
					{
						obj = new Texture2D()
						{
							Name = asset.Name,
							Height = asset.GetInt32("m_Height"),
							Width = asset.GetInt32("m_Width")
						};
					}
					break;
				case 43://Mesh
					{
						obj = new Mesh()
						{
							Name = asset.Name,
							VertexCount = asset.GetBaseField().Get("m_VertexData").Get("m_VertexCount").AsUInt
						};
					}
					break;
				case 48://Shader
					{
						string[] propertyNames;
						AssetTypeValueField serializedShader = asset.GetBaseField().Get("m_ParsedForm");
						if (serializedShader.IsDummy)
						{
							propertyNames = Array.Empty<string>();
						}
						else
						{
							List<AssetTypeValueField> propsList = serializedShader.Get("m_PropInfo").Get("m_Props").Get("Array").Children;
							if (propsList.Count == 0)
							{
								propertyNames = Array.Empty<string>();
							}
							else
							{
								propertyNames = new string[propsList.Count];
								for (int i = 0; i < propsList.Count; i++)
								{
									propertyNames[i] = propsList[i].Get("m_Name").AsString ?? "";
								}
							}
						}

						obj = new Shader()
						{
							Name = asset.Name,
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
							Name = asset.Name,
							Height = asset.GetInt32("m_Height"),
							Width = asset.GetInt32("m_Width")
						};
					}
					break;
				case 114://MonoBehaviour
					{
						UnityAsset? script = asset.TryGetAsset("m_Script");
						obj = new MonoBehaviour()
						{
							Name = asset.Name,
							AssemblyName = script?.GetString("m_AssemblyName") ?? "",
							Namespace = script?.GetString("m_Namespace") ?? "",
							ClassName = script?.GetString("m_ClassName") ?? "",
							GameObject = asset.TryGetAsset("m_GameObject")?.Name,
							Enabled = asset.GetBoolean("m_Enabled")
						};
					}
					break;
				case 115://MonoScript
					{
						obj = new MonoScript()
						{
							AssemblyName = asset.GetString("m_AssemblyName"),
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
							Name = asset.Name,
							Texture = asset.ResolveAsset(asset.GetBaseField().Get("m_RD").Get("texture"))?.Name,
						};
					}
					break;
				case 1113://LightmapParameters
					goto default;
				default:
					obj = new GenericNamedObject()
					{
						TypeID = asset.TypeID,
						Name = asset.Name
					};
					break;
			}
			yield return new KeyValuePair<long, Object>(asset.PathID, obj);
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
				AssetTypeValueField nameField = baseField.Get("m_Name");
				string? name = nameField.IsDummy ? null : nameField.AsString;
				if (string.IsNullOrEmpty(name) && TypeID == 48)//Shader
				{
					name = baseField.Get("m_ParsedForm").Get("m_Name").AsString;
				}
				return name ?? "";
			}
		}

		public string GetString(string fieldName) => GetBaseField().Get(fieldName).AsString ?? "";

		public int GetInt32(string fieldName) => GetBaseField().Get(fieldName).AsInt;

		public bool GetBoolean(string fieldName) => GetBaseField().Get(fieldName).AsBool;

		public UnityAsset? TryGetAsset(string fieldName)
		{
			return ResolveAsset(GetBaseField().Get(fieldName));
		}

		public UnityAsset? ResolveAsset(AssetTypeValueField pptrField)
		{
			AssetExternal assetExternal = manager.GetExtAsset(file, pptrField);
			return assetExternal.file is null || assetExternal.info is null
				? null
				: new UnityAsset(manager, assetExternal.file, assetExternal.info);
		}

		public AssetTypeValueField GetBaseField() => manager.GetBaseField(file, info);

		private string GetDebuggerDisplay()
		{
			return $"{TypeID} : {Name}";
		}
	}
}

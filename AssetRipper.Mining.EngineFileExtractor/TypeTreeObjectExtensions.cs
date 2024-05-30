using AssetRipper.Assets.Metadata;
using AssetRipper.Import.AssetCreation;
using AssetRipper.Import.Structure.Assembly.Serializable;

namespace AssetRipper.Mining.EngineFileExtractor;

internal static class TypeTreeObjectExtensions
{
	public static string Name(this TypeTreeObject asset)
	{
		SerializableStructure baseField = asset.ReleaseFields;
		string? name = baseField.TryGetField("m_Name")?.AsString;
		if (string.IsNullOrEmpty(name) && asset.ClassID == 48)//Shader
		{
			name = baseField.TryGetField("m_ParsedForm")?.AsStructure.TryGetField("m_Name")?.AsString //5.5 and later
				?? baseField.TryGetField("m_PathName")?.AsString; //Earlier than 5.5

			if (string.IsNullOrEmpty(name)
				&& (baseField.TryGetField("m_Script")?.AsString?.StartsWith("Shader \"Standard\"", StringComparison.Ordinal) ?? false))
			{
				//A regex could be used to generalize, but as far as I know, Standard is the only one like this.
				name = "Standard";
			}
		}
		return name ?? "";
	}

	public static TypeTreeObject? TryGetAsset(this TypeTreeObject asset, string fieldName)
	{
		return asset.ReleaseFields.TryGetField(fieldName)?.AsAsset is IPPtr pptr
			? asset.ResolveAsset(pptr)
			: null;
	}

	public static bool GetBoolean(this TypeTreeObject asset, string fieldName)
	{
		return asset.ReleaseFields[fieldName].AsBoolean;
	}

	public static int GetInt32(this TypeTreeObject asset, string fieldName)
	{
		return asset.ReleaseFields[fieldName].AsInt32;
	}

	public static string GetString(this TypeTreeObject asset, string fieldName)
	{
		return asset.ReleaseFields[fieldName].AsString;
	}

	public static string? TryGetString(this TypeTreeObject asset, string fieldName)
	{
		return asset.ReleaseFields.TryGetField(fieldName)?.AsString;
	}

	public static TypeTreeObject? ResolveAsset(this TypeTreeObject asset, IPPtr pptr)
	{
		return asset.Collection.TryGetAsset(new PPtr<TypeTreeObject>(pptr.FileID, pptr.PathID));
	}
}

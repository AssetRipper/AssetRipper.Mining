using System.Diagnostics;

namespace AssetRipper.Mining.EngineAssets;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public record struct AssetInfo
{
	public int TypeID { get; set; }
	public string Name { get; set; }
	public KeyValuePair<string, string>[] PrimitiveFields { get; set; }
	public KeyValuePair<string, string[]>[] ArrayFields { get; set; }

	public AssetInfo(int typeID, string name) : this(typeID, name, Array.Empty<KeyValuePair<string, string>>(), Array.Empty<KeyValuePair<string, string[]>>())
	{
	}

	public AssetInfo(int typeID, string name, KeyValuePair<string, string>[] primitiveFields) : this(typeID, name, primitiveFields, Array.Empty<KeyValuePair<string, string[]>>())
	{
	}

	public AssetInfo(int typeID, string name, KeyValuePair<string, string>[] primitiveFields, KeyValuePair<string, string[]>[] arrayFields)
	{
		Name = name;
		TypeID = typeID;
		PrimitiveFields = primitiveFields;
		ArrayFields = arrayFields;
	}

	private string GetDebuggerDisplay()
	{
		return $"{TypeID} : {Name}";
	}
}

using System.Diagnostics;
using AdditionalFieldArray = System.Collections.Immutable.ImmutableArray<System.Collections.Generic.KeyValuePair<string, string>>;

namespace AssetRipper.Mining.EngineAssets;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly struct AssetInfo : IEquatable<AssetInfo>
{
	public int TypeID { get; }
	public string Name { get; }
	public AdditionalFieldArray AdditionalFields { get; }

	public AssetInfo(int typeID, string name) : this(typeID, name, AdditionalFieldArray.Empty)
	{
	}

	public AssetInfo(int typeID, string name, AdditionalFieldArray additionalFields)
	{
		Name = name;
		TypeID = typeID;
		AdditionalFields = additionalFields;
	}

	public override bool Equals(object? obj)
	{
		return obj is AssetInfo info && Equals(info);
	}

	public bool Equals(AssetInfo other)
	{
		return TypeID == other.TypeID &&
			   Name == other.Name &&
			   AdditionalFields.AsSpan().SequenceEqual(other.AdditionalFields.AsSpan());
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(TypeID, Name, AdditionalFields.Length);
	}

	public static bool operator ==(AssetInfo left, AssetInfo right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(AssetInfo left, AssetInfo right)
	{
		return !(left == right);
	}

	private string GetDebuggerDisplay()
	{
		return $"{TypeID} : {Name}";
	}
}

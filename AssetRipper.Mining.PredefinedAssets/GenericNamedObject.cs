using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class GenericNamedObject : NamedObject
{
	[JsonInclude]
	public new required int TypeID { get; init; }

	public GenericNamedObject()
	{
	}

	[SetsRequiredMembers]
	public GenericNamedObject(string name, int typeID) : base(name)
	{
		TypeID = typeID;
	}

	public static GenericNamedObject CreateComputeShader(string name)
	{
		return new(name, 72);
	}

	public static GenericNamedObject CreateFont(string name)
	{
		return new(name, 128);
	}
}

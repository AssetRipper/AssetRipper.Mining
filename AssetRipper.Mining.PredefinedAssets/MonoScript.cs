using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class MonoScript : NamedObject
{
	public string AssemblyName { get; set; } = "";
	public string Namespace { get; set; } = "";
	public string ClassName { get; set; } = "";

	[JsonIgnore]
	public override int TypeID
	{
		get => 115;
		set { }
	}
}

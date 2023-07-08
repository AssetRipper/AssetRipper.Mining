using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

public sealed record class Shader : NamedObject
{
	/// <summary>
	/// 5.5.0 to Max
	/// </summary>
	public string[] PropertyNames { get; set; } = Array.Empty<string>();

	public bool Equals([NotNullWhen(true)] Shader? other)
	{
		return Equals((NamedObject?)other) && PropertyNames.AsSpan().SequenceEqual(other.PropertyNames);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), PropertyNames.Length);
	}

	[JsonIgnore]
	public override int TypeID
	{
		get => 48;
		set { }
	}
}
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

public sealed record class GameObject : Object
{
	public string Name { get; set; } = "";
	/// <summary>
	/// List of the <see cref="Object.TypeID"/>s for the <see cref="Component"/>s attached to this <see cref="GameObject"/>.
	/// </summary>
	public int[] Components { get; set; } = Array.Empty<int>();
	public uint Layer { get; set; }

	[JsonIgnore]
	public override int TypeID
	{
		get => 1;
		set { }
	}

	public bool Equals([NotNullWhen(true)] GameObject? other)
	{
		return Equals((Object?)other) && Name == other.Name && Components.AsSpan().SequenceEqual(other.Components);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Name, Components.Length);
	}
}
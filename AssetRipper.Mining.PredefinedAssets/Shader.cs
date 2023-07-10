using System.Diagnostics.CodeAnalysis;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class Shader : NamedObject
{
	/// <summary>
	/// 5.5.0 to Max
	/// </summary>
	public required string[] PropertyNames { get; init; }

	public bool Equals([NotNullWhen(true)] Shader? other)
	{
		return other is not null && Name == other.Name && PropertyNames.AsSpan().SequenceEqual(other.PropertyNames);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Name, PropertyNames.Length);
	}
}
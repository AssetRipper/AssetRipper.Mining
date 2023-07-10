using System.Diagnostics.CodeAnalysis;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class GameObject : Object
{
	public required string Name { get; init; }
	/// <summary>
	/// List of the <see cref="Object.TypeID"/>s for the <see cref="Component"/>s attached to this <see cref="GameObject"/>.
	/// </summary>
	public required int[] Components { get; init; }
	public required uint Layer { get; init; }

	public bool Equals([NotNullWhen(true)] GameObject? other)
	{
		return other is not null && Name == other.Name && Components.AsSpan().SequenceEqual(other.Components);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Name, Components.Length);
	}
}
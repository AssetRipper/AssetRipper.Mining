namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class MonoScript : Object
{
	public required string AssemblyName { get; init; }
	public required string Namespace { get; init; }
	public required string ClassName { get; init; }
}

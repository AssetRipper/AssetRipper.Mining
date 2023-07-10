namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class MonoBehaviour : Behaviour
{
	public required string Name { get; init; }
	public required string AssemblyName { get; init; }
	public required string Namespace { get; init; }
	public required string ClassName { get; init; }
}

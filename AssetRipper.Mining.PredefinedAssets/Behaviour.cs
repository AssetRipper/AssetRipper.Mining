namespace AssetRipper.Mining.PredefinedAssets;

public abstract record class Behaviour : Component
{
	public required bool Enabled { get; init; }
}
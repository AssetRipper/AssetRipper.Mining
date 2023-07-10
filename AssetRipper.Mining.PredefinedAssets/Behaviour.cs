namespace AssetRipper.Mining.PredefinedAssets;

public abstract record class Behaviour : Component
{
	public bool Enabled { get; set; }
}
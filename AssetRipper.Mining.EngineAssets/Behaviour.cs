namespace AssetRipper.Mining.EngineAssets;

public abstract record class Behaviour : Component
{
	public bool Enabled { get; set; }
}
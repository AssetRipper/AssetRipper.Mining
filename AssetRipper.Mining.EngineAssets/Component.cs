namespace AssetRipper.Mining.EngineAssets;

public abstract record class Component : Object
{
	/// <summary>
	/// The name of the <see cref="GameObject"/> this component is attached to.
	/// </summary>
	public string? GameObject { get; set; }
}

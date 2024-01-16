namespace AssetRipper.Mining.PredefinedAssets;

public enum AssetType
{
	/// <summary>
	/// Used by a released game
	/// </summary>
	Internal = 0,
	/// <summary>
	/// Library asset file. It is editor created file, it doesn't exist in Assets directory
	/// It has the format "library/cache/[first Hash byte as hex]/[Hash as hex]"
	/// </summary>
	Cached = 1,
	/// <summary>
	/// Serialized asset file. It contains all parameters inside itself.
	/// </summary>
	/// <remarks>
	/// All yaml files use this type.
	/// </remarks>
	Serialized = 2,
	/// <summary>
	/// Binary asset file. It contains all parameters inside meta file.
	/// </summary>
	/// <remarks>
	/// Any "universal" format uses this type. For example, png, jpg, tga, wav, ogg, fbx, cs, dll, shader, etc.
	/// </remarks>
	Meta = 3,
}
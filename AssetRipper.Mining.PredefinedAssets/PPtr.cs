using AssetRipper.Primitives;

namespace AssetRipper.Mining.PredefinedAssets;

/// <summary>
/// A data structure representing a reference to an asset.
/// </summary>
/// <param name="FileID">The local identifier within the file.</param>
/// <param name="Guid">The global identifier for the file.</param>
/// <param name="Meta">The type of file. </param>
public readonly record struct PPtr(long FileID, UnityGuid Guid, AssetType Meta)
{
}

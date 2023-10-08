using AssetsTools.NET;

namespace AssetRipper.Mining.EngineFileExtractor;

internal static class AssetTypeValueFieldExtensions
{
	public static AssetTypeValueField? TryGet(this AssetTypeValueField @this, string name)
	{
		AssetTypeValueField result = @this.Get(name);
		return result.IsDummy ? null : result;
	}
}
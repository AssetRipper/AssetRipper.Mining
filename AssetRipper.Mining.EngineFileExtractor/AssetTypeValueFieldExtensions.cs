using AssetRipper.Mining.PredefinedAssets;
using AssetsTools.NET;
using System.Numerics;

namespace AssetRipper.Mining.EngineFileExtractor;

internal static class AssetTypeValueFieldExtensions
{
	public static AssetTypeValueField? TryGet(this AssetTypeValueField @this, string name)
	{
		AssetTypeValueField result = @this.Get(name);
		return result.IsDummy ? null : result;
	}

	public static Vector3 AsVector3(this AssetTypeValueField @this)
	{
		return new Vector3(@this["x"].AsFloat, @this["y"].AsFloat, @this["z"].AsFloat);
	}

	public static AxisAlignedBoundingBox AsAxisAlignedBoundingBox(this AssetTypeValueField @this)
	{
		return new AxisAlignedBoundingBox(@this["m_Center"].AsVector3(), @this["m_Extent"].AsVector3());
	}
}

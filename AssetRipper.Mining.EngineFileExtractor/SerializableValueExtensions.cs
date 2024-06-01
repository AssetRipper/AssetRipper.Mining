using AssetRipper.Import.Structure.Assembly.Serializable;
using AssetRipper.Mining.PredefinedAssets;
using System.Numerics;

namespace AssetRipper.Mining.EngineFileExtractor;

internal static class SerializableValueExtensions
{
	public static Vector3 AsVector3(this SerializableValue value)
	{
		return new Vector3(
			value.AsStructure["x"].AsSingle,
			value.AsStructure["y"].AsSingle,
			value.AsStructure["z"].AsSingle);
	}

	public static AxisAlignedBoundingBox AsAxisAlignedBoundingBox(this SerializableValue value)
	{
		return new AxisAlignedBoundingBox(
			value.AsStructure["m_Center"].AsVector3(),
			value.AsStructure["m_Extent"].AsVector3());
	}
}

using AssetRipper.Primitives;
using System.Text.Json;

namespace AssetRipper.Mining.PredefinedAssets;

/// <summary>
/// A data structure representing a Unity package.
/// </summary>
/// <remarks>
/// File extensions are not included in any names.
/// </remarks>
/// <param name="Name">The name of this Unity package.</param>
/// <param name="Version">The version of this Unity package. Only applicable if <paramref name="UsedInPackageJson"/> is true.</param>
/// <param name="UsedInPackageJson">If true, this Unity package should be referenced in the project's package json file.</param>
/// <param name="Assemblies">A dictionary of compiled dll assemblies in the package.</param>
/// <param name="Scripts">A dictionary of loose cs scripts in the package. Assembly name : { Full name : Guid }</param>
/// <param name="Assets">A dictionary of assets in the package.</param>
public readonly record struct UnityPackageData(
	string Name,
	Version Version,
	bool UsedInPackageJson,
	Dictionary<string, UnityGuid> Assemblies,
	Dictionary<string, Dictionary<string, UnityGuid>> Scripts,
	KeyValuePair<Object, UnityGuid>[] Assets)
{
	public readonly string ToJson()
	{
		return JsonSerializer.Serialize(this, InternalSerializerContext.Default.UnityPackageData);
	}

	public static UnityPackageData FromJson(string text)
	{
		return JsonSerializer.Deserialize(text, InternalSerializerContext.Default.UnityPackageData);
	}
}
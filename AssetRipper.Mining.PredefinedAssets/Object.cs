using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(AudioClip), nameof(AudioClip))]
[JsonDerivedType(typeof(GenericBehaviour), nameof(Behaviour))]
[JsonDerivedType(typeof(GenericComponent), nameof(Component))]
[JsonDerivedType(typeof(Cubemap), nameof(Cubemap))]
[JsonDerivedType(typeof(GameObject), nameof(GameObject))]
[JsonDerivedType(typeof(GenericNamedObject), nameof(NamedObject))]
[JsonDerivedType(typeof(Material), nameof(Material))]
[JsonDerivedType(typeof(Mesh), nameof(Mesh))]
[JsonDerivedType(typeof(MonoBehaviour), nameof(MonoBehaviour))]
[JsonDerivedType(typeof(MonoScript), nameof(MonoScript))]
[JsonDerivedType(typeof(Shader), nameof(Shader))]
[JsonDerivedType(typeof(Sprite), nameof(Sprite))]
[JsonDerivedType(typeof(TextAsset), nameof(TextAsset))]
[JsonDerivedType(typeof(Texture2D), nameof(Texture2D))]
public abstract record class Object
{
	[JsonIgnore]
	public abstract int TypeID { get; set; }
}

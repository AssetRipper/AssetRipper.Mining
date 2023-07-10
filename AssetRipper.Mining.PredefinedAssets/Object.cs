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
	public int TypeID
	{
		get
		{
			return this switch
			{
				AudioClip => 83,
				Cubemap => 89,
				GameObject => 1,
				Material => 21,
				Mesh => 43,
				MonoBehaviour => 114,
				MonoScript => 115,
				Shader => 48,
				Sprite => 49,
				TextAsset => 49,
				Texture2D => 28,
				_ => GetGenericTypeID(),
			};
		}
	}

	private int GetGenericTypeID()
	{
		return this switch
		{
			GenericBehaviour behaviour => behaviour.TypeID,
			GenericComponent component => component.TypeID,
			GenericNamedObject namedObject => namedObject.TypeID,
			Behaviour => 8,
			Component => 2,
			NamedObject => 130,
			_ => 0,
		};
	}
}

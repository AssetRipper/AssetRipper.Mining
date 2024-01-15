#if !NET7_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed class UInt128JsonConverter : JsonConverter<UInt128>
{
	public override UInt128 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return UInt128.Parse(reader.GetString() ?? throw new JsonException("String was read as null"));
	}

	public override void Write(Utf8JsonWriter writer, UInt128 value, JsonSerializerOptions options)
	{
		writer.WriteRawValue(value.ToString());
	}
}
#endif
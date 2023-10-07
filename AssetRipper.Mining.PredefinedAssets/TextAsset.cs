using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class TextAsset : NamedObject
{
	/// <summary>
	/// The length the text in bytes.
	/// </summary>
	public required int Length { get; init; }

	/// <summary>
	/// An MD5 hash of the text contents.
	/// </summary>
	public required UInt128 Hash { get; init; }

	public TextAsset()
	{
	}

	[SetsRequiredMembers]
	public TextAsset(string name, ReadOnlySpan<byte> data)
	{
		Name = name;
		Length = data.Length;
		Hash = HashData(data);
	}

	private static UInt128 HashData(ReadOnlySpan<byte> data)
	{
		Span<byte> buffer = stackalloc byte[16];
		MD5.HashData(data, buffer);
		return new UInt128(
			BinaryPrimitives.ReadUInt64LittleEndian(buffer[8..]),
			BinaryPrimitives.ReadUInt64LittleEndian(buffer));
		//On .NET 8, replace with ReadUInt128LittleEndian
	}
}
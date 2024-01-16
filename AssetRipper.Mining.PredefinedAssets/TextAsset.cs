using System.Buffers.Binary;
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
		HashData(data, buffer);
#if NET8_0_OR_GREATER
		return BinaryPrimitives.ReadUInt128LittleEndian(buffer);
#else
		return new UInt128(
			BinaryPrimitives.ReadUInt64LittleEndian(buffer[8..]),
			BinaryPrimitives.ReadUInt64LittleEndian(buffer));
#endif
	}

	private static void HashData(ReadOnlySpan<byte> data, Span<byte> buffer)
	{
#if NET5_0_OR_GREATER
		MD5.HashData(data, buffer);
#else
		MD5.Create().ComputeHash(data.ToArray()).AsSpan().CopyTo(buffer);
#endif
	}
}

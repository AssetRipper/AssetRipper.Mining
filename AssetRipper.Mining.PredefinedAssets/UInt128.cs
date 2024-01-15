#if NET7_0_OR_GREATER
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(UInt128))]
#else
using AssetRipper.Mining.PredefinedAssets;
using System.Numerics;
using System.Text.Json.Serialization;

namespace System;

/// <summary>
/// A limited polyfill for the UInt128 struct.
/// </summary>
[JsonConverter(typeof(UInt128JsonConverter))]
public readonly record struct UInt128
{
	private readonly ulong _lower;
	private readonly ulong _upper;

	/// <summary>Initializes a new instance of the <see cref="UInt128" /> struct.</summary>
	/// <param name="upper">The upper 64-bits of the 128-bit value.</param>
	/// <param name="lower">The lower 64-bits of the 128-bit value.</param>
	public UInt128(ulong upper, ulong lower)
	{
		_lower = lower;
		_upper = upper;
	}

	public override string ToString()
	{
		return _upper == 0 ? _lower.ToString() : ToBigInteger().ToString();
	}

	public static UInt128 Parse(string value)
	{
		return TryParse(value, out UInt128 result) ? result : throw new FormatException("Input string was not in a correct format.");
	}

	public static bool TryParse(string value, out UInt128 result)
	{
		if (BigInteger.TryParse(value, out BigInteger bigInteger))
		{
			result = FromBigInteger(bigInteger);
			return true;
		}
		else
		{
			result = default;
			return false;
		}
	}

	private BigInteger ToBigInteger()
	{
		return (new BigInteger(_upper) << 64) | new BigInteger(_lower);
	}

	private static UInt128 FromBigInteger(BigInteger value)
	{
		ulong upper = (ulong)(value >> 64);
		ulong lower = (ulong)value;
		return new UInt128(upper, lower);
	}
}
#endif
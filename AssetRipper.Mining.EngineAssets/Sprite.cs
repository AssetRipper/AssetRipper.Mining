﻿using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

public sealed record class Sprite : NamedObject
{
	public string? Texture { get; set; }

	[JsonIgnore]
	public override int TypeID
	{
		get => 213;
		set { }
	}
}
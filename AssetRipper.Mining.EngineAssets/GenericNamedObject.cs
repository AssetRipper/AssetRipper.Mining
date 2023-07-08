﻿using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

public sealed record class GenericNamedObject : NamedObject
{
	[JsonInclude]
	public override int TypeID { get; set; }
}
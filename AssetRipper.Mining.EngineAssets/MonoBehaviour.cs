﻿using System.Text.Json.Serialization;

namespace AssetRipper.Mining.EngineAssets;

public sealed record class MonoBehaviour : Behaviour
{
	public string Name { get; set; } = "";
	public string AssemblyName { get; set; } = "";
	public string Namespace { get; set; } = "";
	public string ClassName { get; set; } = "";

	[JsonIgnore]
	public override int TypeID
	{
		get => 114;
		set { }
	}
}

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssetRipper.Mining.Unity.Documentation.Web;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class TocNode
{
	private const string TOC = "toc";
	private const string Classes = "Classes";
	private const string Interfaces = "Interfaces";
	private const string Enumerations = "Enumerations";
	private const string Attributes = "Attributes";
	private const string Assemblies = "Assemblies";
	private string? link;

	[JsonPropertyName("link")]
	public required string? Link
	{
		get => link;
		init
		{
			link = value is "null" ? null : value;
		}
	}

	[JsonPropertyName("title")]
	public required string Title { get; init; }

	[JsonPropertyName("children")]
	public required TocNode[]? Children { get; init; }

	[JsonIgnore]
	[MemberNotNullWhen(true, nameof(Link))]
	public bool IsRoot => Link is TOC && Title is TOC;

	[JsonIgnore]
	[MemberNotNullWhen(true, nameof(Link))]
	public bool IsType => Link is not null and not TOC;

	[JsonIgnore]
	[MemberNotNullWhen(false, nameof(Link))]
	public bool IsTypeCollection => Link is null && Title is Classes or Interfaces or Enumerations or Attributes;

	[JsonIgnore]
	[MemberNotNullWhen(false, nameof(Link))]
	public bool IsAssemblyCollection => Link is null && Title is Assemblies;

	[JsonIgnore]
	[MemberNotNullWhen(false, nameof(Link))]
	public bool IsNamespace => Link is null && Title is not Classes and not Interfaces and not Enumerations and not Attributes and not Assemblies;

	[JsonIgnore]
	[MemberNotNullWhen(true, nameof(Children))]
	public bool HasChildren => Children is not null && Children.Length > 0;

	public static TocNode? FromJsonText(string json)
	{
		return JsonSerializer.Deserialize(json, TocSerializerContext.Default.TocNode);
	}

	public static TocNode? FromJsonFile(string path)
	{
		return JsonSerializer.Deserialize(File.OpenRead(path), TocSerializerContext.Default.TocNode);
	}

	private string GetDebuggerDisplay()
	{
		return Title;
	}
}

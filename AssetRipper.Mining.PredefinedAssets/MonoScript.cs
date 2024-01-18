namespace AssetRipper.Mining.PredefinedAssets;

public sealed record class MonoScript : Object
{
	public required string AssemblyName { get; init; }
	public required string Namespace { get; init; }
	public required string ClassName { get; init; }

	public MonoScript()
	{
	}

	[SetsRequiredMembers]
	public MonoScript(string assemblyName, string @namespace, string className)
	{
		AssemblyName = assemblyName;
		Namespace = @namespace;
		ClassName = className;
	}

	public static MonoScript FromType(Type type)
	{
		return new MonoScript(type.Assembly.GetName().Name, type.Namespace, type.Name);
	}
}

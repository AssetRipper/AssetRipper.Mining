namespace AssetRipper.Mining.PredefinedAssets.Tests;

public class Tests
{
	[Test]
	public void TextAssetSerializesConsistently()
	{
		//This test ensures that the TextAsset hash is serialized consistently on all the test target frameworks.
		TextAsset asset = new TextAsset("Example", "Hello, World!"u8);
		string json = asset.ToJson().Replace(Environment.NewLine, "\n");
		Assert.That(json, Is.EqualTo("""
			{
			  "$type": "TextAsset",
			  "Length": 13,
			  "Hash": 281850845012336186393611809232225740901,
			  "Name": "Example"
			}
			""".Replace(Environment.NewLine, "\n")));
	}

	[Test]
	public void UnityPackageDataCanBeSerialized()
	{
		UnityPackageData package = new UnityPackageData("Example", "1.0.0", true);
		package.Assets.Add(new TextAsset("", ""u8), default);
		string json = package.ToJson();
		Assert.That(json, Is.Not.Empty);
	}

	[Test]
	public void UnityPackageDataCanBeDeserialized()
	{
		UnityPackageData package = UnityPackageData.FromJson(MakeJson());
		Assert.That(package.Assets, Is.Not.Empty);

		static string MakeJson()
		{
			UnityPackageData package = new UnityPackageData("Example", "1.0.0", true);
			package.Assets.Add(new TextAsset("", ""u8), default);
			return package.ToJson();
		}
	}
}

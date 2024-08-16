using System.Numerics;
using System.Text.Json;

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

	[Test]
	public void Vector3Serialization()
	{
		string result = """
		{
		  "X": 1,
		  "Y": 2,
		  "Z": 3
		}
		""".Replace("\r", null);
		Vector3 vector = new Vector3(1.0f, 2.0f, 3.0f);
		Assert.That(JsonSerializer.Serialize(vector, MiningSerializerContext.Default.Vector3).Replace("\r", null), Is.EqualTo(result));
	}

	[Test]
	public void Vector3Deserialization()
	{
		string json = """{ "X": 1.0, "Y": 2.0, "Z": 3.0 }""";
		Vector3 result = new Vector3(1.0f, 2.0f, 3.0f);
		Assert.That(JsonSerializer.Deserialize(json, MiningSerializerContext.Default.Vector3), Is.EqualTo(result));
	}

	[TestCase("")]
	[TestCase("{}")]
	public void EmptyEngineResourceDataDeserialization(string json)
	{
		EngineResourceData result = EngineResourceData.FromJson(json);
		Assert.Multiple(() =>
		{
			Assert.That(result.DefaultResources, Is.Not.Null);
			Assert.That(result.ExtraResources, Is.Not.Null);
		});
		Assert.Multiple(() =>
		{
			Assert.That(result.DefaultResources, Is.Empty);
			Assert.That(result.ExtraResources, Is.Empty);
		});
	}
}

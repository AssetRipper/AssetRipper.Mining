using AssetRipper.Mining.Unity.Documentation.Web;

TocNode root = TocNode.FromJsonFile(@"toc.json") ?? throw new NullReferenceException();
Dictionary<string, string> dictionary = TocAnalyzer.Analyze(root);
Dictionary<string, string> reverseDictionary = dictionary.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Value, pair => pair.Key);
_ = reverseDictionary;
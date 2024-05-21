using System.Diagnostics;

namespace AssetRipper.Mining.Unity.Documentation.Web;
public static class TocAnalyzer
{
	public static Dictionary<string, string> Analyze(TocNode root)
	{
		Debug.Assert(root.IsRoot);
		Debug.Assert(root.HasChildren);

		//Link : Full name
		Dictionary<string, string> result = new();
		Stack<(TocNode, int)> stack = new();
		foreach (TocNode rootChild in root.Children)
		{
			if (rootChild.Title is "Other")
			{
				continue;
			}
			stack.Push((rootChild, -1));
			while (stack.Count > 0)
			{
				(TocNode node, int childIndex) = stack.Pop();
				if (childIndex == -1)
				{
					if (node.IsType)
					{
						string @namespace = GetNamespace(stack, result);
						string fullName = $"{@namespace}.{node.Title}";
						result.Add(node.Link, fullName);
					}
					if (node.HasChildren && !node.IsAssemblyCollection)
					{
						stack.Push((node, 0));
					}
				}
				else if (childIndex < node.Children!.Length)
				{
					stack.Push((node, childIndex + 1));
					stack.Push((node.Children[childIndex], -1));
				}
			}
		}
		return result;
	}

	private static string GetNamespace(Stack<(TocNode, int)> stack, Dictionary<string, string> dictionary)
	{
		foreach ((TocNode node, _) in stack)
		{
			if (node.IsNamespace)
			{
				return node.Title;
			}
			if (node.IsType)
			{
				return dictionary[node.Link];
			}
		}
		if (stack.Count == 0)
		{
			throw new InvalidOperationException("The stack is empty.");
		}
		else
		{
			throw new InvalidOperationException("Could not determine the namespace from the stack.");
		}
	}
}

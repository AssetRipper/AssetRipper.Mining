using AssetRipper.Primitives;
using System.Collections;

namespace AssetRipper.Mining.PredefinedAssets;

public readonly struct AssetDictionary : IReadOnlyCollection<KeyValuePair<Object, PPtr>>, ICollection<KeyValuePair<Object, PPtr>>
{
	//This class implements all the IReadOnlyDictionary`2 members, but does not implement the interface because that changes how System.Text.Json serializes it.

	private readonly Dictionary<Object, PPtr> _dictionary = new();

	public int Count => _dictionary.Count;

	public IEnumerable<Object> Keys => _dictionary.Keys;

	public IEnumerable<PPtr> Values => _dictionary.Values;

	public PPtr this[Object key] => _dictionary[key];

	public AssetDictionary()
	{
	}

	public void Add(Object key, PPtr value) => _dictionary.Add(key, value);

	public void Add(MonoScript key, UnityGuid guid) => Add(key, new PPtr(11500000, guid, AssetType.Meta));

	public void Clear() => _dictionary.Clear();

	public bool ContainsKey(Object key)
	{
		return _dictionary.ContainsKey(key);
	}

	public bool TryAdd(Object key, PPtr value)
	{
#if NETSTANDARD
		if (_dictionary.ContainsKey(key))
		{
			return false;
		}
		else
		{
			_dictionary.Add(key, value);
			return true;
		}
#else
		return _dictionary.TryAdd(key, value);
#endif
	}

	public bool TryGetValue(Object key, out PPtr value)
	{
		return _dictionary.TryGetValue(key, out value);
	}

	public IEnumerator<KeyValuePair<Object, PPtr>> GetEnumerator() => _dictionary.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	bool ICollection<KeyValuePair<Object, PPtr>>.IsReadOnly => ((ICollection<KeyValuePair<Object, PPtr>>)_dictionary).IsReadOnly;

	void ICollection<KeyValuePair<Object, PPtr>>.Add(KeyValuePair<Object, PPtr> item)
	{
		((ICollection<KeyValuePair<Object, PPtr>>)_dictionary).Add(item);
	}

	bool ICollection<KeyValuePair<Object, PPtr>>.Contains(KeyValuePair<Object, PPtr> item)
	{
		return ((ICollection<KeyValuePair<Object, PPtr>>)_dictionary).Contains(item);
	}

	void ICollection<KeyValuePair<Object, PPtr>>.CopyTo(KeyValuePair<Object, PPtr>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<Object, PPtr>>)_dictionary).CopyTo(array, arrayIndex);
	}

	bool ICollection<KeyValuePair<Object, PPtr>>.Remove(KeyValuePair<Object, PPtr> item)
	{
		return ((ICollection<KeyValuePair<Object, PPtr>>)_dictionary).Remove(item);
	}
}

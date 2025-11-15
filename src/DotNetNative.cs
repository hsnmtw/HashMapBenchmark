

public sealed class DotNetNative : IHashMap
{
    private readonly Dictionary<string,int> dictionary = [];

    public void Clear()
    {
        dictionary.Clear();
    }

    public long Misses()
    {
        return 0;
    }

    public int Count()
    {
        return dictionary.Count;
    }

    public int Get(string key)
    {
        return dictionary.TryGetValue(key, out var value) ? value : 0;
    }

    public IEnumerable<(string key,int value)> Entries()
    {
        return dictionary.Select(x=>(x.Key,x.Value));
    }

    public IEnumerable<string> Keys()
    {
        return dictionary.Keys;
    }

    public void Put(string key, int value)
    {
        dictionary[key] = value;
    }

    public void Increment(string key)
    {
        if (!dictionary.ContainsKey(key)) dictionary[key] = 0;
        dictionary[key] += 1;
    }
}
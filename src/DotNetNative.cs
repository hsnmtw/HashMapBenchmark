

public sealed class DotNetNative : IHashMap
{
    private readonly Dictionary<string,int> dictionary = [];

    public void Clear()
    {
        dictionary.Clear();
    }

    public int Collisions()
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

    public int Get(int index)
    {
        return 0;
    }

    public void Increment(string key)
    {
        if (!dictionary.ContainsKey(key)) dictionary[key] = 0;
        dictionary[key]++;
    }

    public string[] Keys()
    {
        return [.. dictionary.Keys];
    }

    public void Put(string key, int value)
    {
        dictionary[key] = value;
    }

    public void Put(int index, string key, int value)
    {
        Put(key,value);
    }
}
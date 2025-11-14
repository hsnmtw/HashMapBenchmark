public sealed class NaeiveImpl : IHashMap
{
    private (string key, int value)[] entiries = new (string key, int value)[4197];

    public void Clear()
    {
        entiries = new (string key, int value)[4197];
    }

    public void Put(int index,string key, int value)
    {
        entiries[index] = (key,value);
    }

    public int Collisions()
    {
        return 0;
    }

    public int Count()
    {
        return entiries.Length;
    }

    public int Get(string key)
    {
        foreach(var (k,v) in entiries)
        {
            if(Equals(k,key)) return v;   // O(n) ====> O(1/3)
        }
        return 0;
    }

    public int Get(int index)
    {
        return entiries[index].value;
    }

    public int Index(string key)
    {
        int n = entiries.Length;
        for(int i=0;i<n;++i)
        {
            var (k,v) = entiries[i];
            if(Equals(k,key)) return i; 
        }
        return 0;
    }

    public string[] Keys()
    {
        return [.. entiries.Select(x=>x.key)];
    }

    public void Put(string key, int value)
    {
        int n = entiries.Length;
        int index = Index(key);
        for (int idx = 0; idx < n; idx++)
        {
            var (k,v) = entiries[(index+idx)%n];
            if(string.IsNullOrEmpty(k) || Equals(k,key))
            {
                entiries[(index+idx)%n] = (key,value);
                return;
            }
        }
        var table = new (string key, int value)[n*7];
        for (int i = 0; i < n; i++)
        {
            table[i] = entiries[i];
        }
        table[n] = (key,value);
        entiries = table;
    }

    public void Increment(string key)
    {
        int index = Index(key);
        entiries[index].key = key;
        entiries[index].value++;
    }
}
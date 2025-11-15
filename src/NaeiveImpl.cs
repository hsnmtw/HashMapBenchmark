public sealed class NaeiveImpl : IHashMap
{
    private int pos;
    private (string key, int value)[] entiries = new (string key, int value)[4197];

    public void Clear()
    {
        entiries = new (string key, int value)[4197];
    }

    private long _misses;
    public long Misses()
    {
        return _misses;
    }

    public int Count()
    {
        return pos+1;
    }

    public int Get(string key)
    {
        return entiries[Index(key)].value;
    }

    public IEnumerable<(string key,int value)> Entries()
    {
        return entiries[0..pos];
    }

    public int Index(string key)
    {
        int n = entiries.Length;
        for(int i=0;i<n;++i)
        {
            var k = entiries[i].key;
            if(string.IsNullOrEmpty(k) || Equals(k,key)) return i; 
            _misses++;
        }
        return 0;
    }


    //private readonly HashSet<string> _keys = [];
    public IEnumerable<string> Keys()
    {
        return entiries[0..pos].Select(x=>x.key);
    }

    public void Put(string key, int value)
    {
        int n = entiries.Length;
        
        if (n > pos)
        {
            int index = Index(key);
            entiries[index] = (key,value);
            //_keys.Add(key);
            pos++;
            return;
        }

        var table = new (string key, int value)[n*7];
        for (int i = 0; i < n; i++)
        {
            table[i] = entiries[i];
        }

        table[n] = (key,value);
        entiries = table;
        //_keys.Add(key);
        pos++;

    }

    public void Increment(string key)
    {
        if (entiries.Length <= pos) Put(key,0);
        int index = Index(key);
        entiries[index].key    = key;
        entiries[index].value += 1;
        //_keys.Add(key);
        pos++;
    }
}
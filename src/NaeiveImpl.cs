using System.Collections.Specialized;
using System.Runtime.InteropServices;

public sealed class NaeiveImpl : IHashMap
{
    private int _count;
    private (string key, int value)[] entiries = new (string key, int value)[3793];

    public void Clear()
    {
        entiries = new (string key, int value)[3793];
    }

    private long _misses;
    public long Misses()
    {
        return _misses;
    }

    public int Count()
    {
        return _count+1;
    }

    public int Get(string key)
    {
        return entiries[Index(key)].value;
    }

    public IEnumerable<(string key,int value)> Entries()
    {
        return entiries[0.._count];
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
        return entiries[0.._count].Select(x=>x.key);
    }

    public void Put(string key, int value)
    {
        int n = entiries.Length;
        bool is_empty;
        
        if (n > _count)
        {
            int index = Index(key);
            is_empty = index>=_count && string.IsNullOrEmpty(entiries[index].key);
            entiries[index] = (key,value);
            //_keys.Add(key);
            if(is_empty) _count++;
            return;
        }

        Array.Resize(ref entiries, n*2);
        entiries[n] = (key,value);
        _count++;
    }

    public void Increment(string key)
    {
        if (_count >= entiries.Length) Put(key,0);
        int index = Index(key);
        bool is_empty = index >= _count && string.IsNullOrEmpty(entiries[index].key);
        entiries[index].key    = key;
        entiries[index].value += 1;
        if(is_empty) _count++;
    }
}
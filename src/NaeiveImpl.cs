using System.Collections.Specialized;
using System.Runtime.InteropServices;

internal record struct Entry(string key, int value);

public sealed class NaeiveImpl : IHashMap
{
    private int _count = 0;
    private Entry[] entiries = new Entry[3793];

    public void Clear()
    {
        entiries = new Entry[3793];
    }

    private long _misses;
    public long Misses()
    {
        return _misses;
    }

    public int Count()
    {
        return _count;
    }

    public int Get(string key)
    {
        if (string.IsNullOrEmpty(key)) return 0;
        return entiries[Index(key)].value;
    }

    public IEnumerable<(string key,int value)> Entries()
    {
        return entiries[0.._count].Select(x=>(x.key,x.value));
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
        if (string.IsNullOrEmpty(key)) return;

        int n = entiries.Length;
        bool is_empty;
        
        if (n > _count)
        {
            int index = Index(key);
            is_empty = string.IsNullOrEmpty(entiries[index].key);
            entiries[index] = new(key,value);
            if(is_empty) _count++;
            return;
        }

        Array.Resize(ref entiries, n*2);
        entiries[n] = new(key,value);
        _count++;

    }

    public void Increment(string key)
    {
        if (string.IsNullOrEmpty(key)) return;
        int n = entiries.Length;
        if (_count >= n) {
            Array.Resize(ref entiries, n*2);
        }
        int index = Index(key);
        bool is_empty = string.IsNullOrEmpty(entiries[index].key);
        entiries[index].key    = key;
        entiries[index].value += 1;
        if(is_empty) _count++;
    }
}
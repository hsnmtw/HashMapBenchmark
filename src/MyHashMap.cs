
public sealed class MyHashMap : IHashMap
{
    private (string key, int value)[] entiries = new (string key, int value)[1041973];

    private readonly HashSet<string> _keys = [];

    private long _misses;

    private static readonly int[] primes = [3,5,7,11,13,19,23,29]; 
    private int Hash(string key)
    {
        int l = key.Length;
        long sum = (long)Math.Pow(l,l); 
        for (int i = 0; i < key.Length; i++)
        {
            byte c = (byte)key[i];
            int p = primes[i%primes.Length];
            sum =  ((sum*p ^ c*(l-i+1)) << 3) + c*p;
        }
        int hash = (int)sum;
        return hash < 0 ? -hash : hash;
    }


    public void Clear()
    {
        entiries = new (string key, int value)[10_000_000];
    }

    public int Count()
    {
        return Keys().Count();
    }

    public int Get(string key)
    {
        return entiries[Index(key)].value;
    }

    public IEnumerable<string> Keys()
    {
        return _keys;
    }

    

    public void Put(int index,string key, int value)
    {
        entiries[index] = (key,value);
        _keys.Add(key);

    }

    public void Put(string key, int value)
    {
        int keys_count = Keys().Count();
        int n = entiries.Length;
        _keys.Add(key);
        
        if (keys_count < n) {
            int index = Index(key);
            entiries[index] = (key,value);
            return;
        }


        n = entiries.Length*7;
        var table = new (string key, int value)[n];
        for(int i = 0; i < entiries.Length; ++i)
        {
            var (k,v) = entiries[i];
            if (string.IsNullOrEmpty(k)) continue;
            int t = Hash(k)%n;
            for(int j=0;j<n;++j)
            {
                int idx = (t + j) % n;
                if (string.IsNullOrEmpty(table[idx].key))
                {
                    table[idx] = (k, v);
                    break;                
                }
                _misses++;
            }
        }

        entiries = table;
        Put(key,value);
    }

    public long Misses()
    {
        return _misses;
    }

    public int Index(string key)
    {
        int n = entiries.Length;
        int index = Hash(key) % n;
        for (int i = 0; i < n; i+=1)
        {
            int x = (i+index)%n;
            var k = entiries[x].key;
            if (string.IsNullOrEmpty(k) || Equals(k,key)) return x;
            _misses++;
        }
        return index;
    }

    public void Increment(string key)
    {
        if (entiries.Length <= Keys().Count()) Put(key,0);
        int index = Index(key);
        entiries[index].key    = key;
        entiries[index].value += 1;
        _keys.Add(key);
    }

    public IEnumerable<(string key,int value)> Entries()
    {
        return entiries.Where(x=>!string.IsNullOrEmpty(x.key));
    }
}
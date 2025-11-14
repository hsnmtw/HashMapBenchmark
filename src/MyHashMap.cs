
public sealed class MyHashMap : IHashMap
{
    private (string key, int value)[] entiries = new (string key, int value)[104197];

    private static readonly int[] primes = [3,5,7,11,13,19,23,29]; 
    private HashSet<string> _keys = [];

    private int _collisions;

    private int Hash(string key)
    {
        //Crc32 crcCalculator = new Crc32();
        
        // return (int)Math.Abs(Crc16CcittKermit.ComputeChecksum(Encoding.UTF8.GetBytes(key)));

        // ------------------
        int l = key.Length;
        long sum = l*(long)primes.Select((x,i)=>Math.Pow(x,i+1)).Sum();
        for (int i = 0; i < key.Length; i++)
        {
            int c = key[i];
            int p = primes[i%primes.Length];
            sum =  (sum*p ^ c*(l-i+1)) << 3;
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
        return Keys().Length;
    }

    public int Get(string key)
    {
        int n = entiries.Length;
        int index = Hash(key) % n;
        for (int i = 0; i < n; i++)
        {
            int idx = (index+i) % n;
            var (k,v) = entiries[idx];
            if (string.IsNullOrEmpty(k) || Equals(k,key)) return v;
        }
        return 0;
    }

    public string[] Keys()
    {
        return [.. _keys];//entiries.Where(x => !string.IsNullOrEmpty(x.key)).Select(x=>x.key)];
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
        if (keys_count < n) {
            int index = Index(key);
            entiries[index] = (key,value);
            _keys.Add(key);
            return;
        }


        // System.Console.WriteLine("got here ... need to re-hash");
        n = entiries.Length*7;
        var table = new (string key, int value)[n];
        // var keys = Keys();
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
                _collisions++;
            }
            // System.Console.Write("keys[{0}]='{1}'",i,keys[i]);
            // Console.ReadLine();
        }
        // System.Console.WriteLine("done re-hashing");
        entiries = table;
        Put(key,value);
    }

    public int Collisions()=>_collisions;

    public int Get(int index)
    {
        return entiries[index].value;
    }

    public int Index(string key)
    {
        int n = entiries.Length;
        int index = Hash(key) % n;
        for (int i = 0; i < n; i++)
        {
            int x = (i+index)%n;
            var (k,_) = entiries[x];
            if (string.IsNullOrEmpty(k) || Equals(k,key)) return x;
            _collisions++;
        }
        return index;
    }

    public void Increment(string key)
    {
        int index = Index(key);
        entiries[index].key = key;
        entiries[index].value++;
        _keys.Add(key);
    }
}
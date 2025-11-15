# HashMapBenchmark

```c#
RunBenchMark("Native", new DotNetNative(), 1);
RunBenchMark("OurMap", new MyHashMap(),    1);
RunBenchMark("Naeive", new NaeiveImpl(),   1);
```

*the following is the test results:*
```
D:\coding\hash_map>dotnet run --release
Hash Map [Native]
----------------------------------------------------------------------------------------
Name                                Words            Most Misses  Elapsed min/avg/max (ms)
----------------------------------------------------------------------------------------
AbigailsTale.txt           4KB       307    to      37        ?       9       9       9
black-peter.txt           48KB     2,628   the     502        ?      23      23      23
houn.txt                 346KB    10,268   the    3570        ?     168     168     168
t8.shakespeare.txt     5,458KB    72,105   the   26812        ?   2,532   2,532   2,532
----------------------------------------------------------------------------------------
Hash Map [OurMap]
----------------------------------------------------------------------------------------
Name                                Words            Most Misses  Elapsed min/avg/max (ms)
----------------------------------------------------------------------------------------
AbigailsTale.txt           4KB       307    to      37        1       2       2       2
black-peter.txt           48KB     2,628   the     502       73      23      23      23
houn.txt                 346KB    10,268   the    3570     2035     171     171     171
t8.shakespeare.txt     5,458KB    72,105   the   26812    96143   2,852   2,852   2,852
----------------------------------------------------------------------------------------
Hash Map [Naeive]
----------------------------------------------------------------------------------------
Name                                Words            Most Misses  Elapsed min/avg/max (ms)
----------------------------------------------------------------------------------------
AbigailsTale.txt           4KB       684    to      37    71835       2       2       2
black-peter.txt           48KB     8,872   the     502  5516372      81      81      81
houn.txt                 346KB    68,124   the    3570 108842592   1,309   1,309   1,309
t8.shakespeare.txt     5,458KB   969,450   the   26812 9171151790 152,264 152,264 152,264
----------------------------------------------------------------------------------------
```

The hash function used is not proven to give a unique result for any key, yet, it performed
very well.

```c#
    private static readonly int[] primes = [3,5,7,11,13,19,23,29]; 
    private int Hash(string key)
    {
        int l = key.Length;
        long sum = (long)Math.Pow(l,l); 
        for (int i = 0; i < key.Length; i++)
        {
            byte c = (byte)key[i];
            int p = primes[i%primes.Length];
            sum =  (sum*p ^ c*(l-i+1)) << 5;
        }
        int hash = (int)sum;
        return hash < 0 ? -hash : hash;
    }
```

-- there is a bug in the naeive implementation not getting accurate word count, but who cares
it was not meant to be accurate, it was added to compare algorithm time roughly
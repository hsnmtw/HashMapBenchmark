# HashMapBenchmark

```c#
RunBenchMark("Native", new DotNetNative(), 1);
RunBenchMark("OurMap", new MyHashMap(),    1);
RunBenchMark("Naeive", new NaeiveImpl(),   1);
```

Algorithm used:
```vb
    
    function get_index(key) {
        n := items.count
        index := hash(key) MODULO n

        if item[index].key == NULL or item[index].key == key then
            return index
        end if

        ' the key was not found using hashing function
        ' and the spot in the array is not empty, look for next spot
        ' if the spot at the hashed key (index) is not empty, then 
        ' this means we have a collision

        for i := 0 to n 
            x := (index+i) MODULO n
            if items[x].key == NULL or item[x].key == key then
                return x
            end if
            collisions = collisions + 1
        next

        ' the array is full and needs expansion

        items = expand(n*2)
        return index
    }

    function put(key,value) {
        index := get_index(key)
        items[index] = (key,value); 
    }

    function get(key) {
        index := get_index(key)
        return items[index].value
    }

```

*the following is the test results:*
```shell
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
AbigailsTale.txt           4KB       308    to      37    71835      15      15      15
black-peter.txt           48KB     2,629   the     502  5516372     189     189     189
houn.txt                 346KB    10,271   the    3570 108846903   2,553   2,553   2,553
t8.shakespeare.txt     5,458KB    72,111   the   26812 9171700239 219,018 219,018 219,018
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

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
$ dotnet run --release

Hash Map [Native] with # of iterations = 5
----------------------------------------------------------------------------------------
Name                                Words            Most Misses  Elapsed min/avg/max (ms)
----------------------------------------------------------------------------------------
AbigailsTale.txt           4KB       307    to     185        0       2       4       9
black-peter.txt           48KB     2,628   the    2510        0      20      23      26
houn.txt                 346KB    10,268   the   17850        0     162     182     241
t8.shakespeare.txt     5,458KB    72,105   the  134060        0   2,409   2,589   3,151
----------------------------------------------------------------------------------------

Hash Map [OurMap] with # of iterations = 5
----------------------------------------------------------------------------------------
Name                                Words            Most Misses  Elapsed min/avg/max (ms)
----------------------------------------------------------------------------------------
AbigailsTale.txt           4KB       307    to     185        3       2       3       4
black-peter.txt           48KB     2,628   the    2510      221      32      46      96
houn.txt                 346KB    10,268   the   17850     6251     239     248     262
t8.shakespeare.txt     5,458KB    72,105   the  134060   292499   3,872   3,954   4,039
----------------------------------------------------------------------------------------

Hash Map [Naeive] with # of iterations = 5
----------------------------------------------------------------------------------------
Name                                Words            Most Misses  Elapsed min/avg/max (ms)
----------------------------------------------------------------------------------------
AbigailsTale.txt           4KB       307    to     185      215505       3       4       4
black-peter.txt           48KB     2,628   the    2510    16692786     117     128     132
houn.txt                 346KB    10,268   the   17850   337530943   2,032   2,068   2,125
t8.shakespeare.txt     5,458KB    72,105   the  134060 27729663838 182,662 193,562 208,987
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
            sum =  (sum*p ^ c*(l-i)) << 3;
        }
        int hash = (int)sum;
        return hash < 0 ? -hash : hash;
    }
```

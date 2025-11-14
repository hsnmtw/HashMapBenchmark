using System.Diagnostics;
using System.Text;

Console.WriteLine("Hash Map Implementation from scratch !");

static long Benchmark(string file, IHashMap map)
{
    // System.Console.WriteLine("Benchmarking {0}", file);
    //IHashMap map = new DotNetNative();
    // string   text  = File.ReadAllText(file);
    // string[] words = text.Split([" ","\n","\t","\r","\0"], StringSplitOptions.RemoveEmptyEntries);

    using var fs = File.OpenRead(file);

    Stopwatch sw = Stopwatch.StartNew();
    // foreach (var word in words)
    // {
    //     int pvalue = map.Get(word);
    //     map.Put(word, pvalue+1);
    // }
    int top = Console.CursorTop;
    int length = 256;
    byte[] chars = new byte[length];
    int r = 0;
    string word = "";
    long size = new FileInfo(file).Length;
    double sofar = 0;
    while( (r = fs.Read(chars,0,length)) > 0)
    {
        sofar += r;
        Console.CursorTop = top;
        Console.CursorLeft = 0;
        System.Console.Write("[{0}]: {1:P2}", file, sofar/size);
        for(int i = 0; i < r; ++i)
        {
            char c = (char)chars[i];
            if ( c == ' ' || c == '\r' || c =='\n' || c =='\t' || c =='\0')
            {
                if (word.Length == 0) continue;
                //int index = map.Index(word);
                map.Increment(word);
                word = "";
                continue;
            } 
            word = $"{word}{c}";
        }
    }
    Console.CursorLeft = 0;

    fs.Close();

    sw.Stop();

    // var entries = map.Keys().Select(x => (map.Get(x), x)).OrderDescending();//.FirstOrDefault();    
    // System.Console.WriteLine("keys: {0}", map.Keys().Length);
    // foreach(var (v,k) in entries.Take(5))
    // {
    //     System.Console.WriteLine("'{0}' = {1}", k,v);//entries.x, entries.Item1);
    // }

    //System.Console.WriteLine("Elapsed: {0}ms\n------------------------------------\n", sw.ElapsedMilliseconds);

    return sw.ElapsedMilliseconds;
}

string[] stories = [
    "AbigailsTale.txt",
    "black-peter.txt",
    "houn.txt",
   "t8.shakespeare.txt"
];

System.Console.WriteLine("---------------------------------------------------------------------------");
System.Console.WriteLine("{0,-30} {1,15} {2,15} {3,6}(ms)","Name","Iterations","Collisions","Elapsed");
System.Console.WriteLine("---------------------------------------------------------------------------");

foreach(var file in stories)
{
    double sum = 0;
    double n = 10.0;
    long collisions=0;
    for(int i=0;i<n;++i)
    {
        var map = new MyHashMap();
        sum += Benchmark(file, map);
        collisions+=map.Collisions();
    }
    System.Console.WriteLine("{0,-30} {1,15:N0} {2,15:N0} {3,10:N0}", file, n, collisions, sum/n);

}
System.Console.WriteLine("---------------------------------------------------------------------------");


/*
AbigailsTale.txt     1000 iterations Elapsed: 0.007ms
------------------------------------------------------------
black-peter.txt      1000 iterations Elapsed: 0.584ms
------------------------------------------------------------
houn.txt             1000 iterations Elapsed: 6.039ms
------------------------------------------------------------
t8.shakespeare.txt   1000 iterations Elapsed: 127.728ms
------------------------------------------------------------
*/
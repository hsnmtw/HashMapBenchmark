using System.Diagnostics;
using System.Text;


static long Benchmark(string file, IHashMap map, int iteration, int iterations)
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
        try{
            Console.CursorTop = top;
            Console.CursorLeft = 0;
        } catch {}
        System.Console.Write("[{0}/{1}] [{2}]: {3:P2}", iteration, iterations, file, sofar/size);
        for(int i = 0; i < r; ++i)
        {
            char c = (char)chars[i];
            if ( c == ' ' || c == '\r' || c =='\n' || c =='\t' || c =='\0')
            {
                if (word.Length == 0) continue;
                //map.Put(word, map.Get(word)+1);
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
    
void RunBenchMark(string name, IHashMap map, int iterations)
{

    Console.WriteLine("Hash Map [{0}] with # of iterations = {1}", name, iterations);
    System.Console.WriteLine("----------------------------------------------------------------------------------------");
    System.Console.WriteLine("{0,-25} {1,15} {2,15} {3}  {4} (ms)","Name","Words","Most","Misses","Elapsed min/avg/max");
    System.Console.WriteLine("----------------------------------------------------------------------------------------");

    foreach(var file in stories)
    {
        double sum = 0;
        double min = double.MaxValue;
        double max = double.MinValue;
        long misses=0;
        for(int i=0;i<iterations;++i)
        {
            var elapsed = Benchmark(file, map, i+1, iterations);
            sum += elapsed;
            max = max < elapsed ? elapsed : max;
            min = min > elapsed ? elapsed : min;
            misses+=map.Misses();
        }
        var (v,k) = map.Entries().Select(x => (x.value, x.key)).OrderDescending().FirstOrDefault();
        System.Console.WriteLine("{0,-20} {1,7:N0}KB {2,9:N0} {3,5:N0} {4,7} {5,8} {6,7:N0} {7,7:N0} {8,7:N0}", file ,new FileInfo(file).Length*1.0d/1000.0, map.Count(), k,v, misses/iterations, min,sum/iterations, max);

    }
    System.Console.WriteLine("----------------------------------------------------------------------------------------");
}

RunBenchMark("Native", new DotNetNative(), 3);
RunBenchMark("OurMap", new MyHashMap(),    3);
RunBenchMark("Naeive", new NaeiveImpl(),   3);


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
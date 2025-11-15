public interface IHashMap
{
    int Count();
    void Put(string key, int value);
    void Increment(string key);
    int  Get(string key);
    void Clear();
    IEnumerable<string> Keys();
    IEnumerable<(string key,int value)> Entries();
    long Misses();
}
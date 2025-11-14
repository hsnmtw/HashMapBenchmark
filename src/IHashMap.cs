public interface IHashMap
{
    int Count();
    void Put(string key, int value);
    void Increment(string key);
    // void Put(int index,string key, int value);
    int  Get(string key);
    int  Get(int index);
    void Clear();
    string[] Keys();
    // int Index(string key);

    int Collisions();
}
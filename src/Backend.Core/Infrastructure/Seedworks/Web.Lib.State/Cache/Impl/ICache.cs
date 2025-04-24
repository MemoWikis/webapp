using System.Collections;

namespace Seedworks.Web.State
{
    internal interface ICache
    {
        void Add(string key, object obj);
        void Add(string key, object obj, TimeSpan expiration, bool slidingExpiration);
        object Get(string key);
        Type Get<Type>(string key);
        void Clear();
        void Remove(string key);
        int Count { get; }
        IDictionaryEnumerator GetEnumerator();
    }
}

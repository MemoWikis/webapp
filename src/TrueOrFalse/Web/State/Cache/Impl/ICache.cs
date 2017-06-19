using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Web.State
{
    internal interface ICache
    {
        void Add(string key, object obj);
        void Add(string key, object obj, TimeSpan expiration);
        object Get(string key);
        Type Get<Type>(string key);
        void Clear();
        void Remove(string key);
        int Count { get; }
        IDictionaryEnumerator GetEnumerator();
    }
}

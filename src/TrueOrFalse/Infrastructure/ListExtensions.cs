using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Seedworks.Lib.Persistence;

public static class ListExtensions
{
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        if (list.Count > 255)
        {
            Random rng = new Random();
            int m = list.Count;
            while (m > 1)
            {
                m--;
                int k = rng.Next(m + 1);
                T value = list[k];
                list[k] = list[m];
                list[m] = value;
            }

            return list;
        }

        //http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
        var provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    public static int IndexOf<T>(this IList<T> list, Predicate<T> match)
    {
        for (var i = 0; i < list.Count; i++)
            if (match(list[i]))
                return i;

        return -1;
    }

    public static ConcurrentDictionary<int, T> ToConcurrentDictionary<T>(this IList<T> list) where T : DomainEntity
    {
        return new ConcurrentDictionary<int, T>(list.Select(i => new KeyValuePair<int, T>(i.Id, i)));
    }

    public static ConcurrentDictionary<int, UserCacheCategory> ToConcurrentDictionary(this IList<UserCacheCategory> list)
    {
        return new ConcurrentDictionary<int, UserCacheCategory>(list.Select(UserCacheCategory => new KeyValuePair<int, UserCacheCategory>(UserCacheCategory.Id, UserCacheCategory)));
    }
}

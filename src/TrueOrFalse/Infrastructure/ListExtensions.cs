using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
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

            return;
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
    }

    public static int IndexOf<T>(this IList<T> list, Predicate<T> match)
    {
        for (var i = 0; i < list.Count; i++)
            if (match(list[i]))
                return i;

        return -1;
    }
}

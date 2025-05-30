﻿using System.Collections.Concurrent;
using System.Security.Cryptography;

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

    public static ConcurrentDictionary<int, TElement> ToConcurrentDictionary<TElement>(
        this IList<TElement> list)
        where TElement : IPersistable
    {
        return new ConcurrentDictionary<int, TElement>(
            list.Select(item => new KeyValuePair<int, TElement>(item.Id, item))
        );
    }

    public static ConcurrentDictionary<int, QuestionCacheItem> ToConcurrentDictionary(
        this IList<QuestionCacheItem> list)
    {
        return new ConcurrentDictionary<int, QuestionCacheItem>(list.Select(UserCacheQuestion =>
            new KeyValuePair<int, QuestionCacheItem>(UserCacheQuestion.Id, UserCacheQuestion)));
    }

    public static ConcurrentDictionary<(int, int), ImageMetaData> ToConcurrentDictionary(
        this IList<ImageMetaData> list)
    {
        return new ConcurrentDictionary<(int, int), ImageMetaData>(list.Select(imageMetaData =>
            new KeyValuePair<(int, int), ImageMetaData>(
                (imageMetaData.TypeId, (int)imageMetaData.Type), imageMetaData)));
    }

    public static ConcurrentDictionary<int, List<ShareCacheItem>> ToConcurrentDictionary(
        this List<ShareCacheItem> shareInfos)
    {
        var grouped = shareInfos
            .GroupBy(s => s.PageId)
            .ToDictionary(
                g => g.Key,
                g => g.ToList()
            );

        return new ConcurrentDictionary<int, List<ShareCacheItem>>(grouped);
    }
}
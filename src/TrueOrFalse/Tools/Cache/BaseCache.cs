﻿
using System.Collections.Concurrent;
using CacheManager.Core;


public static class Cache
{
    internal static ICacheManager<object> Mgr = CacheFactory.Build<object>(settings =>
    {
        settings.WithDictionaryHandle();
    });

    public static void IntoForeverCache<T>(string key, ConcurrentDictionary<int, T> objectToCache)
    {
        Mgr.Add(key, objectToCache);
    }

    public static void IntoForeverCache<T>(string key, ConcurrentDictionary<DateTime, T> objectToCache)
    {
        Mgr.Add(key, objectToCache);
    }

    public static void IntoForeverCache<T>(string key, ConcurrentDictionary<(int, int), T> objectToCache)
    {
        Mgr.Add(key, objectToCache);
    }
}


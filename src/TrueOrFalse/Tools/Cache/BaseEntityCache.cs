﻿
using System.Collections.Concurrent;
using CacheManager.Core;


public class BaseEntityCache
{
    protected static ICacheManager<object> _cache;

    public static void IntoForeverCache<T>(string key, ConcurrentDictionary<int, T> objectToCache)
    { 
        _cache = CacheFactory.Build<object>(settings =>
        {
            settings.WithDictionaryHandle();
        });
        _cache.Add(key, objectToCache);
    }
}


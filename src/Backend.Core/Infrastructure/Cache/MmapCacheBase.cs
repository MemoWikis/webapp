using MessagePack;

public abstract class MmapCacheBase<T>
{
    private readonly string _filePath;
    private readonly object _lock = new();
    private readonly string _cacheName;

    protected abstract int SchemaVersionValue { get; }

    public string FilePath => _filePath;

    protected MmapCacheBase(string fileName, string cacheName)
    {
        _cacheName = cacheName;
        var cacheDirectory = Settings.MmapCachePath;
        Directory.CreateDirectory(cacheDirectory);
        _filePath = Path.Combine(cacheDirectory, fileName);
    }

    public List<T> Load()
    {
        if (!File.Exists(_filePath))
        {
            return new List<T>();
        }

        try
        {
            lock (_lock)
            {
                using var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var items = MessagePackSerializer.Deserialize<List<T>>(fileStream);

                var rejection = MmapCacheMetadata.Validate(_filePath, SchemaVersionValue, items.Count);
                if (rejection != null)
                {
                    Log.Warning("{CacheName} mmap cache rejected: {Reason} — falling back to DB", _cacheName, rejection);
                    return new List<T>();
                }

                Log.Information("Loaded {Count} {CacheName} entries from mmap cache", items.Count, _cacheName);
                return items;
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to load {CacheName} from mmap cache", _cacheName);
            File.Delete(_filePath);
            return new List<T>();
        }
    }

    public void SaveAll(IList<T> items)
    {
        lock (_lock)
        {
            var tempFile = _filePath + ".tmp";
            File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(items));
            File.Move(tempFile, _filePath, true);
            MmapCacheMetadata.Save(_filePath, items.Count, SchemaVersionValue);
            Log.Information("Saved {Count} {CacheName} entries to mmap cache", items.Count, _cacheName);
        }
    }

    public void DeleteCacheFile()
    {
        lock (_lock)
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            MmapCacheMetadata.Delete(_filePath);
        }
    }

    protected void AppendSingle(T item, string itemDescription)
    {
        try
        {
            lock (_lock)
            {
                var existingItems = LoadFromFileUnsafe();
                existingItems.Add(item);
                WriteToFileUnsafe(existingItems);

                Log.Debug("Appended {CacheName} entry to mmap cache: {Description}, Total: {Count}",
                    _cacheName, itemDescription, existingItems.Count);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to append {CacheName} entry to mmap cache: {Description}",
                _cacheName, itemDescription);
        }
    }

    protected void AppendRange(IEnumerable<T> items)
    {
        var itemsList = items.ToList();
        if (itemsList.Count == 0)
        {
            Log.Debug("No {CacheName} entries to append to mmap cache", _cacheName);
            return;
        }

        try
        {
            lock (_lock)
            {
                var existingItems = LoadFromFileUnsafe();
                existingItems.AddRange(itemsList);
                WriteToFileUnsafe(existingItems);

                Log.Information("Appended {AppendedCount} {CacheName} entries to mmap cache. Total: {TotalCount}",
                    itemsList.Count, _cacheName, existingItems.Count);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to append {Count} {CacheName} entries to mmap cache",
                itemsList.Count, _cacheName);
        }
    }

    protected void DeleteWhere(Func<T, bool> predicate, string filterDescription)
    {
        try
        {
            lock (_lock)
            {
                if (!File.Exists(_filePath))
                {
                    Log.Debug("No mmap cache file exists for deleting {CacheName} entries: {Description}",
                        _cacheName, filterDescription);
                    return;
                }

                using var readStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var existingItems = MessagePackSerializer.Deserialize<List<T>>(readStream);
                readStream.Close();

                var deletedCount = existingItems.Count(predicate);
                if (deletedCount == 0)
                {
                    Log.Debug("No {CacheName} entries found to delete: {Description}", _cacheName, filterDescription);
                    return;
                }

                var filteredItems = existingItems.Where(item => !predicate(item)).ToList();
                WriteToFileUnsafe(filteredItems);

                Log.Information("Deleted {DeletedCount} {CacheName} entries ({Description}). Remaining: {RemainingCount}",
                    deletedCount, _cacheName, filterDescription, filteredItems.Count);
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to delete {CacheName} entries from mmap cache: {Description}",
                _cacheName, filterDescription);
        }
    }

    private List<T> LoadFromFileUnsafe()
    {
        if (!File.Exists(_filePath))
        {
            return new List<T>();
        }

        using var readStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return MessagePackSerializer.Deserialize<List<T>>(readStream);
    }

    private void WriteToFileUnsafe(List<T> items)
    {
        var tempFile = _filePath + ".tmp";
        File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(items));
        File.Move(tempFile, _filePath, true);
        MmapCacheMetadata.Save(_filePath, items.Count, SchemaVersionValue);
    }
}

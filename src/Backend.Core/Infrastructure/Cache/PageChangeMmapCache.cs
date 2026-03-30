using MessagePack;

[MessagePackObject]
public record struct PageChangeSummary(
    [property: Key(0)] int Id,
    [property: Key(1)] int PageId,
    [property: Key(2)] int DataVersion,
    [property: Key(3)] string Data,
    [property: Key(4)] int AuthorId,
    [property: Key(5)] int Type,
    [property: Key(6)] DateTime DateCreated);

public class PageChangeMmapCache : IRegisterAsInstancePerLifetime
{
    private readonly string _pageChangesFilePath;
    private readonly object _pageChangeLock = new();

    public PageChangeMmapCache()
    {
        var cacheDirectory = Settings.MmapCachePath;
        Directory.CreateDirectory(cacheDirectory);

        _pageChangesFilePath = Path.Combine(cacheDirectory, "pagechanges.mmap");
    }

    public List<PageChangeSummary> LoadPageChanges()
    {
        if (!File.Exists(_pageChangesFilePath))
        {
            return new List<PageChangeSummary>();
        }

        try
        {
            lock (_pageChangeLock)
            {
                using var fileStream =
                    new FileStream(_pageChangesFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var changes = MessagePackSerializer.Deserialize<List<PageChangeSummary>>(fileStream);

                Log.Information("Loaded {Count} page changes from mmap cache", changes.Count);
                return changes;
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to load page changes from mmap cache");
            File.Delete(_pageChangesFilePath);
            return new List<PageChangeSummary>();
        }
    }

    public void SaveAllPageChanges(IList<PageChangeSummary> changes)
    {
        lock (_pageChangeLock)
        {
            var tempFile = _pageChangesFilePath + ".tmp";
            File.WriteAllBytes(tempFile, MessagePackSerializer.Serialize(changes));
            File.Move(tempFile, _pageChangesFilePath, true);
            Log.Information("Saved {Count} page changes to mmap cache", changes.Count);
        }
    }

    public void DeleteCacheFile()
    {
        lock (_pageChangeLock)
        {
            if (File.Exists(_pageChangesFilePath))
            {
                File.Delete(_pageChangesFilePath);
            }
        }
    }

    public static IList<PageChange> ToPageChanges(List<PageChangeSummary> summaries)
    {
        return summaries.Select(s => new PageChange
        {
            Id = s.Id,
            Page = new Page { Id = s.PageId },
            DataVersion = s.DataVersion,
            Data = s.Data,
            AuthorId = s.AuthorId,
            Type = (PageChangeType)s.Type,
            DateCreated = s.DateCreated
        }).ToList();
    }

    public static IList<PageChangeSummary> FromPageChanges(IList<PageChange> changes)
    {
        return changes.Select(c => new PageChangeSummary(
            c.Id,
            c.Page?.Id ?? -1,
            c.DataVersion,
            c.Data,
            c.AuthorId,
            (int)c.Type,
            c.DateCreated
        )).ToList();
    }
}

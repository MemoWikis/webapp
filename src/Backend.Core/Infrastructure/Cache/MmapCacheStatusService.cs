using System.Text.Json;

public class MmapCacheStatusService : IRegisterAsInstancePerLifetime
{
    private readonly string _cacheDirectory;
    private readonly string _pageViewsFile;
    private readonly string _questionViewsFile;

    public MmapCacheStatusService()
    {
        _cacheDirectory = Settings.MmapCachePath;
        _pageViewsFile = Path.Combine(_cacheDirectory, "pageviews.mmap");
        _questionViewsFile = Path.Combine(_cacheDirectory, "questionviews.mmap");
    }

    public string GetCacheStatusAsJson()
    {
        var status = new
        {
            pageViewsCache = new
            {
                exists = File.Exists(_pageViewsFile),
                lastModified = File.Exists(_pageViewsFile) ? File.GetLastWriteTime(_pageViewsFile) : (DateTime?)null,
                sizeBytes = File.Exists(_pageViewsFile) ? new FileInfo(_pageViewsFile).Length : 0
            },
            questionViewsCache = new
            {
                exists = File.Exists(_questionViewsFile),
                lastModified = File.Exists(_questionViewsFile) ? File.GetLastWriteTime(_questionViewsFile) : (DateTime?)null,
                sizeBytes = File.Exists(_questionViewsFile) ? new FileInfo(_questionViewsFile).Length : 0
            }
        };

        return JsonSerializer.Serialize(status);
    }

    public object GetCacheStatus()
    {
        return new
        {
            pageViewsCache = new
            {
                exists = File.Exists(_pageViewsFile),
                lastModified = File.Exists(_pageViewsFile) ? File.GetLastWriteTime(_pageViewsFile) : (DateTime?)null,
                sizeBytes = File.Exists(_pageViewsFile) ? new FileInfo(_pageViewsFile).Length : 0
            },
            questionViewsCache = new
            {
                exists = File.Exists(_questionViewsFile),
                lastModified = File.Exists(_questionViewsFile) ? File.GetLastWriteTime(_questionViewsFile) : (DateTime?)null,
                sizeBytes = File.Exists(_questionViewsFile) ? new FileInfo(_questionViewsFile).Length : 0
            }
        };
    }
}
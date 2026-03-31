public class MmapCacheStatusService(
    PageViewMmapCache _pageViewMmapCache,
    QuestionViewMmapCache _questionViewMmapCache,
    PageChangeMmapCache _pageChangeMmapCache) : IRegisterAsInstancePerLifetime
{
    public MmapCacheStatusResponse GetCacheStatus()
    {
        return new MmapCacheStatusResponse(
            PageViewsCache: GetFileStatus(_pageViewMmapCache.FilePath, PageViewMmapCache.SchemaVersion),
            QuestionViewsCache: GetFileStatus(_questionViewMmapCache.FilePath, QuestionViewMmapCache.SchemaVersion),
            PageChangesCache: GetFileStatus(_pageChangeMmapCache.FilePath, PageChangeMmapCache.SchemaVersion));
    }

    private static MmapCacheFileStatus GetFileStatus(string filePath, int expectedSchemaVersion)
    {
        var exists = File.Exists(filePath);
        var metadata = MmapCacheMetadata.Load(filePath);
        long sizeBytes = exists ? new FileInfo(filePath).Length : 0;
        string? validationError = exists
            ? MmapCacheMetadata.ValidateMetadataOnly(filePath, expectedSchemaVersion)
            : null;

        return new MmapCacheFileStatus(
            Exists: exists,
            SizeKb: Math.Round(sizeBytes / 1024.0, 1),
            SizeMb: sizeBytes > 1024 * 1024 ? Math.Round(sizeBytes / (1024.0 * 1024.0), 1) : null,
            EntryCount: metadata?.EntryCount,
            SavedAtUtc: metadata?.SavedAtUtc,
            SchemaVersion: metadata?.SchemaVersion,
            IsValid: exists && validationError == null,
            ValidationError: validationError);
    }
}

public readonly record struct MmapCacheStatusResponse(
    MmapCacheFileStatus PageViewsCache,
    MmapCacheFileStatus QuestionViewsCache,
    MmapCacheFileStatus PageChangesCache);

public readonly record struct MmapCacheFileStatus(
    bool Exists,
    double SizeKb,
    double? SizeMb,
    int? EntryCount,
    DateTime? SavedAtUtc,
    int? SchemaVersion,
    bool IsValid,
    string? ValidationError);
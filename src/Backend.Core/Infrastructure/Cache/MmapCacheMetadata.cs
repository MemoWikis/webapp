using System.Text.Json;

/// <summary>
/// Metadata sidecar for mmap cache files. Written alongside the .mmap file
/// to enable staleness detection and integrity validation on load.
/// </summary>
public class MmapCacheMetadata
{
    public int EntryCount { get; set; }
    public DateTime SavedAtUtc { get; set; }
    public int SchemaVersion { get; set; }

    private static readonly TimeSpan MaxAge = TimeSpan.FromHours(48);

    public static string GetMetadataPath(string mmapFilePath)
    {
        return mmapFilePath + ".meta";
    }

    public static void Save(string mmapFilePath, int entryCount, int schemaVersion)
    {
        var metadata = new MmapCacheMetadata
        {
            EntryCount = entryCount,
            SavedAtUtc = DateTime.UtcNow,
            SchemaVersion = schemaVersion
        };

        var json = JsonSerializer.Serialize(metadata);
        var metaPath = GetMetadataPath(mmapFilePath);
        var tempFile = metaPath + ".tmp";
        File.WriteAllText(tempFile, json);
        File.Move(tempFile, metaPath, true);
    }

    public static MmapCacheMetadata? Load(string mmapFilePath)
    {
        var metaPath = GetMetadataPath(mmapFilePath);
        if (!File.Exists(metaPath))
        {
            return null;
        }

        try
        {
            var json = File.ReadAllText(metaPath);
            return JsonSerializer.Deserialize<MmapCacheMetadata>(json);
        }
        catch (Exception exception)
        {
            Log.Warning(exception, "Failed to read mmap metadata from {MetaPath}", metaPath);
            return null;
        }
    }

    public static void Delete(string mmapFilePath)
    {
        var metaPath = GetMetadataPath(mmapFilePath);
        if (File.Exists(metaPath))
        {
            File.Delete(metaPath);
        }
    }

    /// <summary>
    /// Validates cache metadata against expected criteria.
    /// Returns a rejection reason string, or null if the cache is valid.
    /// </summary>
    public static string? Validate(string mmapFilePath, int expectedSchemaVersion, int loadedEntryCount)
    {
        var metadata = Load(mmapFilePath);

        if (metadata == null)
        {
            Log.Warning("Mmap cache {Path} has no metadata sidecar — rejecting", mmapFilePath);
            return "no metadata file";
        }

        if (metadata.SchemaVersion != expectedSchemaVersion)
        {
            Log.Warning("Mmap cache {Path} schema version mismatch: expected {Expected}, got {Actual}",
                mmapFilePath, expectedSchemaVersion, metadata.SchemaVersion);
            return $"schema version mismatch (expected {expectedSchemaVersion}, got {metadata.SchemaVersion})";
        }

        var age = DateTime.UtcNow - metadata.SavedAtUtc;
        if (age > MaxAge)
        {
            Log.Warning("Mmap cache {Path} is too old: {Age} (max {MaxAge})",
                mmapFilePath, age, MaxAge);
            return $"cache too old ({age.TotalHours:F1}h, max {MaxAge.TotalHours}h)";
        }

        if (metadata.EntryCount != loadedEntryCount)
        {
            Log.Warning("Mmap cache {Path} entry count mismatch: metadata says {Expected}, loaded {Actual}",
                mmapFilePath, metadata.EntryCount, loadedEntryCount);
            return $"entry count mismatch (metadata: {metadata.EntryCount}, loaded: {loadedEntryCount})";
        }

        return null;
    }

    /// <summary>
    /// Validates metadata without comparing entry count (for status display).
    /// Returns a rejection reason string, or null if the metadata looks valid.
    /// </summary>
    public static string? ValidateMetadataOnly(string mmapFilePath, int expectedSchemaVersion)
    {
        var metadata = Load(mmapFilePath);

        if (metadata == null)
        {
            return "no metadata file";
        }

        if (metadata.SchemaVersion != expectedSchemaVersion)
        {
            return $"schema version mismatch (expected {expectedSchemaVersion}, got {metadata.SchemaVersion})";
        }

        var age = DateTime.UtcNow - metadata.SavedAtUtc;
        if (age > MaxAge)
        {
            return $"cache too old ({age.TotalHours:F1}h, max {MaxAge.TotalHours}h)";
        }

        return null;
    }
}

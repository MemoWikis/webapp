/// <summary>
/// Creates and restores MySQL dumps inside Docker containers used by tests.
/// </summary>
internal static class ScenarioDumpManager
{
    /// <summary>
    /// Creates a new SQL-dump for the current database and stores it beneath /TestData/Dumps.
    /// </summary>
    public static async Task<string> CreateDumpAsync(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            tag = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        string dumpFilePath = ScenarioDumpConstants.GetDumpFilePath(tag);
        Directory.CreateDirectory(Path.GetDirectoryName(dumpFilePath)!);

        var dockerUtilities = new DockerUtilities();
        await dockerUtilities.ExportMySqlDumpAsync(dumpFilePath);

        return dumpFilePath;
    }

    /// <summary>
    /// Loads a previously created dump into the running MySQL container of a TestHarness instance.
    /// </summary>
    public static async Task LoadDumpAsync(string tag)
    {
        string dumpFilePath = ScenarioDumpConstants.GetDumpFilePath(tag);

        if (!File.Exists(dumpFilePath))
            throw new FileNotFoundException($"Dump file not found: {dumpFilePath}");

        var dockerUtilities = new DockerUtilities();
        await dockerUtilities.ImportMySqlDumpAsync(dumpFilePath);
    }
}
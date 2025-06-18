using System.IO;

/// <summary>
/// Shared constants for SQL-dumps.
/// </summary>
internal static class ScenarioDumpConstants
{
    public const string BaseName = "memowikis-test-scenario";
    public const string TagTiny = "micro";

    // Absolute path: …/TestData/Dumps/<basename>_<tag>.sql
    public static string GetDumpFilePath(string tag) =>
        Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", // project-root traversal
            "TestData", "Dumps",
            $"{BaseName}_{tag}.sql"));
}
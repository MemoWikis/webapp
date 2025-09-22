/// <summary>
/// Constants used across test classes
/// </summary>
public static class TestConstants
{
    /// <summary>
    /// MySQL database username for test containers
    /// </summary>
    public const string MySqlUsername = "test";

    /// <summary>
    /// MySQL database password for test containers
    /// </summary>
    public const string MySqlPassword = "P@ssw0rd_#123";

    public const string TestDbName = "memoWikisTest";

    /// <summary>
    /// Fixed port for MySQL test containers (host side)
    /// </summary>
    public const int MySqlTestPort = 3316;

    /// <summary>
    /// Fixed port for Meilisearch test containers (host side)
    /// </summary>
    public const int MeilisearchTestPort = 7778;
}

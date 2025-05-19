using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

public class SessionFactory
{
    private static Configuration _configuration = null!;
    private static readonly HashSet<string> SchemaVersionTables = new() { "SchemaVersion" };

    // Calculate a hash of the schema definition for detecting schema changes
    public static string CalculateSchemaHash()
    {
        var schemaScript = new StringBuilder();
        var schemaExport = new SchemaExport(_configuration);

        schemaExport.SetDelimiter(".");
        schemaExport.Create(sql => schemaScript.AppendLine(sql), execute: false);

        // Use SHA256 to create a hash of the schema script
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(schemaScript.ToString()));

        // Convert to hexadecimal string representation
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }

    // Method to store or update the schema version
    public static void SaveSchemaVersion(string hash)
    {
        using var session = _configuration.BuildSessionFactory().OpenSession();
        using var transaction = session.BeginTransaction();

        var schemaVersion = session.QueryOver<SchemaVersion>().Take(1).SingleOrDefault()
                          ?? new SchemaVersion();

        schemaVersion.SchemaHash = hash;
        schemaVersion.LastUpdated = DateTime.UtcNow;

        session.SaveOrUpdate(schemaVersion);
        transaction.Commit();
    }

    // Get the current schema version hash from database
    public static string? GetCurrentSchemaHash()
    {
        try
        {
            using var session = _configuration.BuildSessionFactory().OpenSession();
            var schemaVersion = session.QueryOver<SchemaVersion>().Take(1).SingleOrDefault();
            return schemaVersion?.SchemaHash;
        }
        catch
        {
            // If we can't query the table, it doesn't exist yet
            return null;
        }
    }

    public static ISessionFactory CreateSessionFactory()
    {
        var sessionFactory = Fluently.Configure()
            .Database(
                MySQLConfiguration.Standard
                    .ConnectionString(Settings.ConnectionString)
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Page>())
            .BuildSessionFactory();

        return sessionFactory;
    }

    public static Configuration BuildTestConfiguration() => BuildTestConfiguration(Settings.ConnectionString);

    public static Configuration BuildTestConfiguration(string connectionString)
    {
        var configuration = Fluently.Configure()
            .Database(
                MySQLConfiguration.Standard
                    .ConnectionString(connectionString)
                    .Dialect<MySQL5FlexibleDialect>()
            )
            .Mappings(m => AddConventions(m).AddFromAssemblyOf<Question>())
            .ExposeConfiguration(SetConfig)
            .BuildConfiguration()
            .SetProperty("generate_statistics", "true");

        return configuration;
    }

    private static FluentMappingsContainer AddConventions(MappingConfiguration m)
    {
        var mappingsContainer = m.FluentMappings.Conventions.Add<EnumConvention>();

        if (MySQL5FlexibleDialect.IsInMemoryEngine())
            mappingsContainer.Conventions.Add<InMemoryEngine_StringPropertyConvention>();

        return mappingsContainer;
    }

    public static void SetConfig(Configuration? config)
    {
        _configuration = config ?? throw new ArgumentNullException(nameof(config));
    }

    public static void BuildSchema()
    {
        var schemaExport = new SchemaExport(_configuration);
        schemaExport.SetDelimiter(";");

        var sb = new StringBuilder();
        schemaExport.Create(sql => sb.AppendLine(sql), execute: false);

        // Remove all lines that start with "alter table" and split by ";"
        // to avoid errors when dropping not existing indices.
        var cleanedLines = sb
            .ToString()
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Where(l => !Regex.IsMatch(
                l,
                @"^\s*alter\s+table.+drop\s+foreign\s+key",   // **nur** FK-Drops
                RegexOptions.IgnoreCase | RegexOptions.Singleline));

        var sqlBatch =
            $"""
             SET foreign_key_checks = 0;
             {string.Join(";\n", cleanedLines)};
             SET foreign_key_checks = 1;
             """;

        var connectionString = Settings.ConnectionString;
        if (!connectionString.Contains("AllowBatch", StringComparison.OrdinalIgnoreCase))
            connectionString += ";AllowBatch=True";

        using var conn = new MySqlConnection(connectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sqlBatch, conn);
        cmd.CommandTimeout = 0;
        cmd.ExecuteNonQuery();
    }

    public static void TruncateAllTables()
    {
        const string getTables =
            """
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = DATABASE()
              AND table_type   = 'BASE TABLE';
            """;

        using var conn = new MySqlConnection(Settings.ConnectionString);
        conn.Open(); var tables = new List<string>();
        using (var cmd = new MySqlCommand(getTables, conn))
        using (var rdr = cmd.ExecuteReader())
            while (rdr.Read())
            {
                var tableName = rdr.GetString(0);
                if (!SchemaVersionTables.Contains(tableName))
                {
                    tables.Add($"`{tableName}`");
                }
            }

        if (tables.Count == 0)
            return;

        var sb = new StringBuilder("SET FOREIGN_KEY_CHECKS = 0;\n");
        foreach (var t in tables) sb.Append("TRUNCATE TABLE ").Append(t).Append(";\n");
        sb.Append("SET FOREIGN_KEY_CHECKS = 1;");

        using var truncateCmd = new MySqlCommand(sb.ToString(), conn) { CommandTimeout = 0 };
        truncateCmd.ExecuteNonQuery();
    }
}
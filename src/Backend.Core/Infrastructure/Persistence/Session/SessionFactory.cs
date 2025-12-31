using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Backend.Core.Infrastructure.Persistence.Session;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

public class SessionFactory
{
    private static Configuration _configuration = null!;
    private static readonly HashSet<string> _schemaVersionTables = ["SchemaVersion".ToLower()];
    private static string? _cachedSchemaHash;

    // Calculate a hash of the schema definition for detecting schema changes
    public static string CalculateSchemaHash()
    {
        if (_cachedSchemaHash != null)
            return _cachedSchemaHash;

        var schemaScript = new StringBuilder();
        var schemaExport = new SchemaExport(_configuration);

        schemaExport.SetDelimiter(".");
        schemaExport.Create(sql => schemaScript.AppendLine(sql), execute: false);

        // Use SHA256 to create a hash of the schema script
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(schemaScript.ToString()));

        // Convert to hexadecimal string representation and cache
        _cachedSchemaHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        return _cachedSchemaHash;
    }

    // Method to store or update the schema version
    public static void SaveSchemaVersion(string hash)
    {
        using var session = _configuration.BuildSessionFactory().OpenSession();
        using var transaction = session.BeginTransaction();

        var schemaVersion = session
            .QueryOver<SchemaVersion>()
            .Take(1).SingleOrDefault() ?? new SchemaVersion();

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
        var connectionString = ConnectionStringHelper.EnsureTimeoutSettings(Settings.ConnectionString);

        var databaseConfig = MySQLConfiguration.Standard
            .ConnectionString(connectionString)
            .QuerySubstitutions("true 1, false 0");

        // Only show SQL in non-test environments or when explicitly enabled
        if (!Settings.IsRunningTests || Settings.ShowSqlInTests)
        {
            databaseConfig.ShowSql();
        }

        var sessionFactory = Fluently.Configure()
            .Database(databaseConfig)
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Page>())
            .ExposeConfiguration(cfg =>
            {
                // Set global command timeout to 5 minutes (300 seconds)
                cfg.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, "300");
            })
            .BuildSessionFactory();

        return sessionFactory;
    }

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
            .BuildConfiguration();
            //.SetProperty("generate_statistics", "true");

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
        var schemaExport = new SchemaExport(_configuration)
            .SetDelimiter(";");

        var raw = new StringBuilder();
        schemaExport.Create(sql => raw.AppendLine(sql), execute: false);

        // 1. split, 2. trim, 3. drop FK-drops, 4. ignore blanks
        var statements = raw.ToString()
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Where(s => !Regex.IsMatch(
                s,
                @"^\s*alter\s+table.+drop\s+foreign\s+key",
                RegexOptions.IgnoreCase | RegexOptions.Singleline));

        // re-assemble without producing empty statements
        var sqlBatch = $"""
                        SET FOREIGN_KEY_CHECKS = 0;
                        {string.Join(";\n", statements)};
                        SET FOREIGN_KEY_CHECKS = 1;
                        """;

        // make sure the connection string allows multi-queries
        var connectionString = Settings.ConnectionString;
        if (!connectionString.Contains("AllowBatch", StringComparison.OrdinalIgnoreCase))
            connectionString += ";AllowBatch=true";

        using var conn = new MySqlConnection(connectionString);
        conn.Open();
        using var cmd = new MySqlCommand(sqlBatch, conn) { CommandTimeout = 0 };
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
        using (var dataReader = cmd.ExecuteReader())
            while (dataReader.Read())
            {
                var tableName = dataReader.GetString(0).ToLower();
                if (!_schemaVersionTables.Contains(tableName))
                    tables.Add($"`{tableName}`");
            }

        if (tables.Count == 0)
            return;

        var stringBuilder = new StringBuilder("SET FOREIGN_KEY_CHECKS = 0;\n");
        foreach (var table in tables)
            stringBuilder
                .Append("TRUNCATE TABLE ")
                .Append(table)
                .Append(";\n");

        stringBuilder.Append("SET FOREIGN_KEY_CHECKS = 1;");

        using var truncateCmd = new MySqlCommand(stringBuilder.ToString(), conn) { CommandTimeout = 0 };
        truncateCmd.ExecuteNonQuery();
    }
}
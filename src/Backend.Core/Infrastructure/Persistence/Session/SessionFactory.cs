using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

public class SessionFactory
{
    private static Configuration _configuration = null!;

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
        _configuration = config;
    }

    public static void BuildSchema()
    {
        var schemaExport = new SchemaExport(_configuration);
        schemaExport.SetDelimiter(";");

        var sb = new StringBuilder();
        schemaExport.Create(sql => sb.AppendLine(sql), execute: false);

        var sqlBatch = 
            $"""
            SET foreign_key_checks = 0;
            {sb}
            SET foreign_key_checks = 1;
            """;

        using var conn = new MySqlConnection(Settings.ConnectionString);
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
        conn.Open();

        var tables = new List<string>();
        using (var cmd = new MySqlCommand(getTables, conn))
        using (var rdr = cmd.ExecuteReader())
            while (rdr.Read())
                tables.Add($"`{rdr.GetString(0)}`");

        if (tables.Count == 0)
            return;

        var sb = new StringBuilder("SET FOREIGN_KEY_CHECKS = 0;\n");
        foreach (var t in tables) sb.Append("TRUNCATE TABLE ").Append(t).Append(";\n");
        sb.Append("SET FOREIGN_KEY_CHECKS = 1;");

        using var truncateCmd = new MySqlCommand(sb.ToString(), conn) { CommandTimeout = 0 };
        truncateCmd.ExecuteNonQuery();
    }
}
using MySql.Data.MySqlClient;
using NHibernate.Cfg;

internal class SchemaBuilder(Action<string> _perfLog)
{
    public async Task Init(string connectionString)
    {
        Settings.ConnectionString = connectionString;
        var configuration = SessionFactory.BuildTestConfiguration(connectionString);
        _perfLog("NHibernate bootstrap: Init");

        if (await SchemaNeedsRebuild())
        {
            SessionFactory.BuildSchema();

            // Save the new schema version after building
            var schemaHash = SessionFactory.CalculateSchemaHash();
            SessionFactory.SaveSchemaVersion(schemaHash);

            _perfLog("NHibernate bootstrap: Build schema");
        }
        else
        {
            SessionFactory.TruncateAllTables();
            _perfLog("NHibernate bootstrap: Truncate All Tables");
        }

        CreateAppVersionSetting(configuration);
    }

    private async Task<bool> SchemaNeedsRebuild()
    {
        bool needSchemaRebuild = !await SchemaExistsAsync();

        // Check for schema changes if schema exists
        if (!needSchemaRebuild)
        {
            // Calculate current schema definition hash
            var newSchemaHash = SessionFactory.CalculateSchemaHash();
            var currentSchemaHash = SessionFactory.GetCurrentSchemaHash();

            // If schema hash is different or missing, we need to rebuild
            if (currentSchemaHash == null || currentSchemaHash != newSchemaHash)
            {
                needSchemaRebuild = true;
                _perfLog("Schema changed - rebuilding");
            }
        }

        return needSchemaRebuild;
    }

    private static async Task<bool> SchemaExistsAsync()
    {
        await using var conn = new MySqlConnection(Settings.ConnectionString);
        await conn.OpenAsync();

        const string sql =
            """
               SELECT COUNT(*)
               FROM information_schema.tables
               WHERE table_schema = DATABASE()
                 AND table_name   = 'User'
            """;

        await using var cmd = new MySqlCommand(sql, conn);
        var count = (long)(await cmd.ExecuteScalarAsync())!;
        return count > 0;
    }

    private void CreateAppVersionSetting(Configuration configuration)
    {
        using var session = configuration.BuildSessionFactory().OpenSession();
        var repositoryDb = new RepositoryDb<DbSettings>(session);
        repositoryDb.Create(new DbSettings
        {
            Id = 1,
            AppVersion = int.MaxValue,
        });

        _perfLog("Seed initial DB data");
    }
}
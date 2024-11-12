using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using TrueOrFalse.Infrastructure.Persistence;


namespace TrueOrFalse
{
    public class SessionFactory
    {
        private static Configuration _configuration;

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

        public static Configuration BuildTestConfiguration()
        {
            var configuration = Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard
                        .ConnectionString(Settings.ConnectionString)
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

        public static void SetConfig(Configuration config)
        {
            _configuration = config;
        }

        public static void BuildSchema()
        {
            new SchemaExport(_configuration)
                .Create(useStdOut: false, execute: true);
        }

        public static void TruncateAllTables()
        {
            const string disableForeignKeyCheck = "SET FOREIGN_KEY_CHECKS = 0;";
            const string enableForeignKeyCheck = "SET FOREIGN_KEY_CHECKS = 1;";

            const string sqlString = @"
                SELECT Concat('TRUNCATE TABLE ', TABLE_NAME, ';') 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE table_schema = DATABASE();

                SET FOREIGN_KEY_CHECKS = 1;
";

            using (var session = _configuration.BuildSessionFactory().OpenSession())
            {
                var statements = session.CreateSQLQuery(sqlString).List<string>();
                session.CreateSQLQuery(disableForeignKeyCheck).ExecuteUpdate();

                foreach (var statement in statements)
                    session.CreateSQLQuery(statement).ExecuteUpdate();

                session.CreateSQLQuery(enableForeignKeyCheck).ExecuteUpdate();
            }
        }
    }
}

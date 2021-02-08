using System;
using System.IO;
using System.Reflection;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Seedworks.Web.State;
using TrueOrFalse.Infrastructure.Persistence;
using Environment = NHibernate.Cfg.Environment;

namespace TrueOrFalse
{
    public class SessionFactory
    {
        private static Configuration _configuration;
        
        public static ISessionFactory CreateSessionFactory()
        {
            var configuration = ReadConfigurationFromCacheOrBuildIt();
            _configuration = configuration;
            _configuration.SetProperty(Environment.Hbm2ddlKeyWords, "none");

            return configuration.BuildSessionFactory();
        }

        private static Configuration ReadConfigurationFromCacheOrBuildIt()
        {
            Configuration nhConfigurationCache;

            var assembly = Assembly.GetAssembly(typeof (Question));  
            if(assembly == null && !ContextUtil.IsWebContext)
                assembly = Assembly.LoadFile(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assembly.GetName().Name + ".dll"));

            var nhCfgCache = new NHConfigurationFileCache(assembly);
            var cachedCfg = nhCfgCache.LoadConfigurationFromFile();

            if (cachedCfg == null)
            {
                nhConfigurationCache = BuildConfiguration();
                nhCfgCache.SaveConfigurationToFile(nhConfigurationCache);
            }
            else
            {
                nhConfigurationCache = cachedCfg;
            }
            return nhConfigurationCache;            
        }

        private static Configuration BuildConfiguration()
        {
            var configuration = Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard
                        .ConnectionString(Settings.ConnectionString())
                        .Dialect<MySQL5FlexibleDialect>()
                )
                .Mappings(m => AddConventions(m).AddFromAssemblyOf<Question>())
                .ExposeConfiguration(SetConfig)
                .BuildConfiguration();

            if (!ContextUtil.IsWebContext || Settings.WithNHibernateStatistics)
                configuration = configuration.SetProperty("generate_statistics", "true");

            return configuration;
        }

        private static FluentMappingsContainer AddConventions(MappingConfiguration m)
        {
            var mappingsContainer = m.FluentMappings.Conventions.Add<EnumConvention>();

            if (MySQL5FlexibleDialect.IsInMemoryEngine())
                mappingsContainer.Conventions.Add<InMemoryEngine_StringPropertyConvention>();

            return mappingsContainer;
        }

        private static void SetConfig(Configuration config)
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
            const string sqlString =
                @"SELECT Concat('TRUNCATE TABLE ', TABLE_NAME, ';') 
                  FROM INFORMATION_SCHEMA.TABLES where  table_schema in (DATABASE())";

            using (var session = _configuration.BuildSessionFactory().OpenSession())
            {
                var statements = session.CreateSQLQuery(sqlString).List<string>();

                foreach (var statement in statements)
                    session.CreateSQLQuery(statement).ExecuteUpdate();
            }
        }
    }
}

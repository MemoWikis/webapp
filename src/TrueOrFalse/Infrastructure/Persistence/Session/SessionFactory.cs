﻿using System;
using System.IO;
using System.Reflection;
using System.Web;
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
            var configuration = ReadConfigurationFromCacheOrBuildIt();
            _configuration = configuration;
            return configuration.BuildSessionFactory();
        }

        private static Configuration ReadConfigurationFromCacheOrBuildIt()
        {
            Configuration nhConfigurationCache;

            var assembly = Assembly.GetAssembly(typeof (Question));  
            if(HttpContext.Current == null)
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
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard
                        .ConnectionString(Settings.ConnectionString())
                        .Dialect<MySQL5FlexibleDialect>
                )
                .Mappings(m => AddConventions(m).AddFromAssemblyOf<Question>())
                .ExposeConfiguration(SetConfig)
                .BuildConfiguration();
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
            DropAllTables();
            new SchemaExport(_configuration)
                .Create(useStdOut: false, execute: true);
        }

        private static void DropAllTables()
        {
            const string sqlString = 
                @"select name into #tables from sys.objects where type = 'U'
                  while (select count(1) from #tables) > 0
                  begin
                  declare @sql varchar(max)
                  declare @tbl varchar(255)
                  select top 1 @tbl = name from #tables
                  set @sql = 'drop table ' + @tbl
                  exec(@sql)
                  delete from #tables where name = @tbl
                  end
                  drop table #tables;";

            using (var session = _configuration.BuildSessionFactory().OpenSession())
            {
                session.CreateSQLQuery(sqlString);
            }
        }
    }
}

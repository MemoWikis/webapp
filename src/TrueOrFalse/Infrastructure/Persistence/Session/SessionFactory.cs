using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse
{
    public class SessionFactory
    {
        private static Configuration _configuration;
        
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(
                MySQLConfiguration.Standard
                  .ConnectionString(GetConnectionString.Run())
              )
              .Mappings(m =>
                m.FluentMappings.Conventions.Add<EnumConvention>()
                    .AddFromAssemblyOf<Question>())
              .ExposeConfiguration(SetConfig)
              .BuildSessionFactory();
        }

        private static void SetConfig(Configuration config)
        {
            _configuration = config;
        }

        public static void BuildSchema()
        {
            DropAllTables();
            new SchemaExport(_configuration)
                .Create(script: false, export: true);
        }

        private static void DropAllTables()
        {
            var sqlString = @"select name into #tables from sys.objects where type = 'U'
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

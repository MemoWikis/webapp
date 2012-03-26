using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Core.Infrastructure.Persistence;

namespace TrueOrFalse.Core
{
    public class SessionFactory
    {
        private static Configuration _configuration;
        
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(
                MsSqlConfiguration.MsSql2008
                  .ConnectionString(GetConnectionString.Run())
              )
              .Mappings(m =>
                m.FluentMappings.Conventions.Add<EnumConvention>().AddFromAssemblyOf<Question>())
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
            DropAllTables();//twice, because FK reference tableds wont't be delete on the first run.
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

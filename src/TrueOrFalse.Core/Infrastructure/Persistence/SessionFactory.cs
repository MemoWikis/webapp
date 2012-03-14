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
    }
}

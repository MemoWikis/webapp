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

namespace TrueOrFalse.Core
{
    public class SessionFactory
    {
        


        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(
                MsSqlConfiguration.MsSql2008
                  .ConnectionString(c => c
                    .FromConnectionStringWithKey("main"))
              )
              .Mappings(m =>
                m.FluentMappings.AddFromAssemblyOf<Question>())
              .ExposeConfiguration(BuildSchema)
              .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            new SchemaExport(config)
              .Create(false, true);
        }
    }
}

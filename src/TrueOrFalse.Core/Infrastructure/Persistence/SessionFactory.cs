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
        private static readonly string _fileDbName;

        static SessionFactory()
        {
            if (HttpContext.Current != null)
                _fileDbName = HttpContext.Current.Server.MapPath("trueOrFalse.db");
            else
                _fileDbName = "trueOrFalse.db";
        }

        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(
                SQLiteConfiguration.Standard
                  .UsingFile(_fileDbName)
              )
              .Mappings(m =>
                m.FluentMappings.AddFromAssemblyOf<Question>())
              .ExposeConfiguration(BuildSchema)
              .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            DeleteDbOnEachRun();

            new SchemaExport(config)
              .Create(false, true);
        }

        private static void DeleteDbOnEachRun()
        {
            if (File.Exists(_fileDbName))
                File.Delete(_fileDbName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace TrueOrFalse.Core
{
    public class SessionFactory
    {
        private static string _fileDbName;

        public static ISessionFactory CreateSessionFactory()
        {
            _fileDbName = "firstProject.db";
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
            // delete the existing db on each run
            if (File.Exists(_fileDbName))
                File.Delete(_fileDbName);

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
              .Create(false, true);
        }
    }
}

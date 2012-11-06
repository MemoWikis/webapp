using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Infrastructure
{
    public class DbSettingsMap : ClassMap<DbSettings>
    {
        public DbSettingsMap()
        {
            Table("Setting");
            Id(x => x.Id);
            Map(x => x.AppVersion);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}

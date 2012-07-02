using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class PersistentLoginMap : ClassMap<PersistentLogin>
    {
        public PersistentLoginMap()
        {
            Id(x => x.Id);
            Map(x => x.UserId);
            Map(x => x.LoginGuid);
            Map(x => x.Created);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NHibernate.Mapping;

namespace TrueOrFalse
{
    public class SetValuationMap : ClassMap<SetValuation>
    {
        public SetValuationMap()
        {
            Id(x => x.Id);
            Map(x => x.UserId);
            Map(x => x.SetId);

            Map(x => x.RelevancePersonal);

            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}

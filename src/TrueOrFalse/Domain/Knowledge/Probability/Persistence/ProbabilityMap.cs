using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

public class ProbabilityMap : ClassMap<Probability>
{
    public ProbabilityMap()
    {
        Id(x => x.Id);

        Map(x => x.Percentage).Precision(9).Scale(8);
        
        References(x => x.Question);
        References(x => x.User);

        Map(x => x.DateTimeCalculated);
    }
}
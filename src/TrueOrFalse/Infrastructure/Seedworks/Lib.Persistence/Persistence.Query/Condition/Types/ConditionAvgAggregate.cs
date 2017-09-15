using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionAvgAggregate : ConditionDouble
    {
        public ConditionAvgAggregate(ConditionContainer conditions, string propertyName)
            : base(conditions, propertyName)
        {
        }

        public override void AddToCriteria(NHibernate.ICriteria criteria)
        {
            if (IsGreaterThan())
                criteria.Add(Restrictions.Gt(_Projections.EmptyGroupDoubleAvg(PropertyName), GetValue()));
            else
                criteria.Add(Restrictions.Lt(_Projections.EmptyGroupDoubleAvg(PropertyName), GetValue()));
        }
    }
}

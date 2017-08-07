using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionDisjunction<T> : ConditionList<T>
    {
        public ConditionDisjunction(ConditionContainer conditions, string propertyName)
            : base(conditions, propertyName)
        {
        }

        public override ICriterion GetCriterion(T item)
        {
            return Restrictions.Eq(PropertyName, item);
        }

        protected override Junction GetInitializedJunction()
        {
          return new Disjunction();
        }
    }
}

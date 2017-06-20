using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionEnum : Condition
	{
		public Enum Value { get; private set; }

		public ConditionEnum(ConditionContainer conditions, string propertyName)
			: base(conditions, propertyName)
		{
		}

		public void EqualTo(Enum value)
		{
			Value = value;
			AddUniqueToContainer();
		}

		public override void AddToCriteria(ICriteria criteria)
		{
			criteria.Add(GetCriterion());
		}

		public override ICriterion GetCriterion()
		{
			return Restrictions.Eq(PropertyName, Value);
		}

		public override void Reset()
		{
			Value = null;
			base.Reset();
		}
	}
}

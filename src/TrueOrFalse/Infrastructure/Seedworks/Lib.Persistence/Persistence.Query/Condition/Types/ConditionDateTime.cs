using System;
using NHibernate;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionDateTime : Condition
    {
        public ConditionDateTime(ConditionContainer conditions) : base(conditions)
        {
        }

        public ConditionDateTime(ConditionContainer conditions, string propertyName) : base(conditions, propertyName)
        {
        }

        private DateTime? _before;
        private DateTime? _after;
    	private bool _mayBeNull;

        public override void AddToCriteria(ICriteria criteria)
        {
            var criterion = GetCriterion();
            if (criterion != null) criteria.Add(criterion);
        }

        public override ICriterion GetCriterion()
        {
            if (_after != null && _before != null)
                return Restrictions.Between(PropertyName, _after.Value, _before.Value);

			if (_after == null && _before == null)
				return null;

        	SimpleExpression criterion;
            if (_after != null)
                criterion = Restrictions.Ge(PropertyName, _after.Value);
			else
                criterion = Restrictions.Le(PropertyName, _before.Value);

			if (!_mayBeNull)
				return criterion;

			var disjunction = new Disjunction();
			disjunction.Add(criterion);
			disjunction.Add(Restrictions.IsNull(PropertyName));
        	return disjunction;
        }

        public void EqualOrAfter(DateTime time)
        {
			_mayBeNull = false;
			_after = time;
            _before = null;

            Conditions.AddUnique(this);
        }

		public void NullOrEqualOrAfter(DateTime time)
    	{
			_mayBeNull = true;
			_after = time;
			_before = null;

			Conditions.AddUnique(this);
		}

    	public void EqualOrBefore(DateTime time)
        {
			_mayBeNull = false;
			_after = null;
            _before = time;

            Conditions.AddUnique(this);
        }

		public void NullOrEqualOrBefore(DateTime time)
    	{
			_mayBeNull = true;
			_after = null;
			_before = time;

			Conditions.AddUnique(this);
		}

    	public void Between(DateTime time1, DateTime time2)
        {
			_mayBeNull = false;
			if (time1 < time2)
            {
                _after = time1;
                _before = time2;
            }
            else
            {
                _after = time2;
                _before = time1;
            }

            Conditions.AddUnique(this);
        }

        public void Year(int year)
        {
			_mayBeNull = false;
			_after = new DateTime(year, 1, 1);
            _before = new DateTime(year + 1, 1, 1);

            Conditions.AddUnique(this);
        }

        public override void Reset()
        {
			_mayBeNull = false;
			_after = null;
            _before = null;
            base.Reset();
        }
    }
}
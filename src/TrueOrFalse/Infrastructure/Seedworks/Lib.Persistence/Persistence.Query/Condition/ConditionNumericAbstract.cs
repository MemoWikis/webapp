using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
    [Serializable]
    public abstract class ConditionNumericAbstract : Condition, IConditionNumeric
    {
        private ConditionComparisonType _queryType;

        /// <summary>
        /// Unset this value if for a less than comparison you do not automatically want to constrain
        /// that the value be greater than or equal to zero.
        /// </summary>
        public bool MustBeGreaterThanOrEqualToZero = true;

        protected ConditionNumericAbstract(ConditionContainer conditions, string propertyName)
            : base(conditions)
        {
            PropertyName = propertyName;
        }

        protected ConditionNumericAbstract(ConditionContainer conditions) : base(conditions){}

        public virtual object GetValue() { throw new NotImplementedException(); }

        /// <summary>
        /// Return true if this condition is set and contained in the ConditionList.
        /// </summary>
        /// <returns>True if this condition is contained in the ConditionList AND if 
        /// its value is set to something other than the default, else false.</returns>
        public override bool IsActive()
        {
            return IsSet() && Conditions.Contains(this);
        }

        /// <summary>
        /// Returns true if this condition's value is set to anything else than the initial value; else false.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsSet();

        public bool IsGreaterThan()
        {
            return _queryType == ConditionComparisonType.Greater;
        }

        public bool IsEqualTo()
        {
            return _queryType == ConditionComparisonType.Equal;
        }

        public bool IsNotEqualTo()
        {
            return _queryType == ConditionComparisonType.NotEqual;
        }

        public bool IsLessThanOrEqual()
        {
            return _queryType == ConditionComparisonType.LessOrEqual;
        }

        protected void SetQueryGreater()
        {
            _queryType = ConditionComparisonType.Greater;
        }

        protected void SetQueryLess()
        {
            _queryType = ConditionComparisonType.Less;
        }

        protected void SetQueryLessOrEqual()
        {
            _queryType = ConditionComparisonType.LessOrEqual;
        }

        protected void SetQueryEqual()
        {
            _queryType = ConditionComparisonType.Equal;
        }

        protected void SetQueryNotEqual()
        {
            _queryType = ConditionComparisonType.NotEqual;
        }

        protected bool RemoveIfMinusOne(object value)
        {
            if (Convert.ToInt32(value) == -1)
            {
                Conditions.Remove(this);
                return true;
            }
            return false;
        }

        public override void AddToCriteria(ICriteria criteria)
        {
            criteria.Add(GetCriterion());
        }

        public override ICriterion GetCriterion()
        {
            if (IsEqualTo())
                return Restrictions.Eq(PropertyName, GetValue());

            if (IsNotEqualTo())
                return !Restrictions.Eq(PropertyName, GetValue());
            
            if (IsGreaterThan())
                return Restrictions.Gt(PropertyName, GetValue());
            
            // Building conjunction if needed.
            ICriterion restriction;

            if (IsLessThanOrEqual())
                restriction = Restrictions.Le(PropertyName, GetValue());
            else // less than
                restriction = Restrictions.Lt(PropertyName, GetValue());

            if (MustBeGreaterThanOrEqualToZero)
                restriction = Restrictions.Conjunction()
                    .Add(restriction)
                    .Add(Restrictions.Ge(PropertyName, 0f));

            return restriction;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionBoolean : Condition
    {
        private bool _value = false;

        public ConditionBoolean(ConditionContainer conditions, string propertyName)
            : base(conditions)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Sets the condition to [true AND active] or [false AND inactive].
        /// </summary>
        /// <param name="value"></param>
        public void SetTrueOrInactive(bool value)
        {
            if (value)
                SetTrue();
            else
            {
                _value = false;
                Conditions.Remove(this);
            }
                
        }
        
        public bool IsTrue()
        {
            return _value;
        }

        /// <summary>
        /// Sets the condition to true
        /// </summary>
        public void SetTrue()
        {
            _value = true;
            Conditions.AddUnique(this);
        }

		public bool IsFalse()
		{
			return !_value;
		}

		/// <summary>
        /// Sets the condition to false (and leaves it ACTIVE!).
        /// </summary>
        public void SetFalse()
        {
            _value = false;
            Conditions.AddUnique(this);
        }

        public bool GetValue()
        {
            return _value;
        }

        /// <summary>
        /// Checks whether this condition is set and contained in the ConditionList.
        /// </summary>
        /// <returns>True if this condition is contained in the ConditionList AND if 
        /// its value is set to something other than the default, else false.</returns>
        public override bool IsActive()
        {
            return IsTrue() && Conditions.Contains(this);
        }

        public override void AddToCriteria(ICriteria criteria)
        {
            criteria.Add(GetCriterion());
        }

        public override ICriterion GetCriterion()
        {
            return Restrictions.Eq(PropertyName, GetValue());
        }

        public override void Reset()
        {
            SetTrueOrInactive(false);
        }
    }
}

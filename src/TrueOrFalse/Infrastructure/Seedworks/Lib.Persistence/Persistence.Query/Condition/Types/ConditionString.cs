using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionString : Condition
    {
        private string _value;
        private bool _isNotNullOrEmpty = false;
        private bool _isLike = false;
        private readonly List<string> _likeValuesCaseIns;

        /// <summary>
        /// The search criteria value of this condition
        /// </summary>
        public string Value { get { return _value; } }
        public bool IsNotNullOrEmpty { get { return _isNotNullOrEmpty; } }
        public bool IsLike { get { return _isLike; } }

        public ConditionString(ConditionContainer conditions, string propertyName)
            : base(conditions)
        {
            PropertyName = propertyName;
            _likeValuesCaseIns = new List<string>();
        }

        /// <summary>
        /// Sets the filter to given value
        /// </summary>
        /// <param name="value"></param>
        public void EqualTo(string value)
        {
            value = value.Trim();

            if (!RemoveIfInvalid(value))
                return;

            ResetAndAddUnique();

            _value = value;
        }

        public void Like(string value)
        {
            value = value.Trim();

            if (!RemoveIfInvalid(value))
                return;

            ResetAndAddUnique();

            _isLike = true;
            _value = value;
        }

        public ConditionString OrLikeCaseIns(string value)
        {
            value = value.Trim();

            if (!RemoveIfInvalid(value))
                return this;

            ResetAndAddUnique();

            if (IsValid(_value))
                _likeValuesCaseIns.Add(_value);

            _value = null;

            _likeValuesCaseIns.Add(value);

            _isLike = true;

            return this;
        }

        private static bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        private bool RemoveIfInvalid(string value)
        {
            if (!IsValid(value))
            {
                Remove();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the filter to empty or null
        /// </summary>
        /// <returns></returns>
        public void NullOrEmpty()
        {
            ResetAndAddUnique();

            _value = "";
        }

        public void NotNullOrEmpty()
        {
            ResetAndAddUnique();

            _isNotNullOrEmpty = true;
        }

        /// <summary>
        /// Sets the condition to find empty/ not defined values
        /// </summary>
        public bool IsNullOrEmpty()
        {
            return _value == "" && !IsNotNullOrEmpty;
        }

        public bool IsActiveFilter()
        {
            return _value != null && Conditions.Contains(this);
        }

        private void ResetAndAddUnique()
        {
            _isNotNullOrEmpty = false;
            _isLike = false;
            Conditions.AddUnique(this);
        }

        public string GetString()
        {
            return _value;
        }

        public override ICriterion GetCriterion()
        {
            if (IsNullOrEmpty())
            {
                var disjunction = Restrictions.Disjunction();
                disjunction.Add(Restrictions.Eq(PropertyName, ""));
                disjunction.Add(Restrictions.IsNull(PropertyName));
                return disjunction;
            }

            if (IsNotNullOrEmpty)
            {
                return Restrictions.And(
                    Restrictions.Not(Restrictions.Eq(PropertyName, "")),
                    Restrictions.IsNotNull(PropertyName)
                );
            }

            if (IsLike)
            {
                if (_likeValuesCaseIns.Count <= 0)
                    return GetLikeRestriction(_value, false);

                var disjunction = Restrictions.Disjunction();
                for (int i = 0; i < _likeValuesCaseIns.Count; i++)
                    disjunction.Add(GetLikeRestriction(_likeValuesCaseIns[i], true));

                return disjunction;
            }

            return Restrictions.Eq(PropertyName, Value);
        }

        private ICriterion GetLikeRestriction(string value, bool caseInsensitive)
        {
            if (caseInsensitive)
                return Restrictions.InsensitiveLike(PropertyName, value, MatchMode.Anywhere);

            return Restrictions.Like(PropertyName, value, MatchMode.Anywhere);
        }

        public override void AddToCriteria(ICriteria criteria)
        {
            criteria.Add(GetCriterion());
        }

        public void AddToDisjunction(Disjunction disjunction)
        {
            disjunction.Add(GetCriterion());
        }

        /// <summary>
        /// Setzt alle Werte des Filters zurück.
        /// </summary>
        public override void Reset()
        {
            _value = null;
            _isNotNullOrEmpty = false;
            _isLike = false;
            _likeValuesCaseIns.Clear();
            base.Reset();
        }
    }
}

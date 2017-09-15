using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionDouble : ConditionNumericAbstract, IConditionNumeric
    {
        private const double _noValue = -1;
        private double _value = _noValue;

        public ConditionDouble(ConditionContainer conditions, string propertyName)
            : base(conditions, propertyName)
        {
            PropertyName = propertyName;
        }

        public void GreaterThan(object value)
        {
            GreaterThan(Convert.ToDouble(value));
        }

        public void GreaterThan(double value)
        {
            SetQueryGreater();
            _value = value;

            if (_value == _noValue)
            {
                Conditions.Remove(this);
                return;
            }

            Conditions.AddUnique(this);
        }

        public void GreaterThan(bool isChecked, double value)
        {
            if (isChecked)
                GreaterThan(value);
            else
                Conditions.Remove(this);
        }

        public void LessThan(bool isChecked, double value)
        {
            if (isChecked)
                LessThan(value);
            else
                Conditions.Remove(this);
        }

        public void LessThan(object value)
        {
            LessThan(Convert.ToDouble(value));
        }

        public void LessThan(double value)
        {
            _value = value;
            SetQueryLess();

            if (_value == _noValue)
            {
                Conditions.Remove(this);
                return;
            }

            Conditions.AddUnique(this);
        }

        public void EqualTo(double value)
        {
            SetQueryEqual();
            _value = value;
        }

        public override object GetValue()
        {
            return _value;
        }

        public override bool IsSet()
        {
            return _value != _noValue;
        }

        public override void Reset()
        {
            _value = _noValue;
            base.Reset();
        }
    }
}

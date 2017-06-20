using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionDecimal : ConditionNumericAbstract, IConditionNumeric
    {
        private const decimal _noValue = decimal.MinusOne;
        private decimal _value = _noValue;

        public ConditionDecimal(ConditionContainer conditions, string propertyName)
            : base(conditions)
        {
            PropertyName = propertyName;
        }

        public void GreaterThan(object value)
        {
            GreaterThan(Convert.ToDecimal(value, CultureInfo.InvariantCulture));
        }

        public void GreaterThan(decimal value)
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

        public void GreaterThan(bool isChecked, decimal value)
        {
            if (isChecked)
                GreaterThan(value);
            else
                Conditions.Remove(this);
        }

        public void LessThan(bool isChecked, decimal value)
        {
            if (isChecked)
                LessThan(value);
            else
                Conditions.Remove(this);
        }

        public void LessThan(object value)
        {
            LessThan(Convert.ToDecimal(value, CultureInfo.InvariantCulture));
        }

        public void LessThan(decimal value)
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

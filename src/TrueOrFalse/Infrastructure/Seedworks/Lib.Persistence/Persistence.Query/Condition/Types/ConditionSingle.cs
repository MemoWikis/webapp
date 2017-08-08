using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionSingle : ConditionNumericAbstract, IConditionNumeric
    {
        private const Single _noValue = -1;
        private Single _value = _noValue;
        private readonly Single _criticalValue = _noValue;

        public ConditionSingle(ConditionContainer conditions, string propertyName)
            : base(conditions)
        {
            PropertyName = propertyName;
        }

        public ConditionSingle(ConditionContainer conditions, string propertyName, Single criticalValue)
            : this(conditions, propertyName)
        {
            _criticalValue = criticalValue;
        }

        public void LessThanCritical(bool activate)
        {
            if (_criticalValue == _noValue)
                throw new ArgumentOutOfRangeException("Critical value has not been set! Use different constructor.");

            LessThanOrEqual(activate, _criticalValue);
        }

        public void GreaterThan(object value)
        {
            GreaterThan(Convert.ToSingle(value));
        }

        public void GreaterThan(Single value)
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

        public void GreaterThan(bool isChecked, Single value)
        {
            if (isChecked)
                GreaterThan(value);
            else
                Conditions.Remove(this);
        }

        public void LessThanOrEqual(bool isChecked, Single value)
        {
            if (isChecked)
                LessThanOrEqual(value);
            else
                Conditions.Remove(this);
        }

        public void LessThanOrEqual(object value)
        {
            LessThanOrEqual(Convert.ToSingle(value));
        }

        public void LessThanOrEqual(Single value)
        {
            SetQueryLessOrEqual();
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

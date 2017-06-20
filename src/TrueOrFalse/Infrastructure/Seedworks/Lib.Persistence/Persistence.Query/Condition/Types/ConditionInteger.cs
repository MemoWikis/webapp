using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionInteger : ConditionNumericAbstract
    {
        private const int _noValue = -1;
        private int _value = _noValue;

        public ConditionInteger(ConditionContainer conditions, string propertyName)
            : base(conditions)
        {
            PropertyName = propertyName;
        }

        public void GreaterThan(object value)
        {
            GreaterThan(Convert.ToInt32(value));
        }

        public void GreaterThan(int value)
        {
            SetQueryGreater();
            _value = value;

            if (RemoveIfMinusOne(value))
                return;

            Conditions.AddUnique(this);
        }

        public void GreaterThan(bool isChecked, int value)
        {
            if (isChecked)
                GreaterThan(value);
            else
                Conditions.Remove(this);
        }

        public void LessThan(bool isChecked, int value)
        {
            if (isChecked)
                LessThan(value);
            else
                Conditions.Remove(this);
        }

        public void LessThan(object value)
        {
            LessThan(Convert.ToInt32(value));
        }

        public void LessThan(int value)
        {
            SetQueryLess();
            _value = value;
            
            if(RemoveIfMinusOne(value))
                return;

            Conditions.AddUnique(this);
        }

        public void EqualTo(object id)
        {
            if(String.IsNullOrEmpty(id.ToString()))
            {
                Remove();
                return;
            }

            EqualTo(Convert.ToInt32(id));
        }

        public void EqualTo(int id)
        {
            SetQueryEqual();
            _value = id;

            Conditions.AddUnique(this);
        }

        public void IsNotEqualTo(int execptForId)
        {
            _value = execptForId;
            SetQueryNotEqual();

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

        public string GetString()
        {
            return _value.ToString();
        }

        public override void Reset()
        {
            _value = _noValue;
            base.Reset();
        }
    }
}

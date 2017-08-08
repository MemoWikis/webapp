using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Seedworks.Lib.Persistence
{
    [Serializable]
    public abstract class Condition
    {
        public string PropertyName { get; set; }
        private readonly ConditionContainer _conditions;
        public ConditionContainer Conditions
        {
            get { return _conditions; }
        }

        
        public Condition(ConditionContainer conditions)
        {
            _conditions = conditions;
        }

        public Condition(ConditionContainer conditions, string propertyName)
        {
            _conditions = conditions;
            PropertyName = propertyName;
        }
        
        /// <summary>
        /// Entfernt diese <see cref="Condition"/> aus der Liste.
        /// </summary>
		public void Remove()
        {
            if (_conditions.Contains(PropertyName))
            {
				_conditions.Remove(this);
				Reset();
            }
        }

        public override int GetHashCode()
        {
            return PropertyName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            Condition condition = (Condition)obj;

            if (!PropertyName.Equals(condition.PropertyName)) return false;

            return true;
        }

        private void AddUniqueToContainer(Condition condition)
        {
            Conditions.AddUnique(condition);
        }

        public void AddUniqueToContainer()
        {
            AddUniqueToContainer(this);
        }

        public abstract void AddToCriteria(ICriteria criteria);
        public abstract ICriterion GetCriterion();

        public virtual void Reset()
        {
            Conditions.Remove(this);
        }

        public virtual bool IsActive()
        {
            return _conditions.Contains(this);
        }

        public static Condition ForType(Type type, ConditionContainer conditions, string propertyName)
        {
            if (type == typeof(bool))
                return new ConditionBoolean(conditions, propertyName);

            if (type == typeof(float))
                return new ConditionSingle(conditions, propertyName);

            if (type == typeof(int))
                return new ConditionInteger(conditions, propertyName);

            throw new ArgumentException(string.Format("There is no condition for type {0}.", type), "type");
        }

        public static List<Type> SupportedTypes()
        {
            return new List<Type> {typeof(bool), typeof(string), typeof(int)};
        }
    }
}

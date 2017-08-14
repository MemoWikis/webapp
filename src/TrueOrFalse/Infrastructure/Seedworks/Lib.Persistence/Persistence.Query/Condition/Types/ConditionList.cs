using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
    [Serializable]
    public abstract class ConditionList<T> : Condition 
    {
        public int ItemCount { get { return _items.Count; } }
        
        private readonly List<T> _items = new List<T>();
        public List<T>  Items { get { return _items; } }

        public ConditionList(ConditionContainer conditions, string propertyName) : base(conditions, propertyName)
        {
            if (Conditions != null) 
				Conditions.Add(this);
		}

		public void Set(List<T> items)
		{
			Clear();
			Add(items);
			if (!Conditions.Contains(this))
				Conditions.Add(this);
		}

        public void Add(params T[] values)
        {
            foreach (var value in values)
                Add(value);
        }

        public void Add(List<T> values)
        {
            foreach (var value in values)
                Add(value);
        }

        public void Add(T value)
        {
            if (typeof(T) != typeof(Int32) && 
                typeof(T) != typeof(String) &&
                typeof(T) != typeof(bool) &&
                !typeof(T).IsEnum)
                throw new TypeMismatchException("expected int, string or enum");

            if (!_items.Contains(value))
                _items.Add(value);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public override void AddToCriteria(ICriteria criteria)
        {
            var criterion = GetCriterion();
            if (criterion != null) criteria.Add(criterion);
        }

        public override ICriterion GetCriterion()
        {
            if (Items.Count == 0)
                return null;
            var junction = GetInitializedJunction();
            foreach (var item in _items)
                junction.Add(GetCriterion(item));
            return junction;
        }

        public abstract ICriterion GetCriterion(T item);
        protected abstract Junction GetInitializedJunction();

		public override void Reset()
        {
            Items.Clear();
            Conditions.Remove(this);
        }
    }

}
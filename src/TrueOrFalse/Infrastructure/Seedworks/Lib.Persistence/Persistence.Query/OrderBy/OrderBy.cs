using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class OrderByExtender
	{
		private readonly OrderByCriteria _andOrderByCriteria;

		public T AndOrderBy<T>() where T:OrderByCriteria
		{
			_andOrderByCriteria.BeginAdding();
			return (T)_andOrderByCriteria;
		}

//		public OrderByCriteria AndOrderBy
//		{
//			get
//			{
//				_andOrderBy.BeginAdding();
//				return _andOrderBy;
//			}
//			set { _andOrderBy = value; }
//		}

		public OrderByExtender(OrderByCriteria orderByCriteria)
		{
			_andOrderByCriteria = orderByCriteria;
		}
	}

    [Serializable]
    public class OrderByExtenderT<T> where T : OrderByCriteria
	{
		private T _andOrderBy;

		public T AndOrderBy
		{
			get
			{
				_andOrderBy.BeginAdding();
				return _andOrderBy;
			}
			set { _andOrderBy = value; }
		}

		public OrderByExtenderT(OrderByCriteria orderBy)
		{
			AndOrderBy = (T)orderBy;
		}
	}

    [Serializable]
    public class OrderBy
    {
        private readonly OrderByCriteria _criteria;
		private readonly OrderByExtender _andOrderBy;
		private OrderDirection _direction = OrderDirection.Ascending;

		/// <summary>The table alias used in associations.</summary>
		public string Alias { get; private set; }
		public bool HasAlias { get { return !string.IsNullOrEmpty(Alias); } }

		/// <summary>
		/// An action to perform before adding the OrderBy to the Criteria 
		/// (e.g. CreateAlias) with this instance's <see cref="Alias"/>.
		/// </summary>
		public Action<ICriteria> CriteriaAction { get; private set; }
		public bool HasCriteriaAction { get { return CriteriaAction != null; } }

		public string PropertyName { get; private set; }
        public OrderDirection Direction { get{ return _direction; } }


        public OrderBy(string propertyName, OrderByCriteria criteria)
        {
            _criteria = criteria;
            PropertyName = propertyName;
			_andOrderBy = new OrderByExtender(criteria);
        }

		public OrderBy(string propertyName, OrderByCriteria criteria, string alias, Action<ICriteria> criteriaAction)
			: this(propertyName, criteria)
		{
			Alias = alias;
			CriteriaAction = criteriaAction;
		}

    	public OrderByExtender Asc()
        {
            return Set(OrderDirection.Ascending);
        }

        public OrderByExtender Desc()
        {
            return Set(OrderDirection.Descending);
        }

		public OrderByExtender Set(OrderDirection direction)
		{
			_direction = direction;

			if (_criteria.IsAdding)
				_criteria.Add(this);
			else
				_criteria.Current = this;
			_criteria.EndAdding();

			return _andOrderBy;
		}

    	public void Toggle()
        {
            if (_direction == OrderDirection.Descending)
                _direction = OrderDirection.Ascending;
            else
                _direction = OrderDirection.Descending;

            _criteria.Current = this;
        }

        public void AscOrToggle()
        {
            if (!IsCurrent())
                Asc();
            else
                Toggle();
        }

        public bool IsDesc()
        {
            return _direction == OrderDirection.Descending;
        }


        public bool IsAsc()
        {
            return _direction == OrderDirection.Ascending;
        }

        public bool IsCurrent()
        {
            return _criteria.Current == this;
        }

		public T AndOrderBy<T>() where T : OrderByCriteria
		{
			return _andOrderBy.AndOrderBy<T>();
		}
    }

    [Serializable]
    public class OrderByList : List<OrderBy> { }
}

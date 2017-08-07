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
	public class ConditionSubquery<TProperty, TSubquery> : ConditionList<TProperty>
    {
        private readonly Dictionary<string, string> _aliases;
        private readonly string _keyColumn;
        private readonly string _idColumn;


        public ConditionSubquery(ConditionContainer conditions, string propertyName, string keyColumn, string idColumn)
            : base(conditions, propertyName)
        { 
            _keyColumn = keyColumn;
            _idColumn = idColumn;

            _aliases = new Dictionary<string, string>();
        }

        public ConditionSubquery(ConditionContainer conditions, string propertyName, string keyColumn, string idColumn, string associationPath, string alias)
            : this(conditions, propertyName, keyColumn, idColumn)
        {
            _aliases.Add(associationPath, alias);
        }

        public ConditionSubquery(ConditionContainer conditions, string propertyName, string keyColumn, string idColumn, Dictionary<string,string> aliases)
            : this(conditions, propertyName, keyColumn, idColumn)
        {
            foreach (var alias in aliases)
                _aliases.Add(alias.Key, alias.Value);
        }

        public bool UseLike { get; set; }

        
        public override ICriterion GetCriterion(TProperty item)
        {
            var subqueryCriteria = DetachedCriteria.For<TSubquery>();

            foreach (var alias in _aliases)
                subqueryCriteria.CreateAlias(alias.Key, alias.Value);

            subqueryCriteria.Add(new Conjunction()
                         .Add(UseLike
                                  ? Restrictions.InsensitiveLike(PropertyName, item.ToString(), MatchMode.Anywhere)
                                  : Restrictions.Eq(PropertyName, item))
                         .Add(Restrictions.EqProperty(_keyColumn, _idColumn)))
                .SetProjection(Projections.Id());

            return Subqueries.Exists(subqueryCriteria);
        }

        protected override Junction GetInitializedJunction()
        {
            return new Disjunction();
        }
    }
}

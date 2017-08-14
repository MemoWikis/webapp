using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.SqlCommand;

namespace Seedworks.Lib.Persistence
{
    [Serializable]
    public class ConditionContainer : List<Condition>
    {
        public Condition FindByPropertyName(string propertyName)
        {
            foreach (Condition condition in this)
                if (condition.PropertyName == propertyName)
                    return condition;

            return null;
        }

        public bool Contains(string propertyName)
        {
            return FindByPropertyName(propertyName) != null;
        }

        public void AddUnique(Condition condition)
        {
            if (Contains(condition.PropertyName))
                Remove(condition);

            Add(condition);
        }

        public int CountBooleanFilters()
        {
            int resultCount = 0;
            foreach (Condition filter in this)
            {
                if (typeof(ConditionBoolean) != filter.GetType())
                    continue;

                resultCount++;
            }

            return resultCount;
        }

        private readonly Dictionary<string, string> _aliases = new Dictionary<string, string>();

        protected void AddAlias(string associationPath, string alias)
        {
            _aliases.Add(associationPath, alias);
        }

        public void CreateAliases(ICriteria criteria)
        {
            foreach (var alias in _aliases)
                criteria.CreateAlias(alias.Key, alias.Value, JoinType.LeftOuterJoin);
        }

		public virtual void Reset()
		{
			ForEach(cond => cond.Reset());
			Clear();
		}
    }
}

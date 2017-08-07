using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Seedworks.Lib.Persistence
{
	[Serializable]
	public class ConditionTextSearch : Condition
    {
        public bool SearchWithOr { get; set; }

        private readonly List<string> _propertyNames;
        private readonly List<ConditionList<string>> _subqueries;

        public ConditionTextSearch(ConditionContainer conditions, params string[] propertyNames)
            : base(conditions)
        {
            _propertyNames = new List<string>();
            _propertyNames.AddRange(propertyNames);
            
            _subqueries = new List<ConditionList<string>>();
            
            conditions.Add(this);
        }

        public void AddPropertyName(string propertyName)
        {
            _propertyNames.Add(propertyName);
        }

        public void AddSubquerySearchField<TSubquery>(string propertyName, string keyColumn, string idColumn)
        {
            var subquery = new ConditionSubquery<string, TSubquery>(null, propertyName, keyColumn, idColumn)
                               {UseLike = true};

            _subqueries.Add(subquery);
        }

        public void AddSubquerySearchField<TSubquery>(string propertyName, string keyColumn, string idColumn, string associationPath, string alias)
        {
            var subquery = new ConditionSubquery<string, TSubquery>(null, propertyName, keyColumn, idColumn,
                                                                    associationPath, alias) {UseLike = true};

            _subqueries.Add(subquery);
        }

        private readonly List<string> _items = new List<string>();
        public List<string> Items { get { return _items; } }

        public void AddTerms(string searchString)
        {
            List<string> strings = GetSearchTerms(searchString);
            Add(strings);
        }

        public static List<string> GetSearchTerms(string searchString)
        {
            var strings = Regex.Replace(searchString, "\\W", " ").Trim().Split(' ').ToList();
            strings.RemoveAll(str => string.IsNullOrEmpty(str));
            return strings;
        }

        public void Add(params string[] values)
        {
            foreach (var value in values)
                Add(value);
        }

        public void Add(List<string> values)
        {
            foreach (var value in values)
                Add(value);
        }

        public void Add(string value)
        {
            if (_items.Contains(value)) return;

            _items.Add(value);

            foreach (var subquery in _subqueries)
                subquery.Add(value);
        }

        public void Clear()
        {
            _items.Clear();

            foreach (var subquery in _subqueries)
                subquery.Clear();
        }

        public override void AddToCriteria(ICriteria criteria)
        {
            var criterion = GetCriterion();
            if (criterion != null)
            {
                criteria.Add(criterion);
            }
        }

        public override ICriterion GetCriterion()
        {
            if (_items.Count <= 0)
                return null;

            Junction junction;

            if (SearchWithOr)
                junction = Restrictions.Disjunction();
            else
                junction = Restrictions.Conjunction();

            foreach (var searchTerm in _items)
            {
                var disjunction = Restrictions.Disjunction();

                foreach (var column in _propertyNames)
                    disjunction.Add(Restrictions.InsensitiveLike(column, searchTerm, MatchMode.Anywhere));

                foreach (var subquery in _subqueries)
                    disjunction.Add(subquery.GetCriterion(searchTerm));

                junction.Add(disjunction);
            }

            return junction;
        }


    }
}

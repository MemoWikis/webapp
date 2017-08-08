using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace Seedworks.Lib.Persistence
{
    /// <summary>
    /// From http://nhforge.org/blogs/nhibernate/archive/2008/08/31/the-query-batcher.aspx.
    /// </summary>
    public class QueryBatcher
    {

        private readonly Dictionary<string, int> criteriaResultPositions;

        private readonly List<ICriteria> criteriaList;

        private readonly List<IQuery> hqlQueryList;

        private readonly Dictionary<string, int> queryResultPositions;

        private readonly ISession session;



        private IList criteriaResults;

        private IList queryResults;



        public QueryBatcher(ISession session)
        {

            this.session = session;

            criteriaList = new List<ICriteria>();

            hqlQueryList = new List<IQuery>();

            criteriaResultPositions = new Dictionary<string, int>();

            queryResultPositions = new Dictionary<string, int>();

        }

        public int CriteriaCount
        {
            get { return criteriaList.Count; }
        }


        public void AddCriteria(string key, ICriteria criteria)
        {

            criteriaList.Add(criteria);

            criteriaResultPositions.Add(key, criteriaList.Count - 1);

        }



        public void AddCriteria(string key, DetachedCriteria detachedCriteria)
        {

            AddCriteria(key, detachedCriteria.GetExecutableCriteria(session));

        }



        public object GetResult(string key)
        {

            ExecuteQueriesIfNecessary();

            object result = GetResultFromList(key, criteriaResults, criteriaResultPositions);

            if (result != null) return result;

            result = GetResultFromList(key, queryResults, queryResultPositions);

            if (result != null) return result;



            return null;

        }



        public IEnumerable<T> GetEnumerableResult<T>(string key)
        {

            var list = GetResult<IList>(key);

            return list.Cast<T>();

        }



        public T GetSingleResult<T>(string key)
        {

            var result = GetResult<IList>(key);

            return (T)result[0];

        }



        public void AddHqlQuery(string key, IQuery query)
        {

            hqlQueryList.Add(query);

            queryResultPositions.Add(key, hqlQueryList.Count - 1);

        }



        public void AddHqlQuery(string key, string query)
        {

            AddHqlQuery(key, session.CreateQuery(query));

        }



        public void ExecuteQueriesIfNecessary()
        {

            ExecuteCriteriaIfNecessary();

            ExecuteHqlIfNecessary();

        }



        private void ExecuteCriteriaIfNecessary()
        {

            if (criteriaList.Count > 0 && criteriaResults == null)
            {

                if (criteriaList.Count == 1)
                {

                    criteriaResults = new ArrayList { criteriaList[0].List() };

                }

                else
                {

                    var multiCriteria = session.CreateMultiCriteria();

                    criteriaList.ForEach(c => multiCriteria.Add(c));

                    criteriaResults = multiCriteria.List();

                }

            }

        }



        private void ExecuteHqlIfNecessary()
        {

            if (hqlQueryList.Count > 0 && queryResults == null)
            {

                if (hqlQueryList.Count == 1)
                {

                    queryResults = new ArrayList { hqlQueryList[0].List() };

                }

                else
                {

                    var multiQuery = session.CreateMultiQuery();

                    hqlQueryList.ForEach(q => multiQuery.Add(q));

                    queryResults = multiQuery.List();

                }

            }

        }



        private T GetResult<T>(string key)
        {

            return (T)GetResult(key);

        }



        private static object GetResultFromList(string key, IList list, IDictionary<string, int> positions)
        {

            if (positions.ContainsKey(key)) return list[positions[key]];

            return null;

        }

    }
}

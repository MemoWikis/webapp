using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;

public class AnswerHistoryTestRepo : RepositoryDbBase<AnswerHistoryTest>
{
	public AnswerHistoryTestRepo(ISession session) : base(session)
	{
	}

	public void TruncateTable()
	{
		_session.CreateSQLQuery("truncate table answerhistory_test").ExecuteUpdate();
	}

    public IList<AlgoSummary> LoadSummaries()
    {
        string query = 
            @"SELECT 
                  AlgoId,
	              COUNT(AlgoId) as TestCount,
	              SUM(IsCorrect) as SuccessCount,
	              AVG(IsCorrect) as SuccessRate, 
	              AVG(
                      CASE IsCorrect
                          WHEN True THEN 100 - Probability
                          WHEN False THEN ABS(0 - Probability)
                      END
                  ) as AvgDistance,
	              ROUND(AVG(
                      CASE IsCorrect
                          WHEN True THEN POW(100 - Probability, 2)
                          WHEN False THEN POW(ABS(0 - Probability), 2)
                      END
                  ), 0) as AvgDistanceWeighted
              FROM answerhistory_test
              GROUP BY AlgoId";


        return _session
            .CreateSQLQuery(query)
            .SetResultTransformer(Transformers.AliasToBean(typeof (AlgoSummary)))
            .List<AlgoSummary>();
    }
}

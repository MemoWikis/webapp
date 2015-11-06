using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;

public class AnswerTestRepo : RepositoryDbBase<AnswerTest>
{
	public AnswerTestRepo(ISession session) : base(session)
	{
	}

	public void TruncateTable()
	{
		_session.CreateSQLQuery("truncate table answer_test").ExecuteUpdate();
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
                      CASE AnswerredCorrectly
                          WHEN True THEN 100 - Probability
                          WHEN False THEN ABS(0 - Probability)
                      END
                  ) as AvgDistance,
	              ROUND(AVG(
                      CASE AnswerredCorrectly
                          WHEN True THEN POW(100 - Probability, 2)
                          WHEN False THEN POW(ABS(0 - Probability), 2)
                      END
                  ), 0) as AvgDistanceWeighted
              FROM answer_test ah_t
              LEFT JOIN answer ah
              ON ah_t.Answer_id = ah.Id
              GROUP BY AlgoId";


        return _session
            .CreateSQLQuery(query)
            .SetResultTransformer(Transformers.AliasToBean(typeof (AlgoSummary)))
            .List<AlgoSummary>();
    }

    public IList<AlgoSummary> LoadSummariesWithFeatures()
    {
        string query =
            @"SELECT 
                AlgoId,
                af.Id as FeatureId,
                af.GroupName as FeatureGroup,
                af.Name as FeatureName,
                COUNT(AlgoId) as TestCount,
                SUM(IsCorrect) as SuccessCount,
                AVG(IsCorrect) as SuccessRate,
                AVG(
                     CASE AnswerredCorrectly
                         WHEN True THEN 100 - Probability
                         WHEN False THEN ABS(0 - Probability)
                     END
                 ) as AvgDistance,
                ROUND(AVG(
                     CASE AnswerredCorrectly
                         WHEN True THEN POW(100 - Probability, 2)
                         WHEN False THEN POW(ABS(0 - Probability), 2)
                     END
                 ), 0) as AvgDistanceWeighted
              FROM answer_test ah_t
              LEFT JOIN answer ah
              ON ah_t.Answer_id = ah.Id
              LEFT JOIN answerfeature_to_answer af_t_ah
              ON ah.Id = af_t_ah.Answer_id
              LEFT JOIN answerfeature af
              ON af_t_ah.AnswerFeature_id = af.Id
              GROUP BY AlgoId, af.Id";

        return _session
            .CreateSQLQuery(query)
            .SetResultTransformer(Transformers.AliasToBean(typeof(AlgoSummary)))
            .List<AlgoSummary>();
    }
}

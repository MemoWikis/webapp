using NHibernate;

public class UpdateQuestionAnswerCounts : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public UpdateQuestionAnswerCounts(ISession session){
        _session = session;
    }

    public void Run()
    {
        // Set timeout to 5 minutes for these potentially long-running queries
        var timeout = TimeSpan.FromMinutes(5);
        
        _session.CreateSQLQuery("UPDATE Question SET TotalTrueAnswers = 0 WHERE TotalTrueAnswers is null")
            .SetTimeout((int)timeout.TotalSeconds)
            .ExecuteUpdate();
        
        _session.CreateSQLQuery("UPDATE Question SET TotalFalseAnswers = 0 WHERE TotalFalseAnswers is null")
            .SetTimeout((int)timeout.TotalSeconds)
            .ExecuteUpdate();

        // Use a more efficient approach with a single query and JOIN instead of correlated subqueries
        _session.CreateSQLQuery(@"
            UPDATE question q 
            LEFT JOIN (
                SELECT 
                    QuestionId,
                    SUM(CASE WHEN AnswerredCorrectly IN (1, 2) THEN 1 ELSE 0 END) as TrueCount,
                    SUM(CASE WHEN AnswerredCorrectly = 0 THEN 1 ELSE 0 END) as FalseCount
                FROM Answer 
                WHERE QuestionId IS NOT NULL
                GROUP BY QuestionId
            ) answer_counts ON q.Id = answer_counts.QuestionId
            SET 
                q.TotalTrueAnswers = COALESCE(answer_counts.TrueCount, 0),
                q.TotalFalseAnswers = COALESCE(answer_counts.FalseCount, 0)
            WHERE q.Id <> -1")
            .SetTimeout((int)timeout.TotalSeconds)
            .ExecuteUpdate();
    }
}
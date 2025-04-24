using NHibernate;

public class UpdateQuestionAnswerCounts : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public UpdateQuestionAnswerCounts(ISession session){
        _session = session;
    }

    public void Run()
    {
        _session.CreateSQLQuery("UPDATE Question SET TotalTrueAnswers = 0 WHERE TotalTrueAnswers is null").ExecuteUpdate();
        _session.CreateSQLQuery("UPDATE Question SET TotalFalseAnswers = 0 WHERE TotalFalseAnswers is null ").ExecuteUpdate();

        _session.CreateSQLQuery(@"UPDATE question as q
                                SET q.TotalTrueAnswers = (SELECT count(*) as AnswerredCorrectly
	                                    FROM Answer as aw
	                                    WHERE (AnswerredCorrectly = 2 OR AnswerredCorrectly = 1)
	                                    AND aw.QuestionId = q.Id),
                                q.TotalFalseAnswers = (SELECT count(*) as AnswerredCorrectly
	                                    FROM Answer as aw
	                                    WHERE AnswerredCorrectly = 0
	                                    AND aw.QuestionId = q.Id) 
                                where q.Id <> -1").ExecuteUpdate();
    }
}
using ISession = NHibernate.ISession;

public class UpdateQuestionAnswerCount : IRegisterAsInstancePerLifetime
{
    private readonly object _updateLock = new();
    private readonly ISession _session;

    public UpdateQuestionAnswerCount(ISession session)
    {
        _session = session;
    }

    public void Run(int questionId, bool isCorrect)
    {
        if (isCorrect)
            AddCorrectAnswer(questionId);
        else
            AddWrongAnswer(questionId);
    }

    private void AddCorrectAnswer(int questionId)
    {
        var query = @"UPDATE Question SET TotalTrueAnswers = TotalTrueAnswers + 1 where Id = :questionId";

        _session.CreateSQLQuery(query)
            .SetParameter("questionId", questionId)
            .ExecuteUpdate();

        EntityCache.GetQuestionById(questionId).TotalTrueAnswers++;
    }

    private void AddWrongAnswer(int questionId)
    {
        var query = "UPDATE Question SET TotalFalseAnswers = TotalFalseAnswers + 1 where Id = :questionId";
        _session.CreateSQLQuery(query)
            .SetParameter("questionId", questionId)
            .ExecuteUpdate();

        EntityCache.GetQuestionById(questionId).TotalFalseAnswers++;
    }

    public void ChangeOneWrongAnswerToCorrect(int questionId)
    {
        lock (_updateLock)
        {
            var removeFalseAnswer =
                "UPDATE Question SET TotalFalseAnswers = TotalFalseAnswers - 1 where Id = :questionId";

            _session.CreateSQLQuery(removeFalseAnswer)
                .SetParameter("questionId", questionId)
                .ExecuteUpdate();

            var addCorrectAnswer =
                "UPDATE Question SET TotalTrueAnswers = TotalTrueAnswers + 1 where Id = :questionId";
            _session.CreateSQLQuery(
                    addCorrectAnswer)
                .SetParameter("questionId", questionId)
                .ExecuteUpdate();

            EntityCache.GetQuestionById(questionId).TotalTrueAnswers++;
            EntityCache.GetQuestionById(questionId).TotalFalseAnswers--;
        }
    }
}
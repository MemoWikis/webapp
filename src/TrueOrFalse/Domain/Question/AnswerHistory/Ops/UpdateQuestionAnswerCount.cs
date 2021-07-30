using NHibernate;

public class UpdateQuestionAnswerCount : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;

    public UpdateQuestionAnswerCount(ISession session){
        _session = session;
    }

    public void Run(int questionId, bool countUnansweredIsCorrect, bool isCorrect)
    {
        if(isCorrect || countUnansweredIsCorrect)
            AddCorrectAnswer(questionId);
        else
            AddWrongAnswer(questionId);
    }

    private void AddCorrectAnswer(int questionId)
    {
        _session.CreateSQLQuery("UPDATE Question SET TotalTrueAnswers = TotalTrueAnswers + 1 where Id = " +
                                questionId).ExecuteUpdate();
        EntityCache.GetQuestionById(questionId).TotalTrueAnswers++; 
    }

    private void AddWrongAnswer(int questionId)
    {
        _session.CreateSQLQuery("UPDATE Question SET TotalFalseAnswers = TotalFalseAnswers + 1 where Id = " +
                                questionId).ExecuteUpdate();
        EntityCache.GetQuestionById(questionId).TotalFalseAnswers++;
    }

    public void ChangeOneWrongAnswerToCorrect(int questionId)
    {
        _session.CreateSQLQuery("UPDATE Question SET TotalTrueAnswers = TotalTrueAnswers + 1 where Id = " +
                                questionId).ExecuteUpdate(); 
        _session.CreateSQLQuery("UPDATE Question SET TotalFalseAnswers = TotalFalseAnswers - 1 where Id = " +
                                    questionId).ExecuteUpdate();

            EntityCache.GetQuestionById(questionId).TotalTrueAnswers++;
            EntityCache.GetQuestionById(questionId).TotalFalseAnswers--;

    }

}
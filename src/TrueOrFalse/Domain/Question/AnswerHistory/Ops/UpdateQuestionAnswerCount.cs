using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ISession = NHibernate.ISession;

public class UpdateQuestionAnswerCount : IRegisterAsInstancePerLifetime
{
    private readonly object _updateLock = new();
    private readonly ISession _session;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UpdateQuestionAnswerCount(ISession session,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _session = session;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
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
        _session.CreateSQLQuery("UPDATE Question SET TotalTrueAnswers = TotalTrueAnswers + 1 where Id = " +
                                questionId).ExecuteUpdate();
        EntityCache.GetQuestionById(questionId, _httpContextAccessor, _webHostEnvironment).TotalTrueAnswers++;
    }

    private void AddWrongAnswer(int questionId)
    {
        _session.CreateSQLQuery("UPDATE Question SET TotalFalseAnswers = TotalFalseAnswers + 1 where Id = " +
                                questionId).ExecuteUpdate();
        EntityCache.GetQuestionById(questionId, _httpContextAccessor, _webHostEnvironment).TotalFalseAnswers++;
    }

    public void ChangeOneWrongAnswerToCorrect(int questionId)
    {
        lock (_updateLock)
        {
            _session.CreateSQLQuery("UPDATE Question SET TotalTrueAnswers = TotalTrueAnswers + 1 where Id = " +
                                    questionId).ExecuteUpdate();
            _session.CreateSQLQuery("UPDATE Question SET TotalFalseAnswers = TotalFalseAnswers - 1 where Id = " +
                                    questionId).ExecuteUpdate();

            EntityCache.GetQuestionById(questionId, _httpContextAccessor, _webHostEnvironment).TotalTrueAnswers++;
            EntityCache.GetQuestionById(questionId, _httpContextAccessor, _webHostEnvironment).TotalFalseAnswers--;
        }
    }

}
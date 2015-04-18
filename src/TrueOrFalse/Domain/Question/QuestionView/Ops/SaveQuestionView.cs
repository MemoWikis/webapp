using System.Web;
using NHibernate;
using TrueOrFalse.Search;

public class SaveQuestionView : IRegisterAsInstancePerLifetime
{
    private readonly QuestionViewRepository _questionViewRepo;
    private readonly SearchIndexQuestion _searchIndexQuestion;
    private readonly ISession _session;

    public SaveQuestionView(
        QuestionViewRepository questionViewRepo, 
        SearchIndexQuestion searchIndexQuestion,
        ISession session)
    {
        _questionViewRepo = questionViewRepo;
        _searchIndexQuestion = searchIndexQuestion;
        _session = session;
    }

    public void Run(Question question, User user)
    {
        Run(question, user == null ? -1 : user.Id);
    }

    public void Run(Question question, int userId)
    {
        if (userId != -1) //if user is logged in, always log
            if (HttpContext.Current != null && HttpContext.Current.Request.Browser.Crawler)
                return;

        _questionViewRepo.Create(new QuestionView{QuestionId = question.Id, UserId = userId});
        _session.CreateSQLQuery("UPDATE Question SET TotalViews = " + _questionViewRepo.GetViewCount(question.Id) + " WHERE Id = " + question.Id).
            ExecuteUpdate();

        _searchIndexQuestion.Update(question);
    }
}

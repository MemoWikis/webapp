using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ISession = NHibernate.ISession;

public class SaveQuestionView : IRegisterAsInstancePerLifetime
{
    private readonly QuestionViewRepository _questionViewRepo;
    private readonly ISession _session;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SaveQuestionView(
        QuestionViewRepository questionViewRepo,
        ISession session,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _questionViewRepo = questionViewRepo;

        _session = session;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public void Run(Guid questionViewGuid, QuestionCacheItem question, IUserTinyModel user)
    {
        Run(questionViewGuid, question, user?.Id ?? -1);
    }

    public void Run(
        Guid questionViewGuid,
        QuestionCacheItem question,
        int userId)
    {
        if (_httpContextAccessor.HttpContext == null)
            return;

        var userAgent = UserAgent.Get(_httpContextAccessor.HttpContext);

        if (IsCrawlerRequest.Yes(_httpContextAccessor.HttpContext))
            return;

        _questionViewRepo.Create(new QuestionView
        {
            Guid = questionViewGuid,
            QuestionId = question.Id,
            UserId = userId,
            Milliseconds = -1,
            UserAgent = userAgent
        });

        _session.CreateSQLQuery("UPDATE Question SET TotalViews = " +
                                _questionViewRepo.GetViewCount(question.Id) + " WHERE Id = " +
                                question.Id).ExecuteUpdate();
    }
}
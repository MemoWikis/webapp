using Microsoft.AspNetCore.Http;
using ISession = NHibernate.ISession;

public class SaveQuestionView : IRegisterAsInstancePerLifetime
{
    private readonly QuestionViewRepository _questionViewRepo;
    private readonly ISession _session;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SaveQuestionView(
        QuestionViewRepository questionViewRepo,
        ISession session,
        IHttpContextAccessor httpContextAccessor)
    {
        _questionViewRepo = questionViewRepo;

        _session = session;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Run(QuestionCacheItem question, IUserTinyModel user)
    {
        Run(question, user.Id);
    }

    public void Run(
        QuestionCacheItem question,
        int userId)
    {
        if (_httpContextAccessor.HttpContext == null)
            return;

        var userAgent = UserAgent.Get(_httpContextAccessor.HttpContext);

        if (IsCrawlerRequest.Yes(_httpContextAccessor.HttpContext))
            return;

        var questionView = new QuestionView
        {
            QuestionId = question.Id,
            UserId = userId,
            Milliseconds = -1,
            UserAgent = userAgent,
            DateOnly = DateTime.UtcNow.Date
        };

        _questionViewRepo.Create(questionView);

        question.AddQuestionView(questionView.DateOnly);
    }
}
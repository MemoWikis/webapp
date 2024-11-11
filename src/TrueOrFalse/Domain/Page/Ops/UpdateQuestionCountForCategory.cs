using NHibernate;

public class UpdateQuestionCountForCategory : IRegisterAsInstancePerLifetime
{
    private readonly QuestionReadingRepo _questionReadingRepository;
    private readonly SessionUser _sessionUser;
    private readonly ISession _nhinbernateSession;

    public UpdateQuestionCountForCategory(
        QuestionReadingRepo questionReadingRepo,
        SessionUser sessionUser,
        ISession nhinbernateSession)
    {
        _questionReadingRepository = questionReadingRepo;
        _sessionUser = sessionUser;
        _nhinbernateSession = nhinbernateSession;
    }

    public void Run(Page page, int userId)
    {
        page.UpdateCountQuestionsAggregated(userId);
    }

    public void RunForJob(Page page, int authorId)
    {
        page.UpdateCountQuestionsAggregated(authorId);
    }

    public void Run(IList<Page> pages, int? userId = null)
    {
        userId ??= _sessionUser.UserId;

        foreach (var page in pages)
        {
            Run(page, (int)userId);
        }
    }
}
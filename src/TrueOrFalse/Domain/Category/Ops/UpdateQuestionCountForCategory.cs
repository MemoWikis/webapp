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

    public void Run(Category category, int userId)
    {
        category.UpdateCountQuestionsAggregated(userId);
    }

    public void RunForJob(Category category, int authorId)
    {
        category.UpdateCountQuestionsAggregated(authorId);
    }

    public void Run(IList<Category> categories, int? userId = null)
    {
        userId ??= _sessionUser.UserId;

        foreach (var category in categories)
        {
            Run(category, (int)userId);
        }
    }
}
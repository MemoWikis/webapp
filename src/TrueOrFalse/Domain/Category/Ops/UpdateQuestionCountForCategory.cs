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

    public void Run(Category category)
    {
        category.UpdateCountQuestionsAggregated(_sessionUser.UserId);
    }

    public void RunForJob(Category category, int authorId)
    {
        category.UpdateCountQuestionsAggregated(authorId);
    }

    public void Run(IList<Category> categories)
    {
        foreach (var category in categories)
        {
            Run(category);
        }
    }
}
using NHibernate;


public class QuestionReadingRepo : RepositoryDbBase<Question>
{
    private readonly ISession _session;
    private readonly RepositoryDb<Question> _repo;

    public QuestionReadingRepo(
        ISession session) : base(session)
    {
        _repo = new RepositoryDbBase<Question>(session);
        _session = session;
    }

    public int TotalPublicQuestionCount()
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.Public)
            .RowCount();
    }

    public int HowManyNewPublicQuestionsCreatedSince(DateTime since)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.DateCreated > since)
            .And(q => q.Visibility == QuestionVisibility.Public)
            .RowCount();
    }

    public IList<Question> GetAllEager()
    {
        var questions = _session.QueryOver<Question>().List();

        // With BatchSize(500) on QuestionMap, NHibernate batches the initialization:
        // instead of N individual queries, it fires ceil(N/500) batch queries.
        foreach (var question in questions)
        {
            NHibernateUtil.Initialize(question.Pages);
        }

        foreach (var question in questions)
        {
            NHibernateUtil.Initialize(question.References);
        }

        return questions;
    }

    public Question GetById(int id)
    {
        return _repo.GetById(id);
    }

    public IList<Question> GetAll()
    {
        return _repo.GetAll();
    }
}
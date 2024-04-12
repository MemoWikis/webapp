using NHibernate;
using Seedworks.Lib.Persistence;

public class QuestionReadingRepo : RepositoryDbBase<Question>
{
    private readonly ISession _session;
    private readonly PermissionCheck _permissionCheck;
    private readonly CategoryRepository _categoryRepository;
    private readonly RepositoryDb<Question> _repo;

    public QuestionReadingRepo(
        ISession session,
        PermissionCheck permissionCheck,
        CategoryRepository categoryRepository) : base(session)
    {
        _repo = new RepositoryDbBase<Question>(session);
        _session = session;
        _permissionCheck = permissionCheck;
        _categoryRepository = categoryRepository;
    }

    public int TotalPublicQuestionCount()
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    public int HowManyNewPublicQuestionsCreatedSince(DateTime since)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.DateCreated > since)
            .And(q => q.Visibility == QuestionVisibility.All)
            .RowCount();
    }

    public IList<Question> GetForCategory(int categoryId)
    {
        return _session.QueryOver<Question>()
            .Where(q => q.Visibility == QuestionVisibility.All)
            .JoinQueryOver<Category>(q => q.Categories)
            .Where(c => c.Id == categoryId)
            .List<Question>();
    }

    public IList<Question> GetAllEager()
    {
        var questions = _session.QueryOver<Question>().Future().ToList();

        _session.QueryOver<Question>()
            .Fetch(SelectMode.Fetch, x => x.Categories)
            .Future();

        _session.QueryOver<Question>()
            .Fetch(SelectMode.Fetch, x => x.References)
            .Future();
        var result = questions;

        foreach (var question in result)
        {
            NHibernateUtil.Initialize(question.Creator);
            NHibernateUtil.Initialize(question.References);
        }

        return result.ToList();
    }

    public Question GetById(int id)
    {
        return _repo.GetById(id);
    }

    public IList<Question> GetAll()
    {
        return _repo.GetAll();
    }

    public List<Category> GetAllParentsForQuestion(List<int> newCategoryIds, Question question)
    {
        var categories = new List<Category>();
        var privateCategories =
            question.Categories.Where(c => !_permissionCheck.CanEdit(c)).ToList();
        categories.AddRange(privateCategories);

        foreach (var categoryId in newCategoryIds)
            categories.Add(_categoryRepository.GetById(categoryId));

        return categories;
    }

    public List<Category> GetAllParentsForQuestion(int newCategoryId, Question question) =>
        GetAllParentsForQuestion(new List<int> { newCategoryId }, question);
}
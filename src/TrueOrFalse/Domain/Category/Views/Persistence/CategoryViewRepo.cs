using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;

public class CategoryViewRepo : RepositoryDb<CategoryView>
{
    private readonly CategoryRepository _categoryRepository;
    private readonly UserReadingRepo _userReadingRepo;

    public CategoryViewRepo(ISession session, CategoryRepository categoryRepository, UserReadingRepo userReadingRepo) : base(session)
    {
        _categoryRepository = categoryRepository;
        _userReadingRepo = userReadingRepo;
    }

    public int GetViewCount(int categoryId)
    {
        return _session.QueryOver<CategoryView>()
            .Select(Projections.RowCount())
            .Where(x => x.Category.Id == categoryId)
            .FutureValue<int>()
            .Value;
    }

    public int GetTodayViewCount(int categoryId)
    {
        return _session.QueryOver<CategoryView>()
            .Select(Projections.RowCount())
            .Where(x => x.Category.Id == categoryId 
                        && DateTime.Now.Date == x.DateCreated.Date)
            .FutureValue<int>()
            .Value;
    }

    public void AddView(string userAgent, int topicId, int userId)
    {
        var topic = _categoryRepository.GetById(topicId);
        var user = userId > 0 ? _userReadingRepo.GetById(userId) : null;

        var categoryView = new CategoryView
        {
            UserAgent = userAgent,
            Category = topic,
            User = user,
            DateCreated = DateTime.UtcNow
        };

        using (var transaction = _session.BeginTransaction())
        {
            _session.Save(categoryView);
            transaction.Commit();
        }

       var categoryCacheItem =  EntityCache.GetCategory(topicId);
       if (categoryCacheItem != null)
           categoryCacheItem.TodayViewCount ++; 
    }
}
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;
using System.Diagnostics;

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

    public IEnumerable<CategoryView> GetTodayViews()
    {
        var watch = new Stopwatch();
        watch.Start();
        var query = _session.CreateCriteria<CategoryView>()
            .Add(Restrictions.Ge("DateCreated", DateTime.Now.Date));

        var result = query.List<CategoryView>();  
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Logg.r.Information("GetTodayViews elapsed time:", elapsed);
        return result;
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

       Create(categoryView);

       var categoryCacheItem =  EntityCache.GetCategory(topicId);
       if (categoryCacheItem != null)
           categoryCacheItem.IncrementTodayViewCount(); 
    }
}
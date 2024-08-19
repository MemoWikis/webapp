using System.Collections.Concurrent;
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

    public ConcurrentDictionary<DateTime, int> GetViewsForLastNDays(int days)
    {
        var watch = new Stopwatch();
        watch.Start();

        var query = _session.CreateSQLQuery("SELECT COUNT(DateOnly) AS Count, DateOnly FROM CategoryView WHERE DateOnly BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE() GROUP BY DateOnly");
        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TopicViewSummary)))
            .List<TopicViewSummary>();
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();

        foreach (var item in result)
        {
            dictionaryResult[item.DateOnly] = Convert.ToInt32(item.Count);
        }

        return dictionaryResult;
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
    }
    public record struct TopicViewSummary(Int64 Count, DateTime DateOnly); 
}
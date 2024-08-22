using System.Collections.Concurrent;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;
using System.Diagnostics;

public class CategoryViewRepo(
    ISession _session, 
    CategoryRepository _categoryRepository,
    UserReadingRepo _userReadingRepo) : RepositoryDb<CategoryView>(_session)
{

    public int GetViewCount(int categoryId)
    {
        return _session.QueryOver<CategoryView>()
            .Select(Projections.RowCount())
            .Where(x => x.Category.Id == categoryId)
            .FutureValue<int>()
            .Value;
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

    public IList<TopicViewSummaryOrderById> GetViewsForLastNDaysGroupByCategoryId(int days)
    {
        var watch = new Stopwatch();
        watch.Start();

        var query = _session.CreateSQLQuery("SELECT Category_Id, DateOnly, COUNT(DateOnly) AS Count FROM CategoryView WHERE DateOnly BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE() GROUP BY Category_Id, DateOnly ORDER BY Category_Id, DateOnly;");
        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TopicViewSummaryOrderById)))
            .List<TopicViewSummaryOrderById>();
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Logg.r.Information("GetViewsForLastNDaysGroupByCategoryId " + elapsed);

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
    }
    public record struct TopicViewSummaryOrderById(Int64 Count, DateTime DateOnly, int Category_Id); 
    public record struct TopicViewSummary(Int64 Count, DateTime DateOnly); 
}
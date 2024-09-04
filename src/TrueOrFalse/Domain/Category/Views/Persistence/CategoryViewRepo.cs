using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;
using System.Collections.Concurrent;

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

    public ConcurrentDictionary<DateTime, int> GetViewsForPastNDays(int days)
    {

        var query = _session.CreateSQLQuery("SELECT COUNT(DateOnly) AS Count, DateOnly FROM CategoryView WHERE DateOnly BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE() GROUP BY DateOnly");
        query.SetParameter("days", days);

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TopicViewSummary)))
            .List<TopicViewSummary>();

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

    public ConcurrentDictionary<DateTime, int> GetActiveUserCountForPastNDays(int days)
    {
        var query = _session.CreateSQLQuery("SELECT DateOnly, COUNT(DISTINCT User_id) AS unique_user_count FROM categoryview WHERE User_id > 0 GROUP BY DateOnly ORDER BY DateOnly");
        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TopicViewSummary)))
            .List<TopicViewSummary>();

        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();
        foreach (var item in result)
        {
            dictionaryResult[item.DateOnly] = Convert.ToInt32(item.Count);
        }

        return dictionaryResult;
    }
}
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;
using System.Collections.Concurrent;

public class PageViewRepo(
    ISession _session,
    PageRepository pageRepository,
    UserReadingRepo _userReadingRepo) : RepositoryDb<PageView>(_session)
{

    public int GetViewCount(int categoryId)
    {
        return _session.QueryOver<PageView>()
            .Select(Projections.RowCount())
            .Where(x => x.Page.Id == categoryId)
            .FutureValue<int>()
            .Value;
    }

    public ConcurrentDictionary<DateTime, int> GetViewsForPastNDays(int days)
    {

        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly 
        FROM CategoryView 
        WHERE DateOnly BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE() 
        GROUP BY DateOnly");

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

    public IList<TopicViewSummary> GetViewsForPastNDaysById(int days, int id)
    {
        var query = _session.CreateSQLQuery(@"
            SELECT COUNT(DateOnly) AS Count, DateOnly 
            FROM CategoryView 
            WHERE Category_id = :categoryId AND DateOnly BETWEEN NOW() - INTERVAL :days DAY AND NOW()
            GROUP BY DateOnly");

        query.SetParameter("days", days);
        query.SetParameter("categoryId", id);

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TopicViewSummary)))
            .List<TopicViewSummary>();

        return result;
    }

    public IList<TopicViewSummary> GetViewsForPastNDaysByIds(int days, List<int> ids)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly 
        FROM CategoryView 
        WHERE Category_id IN (:categoryIds) AND DateOnly BETWEEN NOW() - INTERVAL :days DAY AND NOW()
        GROUP BY DateOnly");

        query.SetParameterList("categoryIds", ids);
        query.SetParameter("days", days);

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TopicViewSummary)))
            .List<TopicViewSummary>();

        return result;
    }


    public IList<TopicViewSummaryWithId> GetViewsForLastNDaysGroupByCategoryId(int days)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT Category_Id AS PageId, DateOnly, COUNT(DateOnly) AS Count 
        FROM CategoryView 
        WHERE DateOnly 
            BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE()
        GROUP BY Category_Id, DateOnly 
        ORDER BY Category_Id, DateOnly;");

        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TopicViewSummaryWithId)))
            .List<TopicViewSummaryWithId>();

        return result;
    }

    public void AddView(string userAgent, int topicId, int userId)
    {
        var topic = pageRepository.GetById(topicId);
        var user = userId > 0 ? _userReadingRepo.GetById(userId) : null;

        var categoryView = new PageView
        {
            UserAgent = userAgent,
            Page = topic,
            User = user,
            DateCreated = DateTime.UtcNow,
            DateOnly = DateTime.UtcNow.Date
        };

        Create(categoryView);
        EntityCache.GetPage(topicId)?.AddTopicView(categoryView.DateOnly);
        GraphService.IncrementTotalViewsForAllAscendants(topicId);
    }

    public record struct TopicViewSummaryWithId(Int64 Count, DateTime DateOnly, int PageId);
    public record struct TopicViewSummary(Int64 Count, DateTime DateOnly);

    public ConcurrentDictionary<DateTime, int> GetActiveUserCountForPastNDays(int days)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT DateOnly, COUNT(DISTINCT User_id) AS Count
        FROM categoryview
        WHERE User_id > 0
          AND DateOnly >= CURDATE() - INTERVAL :days DAY
        GROUP BY DateOnly
        ORDER BY DateOnly");

        query.SetParameter("days", days);

        var result = query
            .SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TopicViewSummary)))
            .List<TopicViewSummary>();

        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();
        foreach (var item in result)
        {
            dictionaryResult[item.DateOnly] = Convert.ToInt32(item.Count);
        }

        return dictionaryResult;
    }

    public IList<TopicViewSummaryWithId> GetAllEager()
    {
        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly, Category_Id
        FROM categoryview 
        GROUP BY 
            Category_Id, 
            DateOnly
        ORDER BY 
            Category_Id, 
            DateOnly;");

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TopicViewSummaryWithId)))
            .List<TopicViewSummaryWithId>();

        return result;
    }
}
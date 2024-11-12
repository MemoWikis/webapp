using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib.Persistence;
using System.Collections.Concurrent;

public class PageViewRepo(
    ISession _session,
    PageRepository pageRepository,
    UserReadingRepo _userReadingRepo) : RepositoryDb<PageView>(_session)
{

    public int GetViewCount(int pageId)
    {
        return _session.QueryOver<PageView>()
            .Select(Projections.RowCount())
            .Where(x => x.Page.Id == pageId)
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

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PageViewSummary)))
            .List<PageViewSummary>();

        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();
        foreach (var item in result)
        {
            dictionaryResult[item.DateOnly] = Convert.ToInt32(item.Count);
        }

        return dictionaryResult;
    }

    public IList<PageViewSummary> GetViewsForPastNDaysById(int days, int id)
    {
        var query = _session.CreateSQLQuery(@"
            SELECT COUNT(DateOnly) AS Count, DateOnly 
            FROM CategoryView 
            WHERE Category_id = :pageId AND DateOnly BETWEEN NOW() - INTERVAL :days DAY AND NOW()
            GROUP BY DateOnly");

        query.SetParameter("days", days);
        query.SetParameter("pageId", id);

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PageViewSummary)))
            .List<PageViewSummary>();

        return result;
    }

    public IList<PageViewSummary> GetViewsForPastNDaysByIds(int days, List<int> ids)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly 
        FROM CategoryView 
        WHERE Category_id IN (:pageIds) AND DateOnly BETWEEN NOW() - INTERVAL :days DAY AND NOW()
        GROUP BY DateOnly");

        query.SetParameterList("pageIds", ids);
        query.SetParameter("days", days);

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PageViewSummary)))
            .List<PageViewSummary>();

        return result;
    }


    public IList<PageViewSummaryWithId> GetViewsForLastNDaysGroupByPageId(int days)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT Category_Id AS PageId, DateOnly, COUNT(DateOnly) AS Count 
        FROM CategoryView 
        WHERE DateOnly 
            BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE()
        GROUP BY Category_Id, DateOnly 
        ORDER BY Category_Id, DateOnly;");

        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PageViewSummaryWithId)))
            .List<PageViewSummaryWithId>();

        return result;
    }

    public void AddView(string userAgent, int pageId, int userId)
    {
        var topic = pageRepository.GetById(pageId);
        var user = userId > 0 ? _userReadingRepo.GetById(userId) : null;

        var pageView = new PageView
        {
            UserAgent = userAgent,
            Page = topic,
            User = user,
            DateCreated = DateTime.UtcNow,
            DateOnly = DateTime.UtcNow.Date
        };

        Create(pageView);
        EntityCache.GetPage(pageId)?.AddPageView(pageView.DateOnly);
        GraphService.IncrementTotalViewsForAllAscendants(pageId);
    }

    public record struct PageViewSummaryWithId(Int64 Count, DateTime DateOnly, int PageId);
    public record struct PageViewSummary(Int64 Count, DateTime DateOnly);

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
            .SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PageViewSummary)))
            .List<PageViewSummary>();

        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();
        foreach (var item in result)
        {
            dictionaryResult[item.DateOnly] = Convert.ToInt32(item.Count);
        }

        return dictionaryResult;
    }

    public IList<PageViewSummaryWithId> GetAllEager()
    {
        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly, Category_Id as PageId
        FROM categoryview 
        GROUP BY 
            Category_Id, 
            DateOnly
        ORDER BY 
            Category_Id, 
            DateOnly;");

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PageViewSummaryWithId)))
            .List<PageViewSummaryWithId>();

        return result;
    }
}
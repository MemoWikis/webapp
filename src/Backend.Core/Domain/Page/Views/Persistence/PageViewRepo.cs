using NHibernate;
using NHibernate.Criterion;
using System.Collections.Concurrent;

public class PageViewRepo(
    ISession _session,
    PageRepository pageRepository,
    UserReadingRepo _userReadingRepo,
    ExtendedUserCache _extendedUserCache) : RepositoryDb<PageView>(_session)
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
        FROM pageview 
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

    public IList<PageViewSummaryWithId> GetViewsForLastNDaysGroupByPageId(int days)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT Page_Id AS PageId, DateOnly, COUNT(DateOnly) AS Count 
        FROM pageview 
        WHERE DateOnly 
            BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE()
        GROUP BY Page_Id, DateOnly 
        ORDER BY Page_Id, DateOnly;");

        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PageViewSummaryWithId)))
            .List<PageViewSummaryWithId>();

        return result;
    }

    public void AddView(string userAgent, int pageId, int userId)
    {
        var page = pageRepository.GetById(pageId);
        var user = userId > 0 ? _userReadingRepo.GetById(userId) : null;

        var pageView = new PageView
        {
            UserAgent = userAgent,
            Page = page,
            User = user,
            DateCreated = DateTime.UtcNow,
            DateOnly = DateTime.UtcNow.Date
        };

        Create(pageView);
        EntityCache.GetPage(pageId)?.AddPageView(pageView.DateOnly);
        GraphService.IncrementTotalViewsForAllAscendants(pageId);

        AddToRecentPages(pageId, userId);
    }

    private void AddToRecentPages(int pageId, int userId)
    {
        if (userId <= 0)
            return;

        if (!_extendedUserCache.ItemExists(userId))
            return;

        var userCacheItem = _extendedUserCache.GetUser(userId);
        if (userCacheItem.RecentPages != null)
            userCacheItem.RecentPages.VisitPage(pageId);
    }

    public ConcurrentDictionary<DateTime, int> GetActiveUserCountForPastNDays(int days)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT DateOnly, COUNT(DISTINCT User_id) AS Count
        FROM pageview
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
        const int batchSize = 100000; // Balanced for performance and stability
        var allResults = new List<PageViewSummaryWithId>();
        int? lastPageId = null;
        DateTime? lastDateOnly = null;
        
        Log.Information("Loading page views in batches of {BatchSize} using cursor-based pagination", batchSize);
        
        while (true)
        {
            var batch = GetAllEagerBatchCursorWithRetry(lastPageId, lastDateOnly, batchSize);
            if (!batch.Any())
                break;
                
            allResults.AddRange(batch);
            
            // Update cursor to the last item in this batch
            var lastItem = batch.Last();
            lastPageId = lastItem.PageId;
            lastDateOnly = lastItem.DateOnly;
            
            Log.Information("Loaded batch with {Count} page views (total: {Total}), last cursor: PageId={PageId}, DateOnly={DateOnly}", 
                batch.Count, allResults.Count, lastPageId, lastDateOnly);
        }
        
        Log.Information("Finished loading {Total} page views", allResults.Count);
        return allResults;
    }

    private IList<PageViewSummaryWithId> GetAllEagerBatchCursor(int? lastPageId, DateTime? lastDateOnly, int batchSize)
    {
        string sqlQuery;
        
        if (lastPageId.HasValue && lastDateOnly.HasValue)
        {
            sqlQuery = @"
                SELECT COUNT(DateOnly) AS Count, DateOnly, Page_Id as PageId, MAX(DateCreated) as LastPageViewCreatedAt
                FROM pageview 
                WHERE (Page_Id > :lastPageId) 
                   OR (Page_Id = :lastPageId AND DateOnly > :lastDateOnly)
                GROUP BY 
                    Page_Id, 
                    DateOnly
                ORDER BY 
                    Page_Id, 
                    DateOnly
                LIMIT :batchSize";
        }
        else
        {
            sqlQuery = @"
                SELECT COUNT(DateOnly) AS Count, DateOnly, Page_Id as PageId, MAX(DateCreated) as LastPageViewCreatedAt
                FROM pageview 
                GROUP BY 
                    Page_Id, 
                    DateOnly
                ORDER BY 
                    Page_Id, 
                    DateOnly
                LIMIT :batchSize";
        }

        var query = _session.CreateSQLQuery(sqlQuery);

        if (lastPageId.HasValue && lastDateOnly.HasValue)
        {
            query.SetParameter("lastPageId", lastPageId.Value);
            query.SetParameter("lastDateOnly", lastDateOnly.Value);
        }
        query.SetParameter("batchSize", batchSize);
        query.SetTimeout(300); // 5 minutes timeout

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PageViewSummaryWithId)))
            .List<PageViewSummaryWithId>();

        return result;
    }

    private IList<PageViewSummaryWithId> GetAllEagerBatchCursorWithRetry(int? lastPageId, DateTime? lastDateOnly, int batchSize)
    {
        const int maxRetries = 3;
        const int retryDelayMs = 5000; // 5 seconds
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                return GetAllEagerBatchCursor(lastPageId, lastDateOnly, batchSize);
            }
            catch (Exception ex) when (attempt < maxRetries && IsRetriableException(ex))
            {
                Log.Error(ex, "PageView batch query failed on attempt {Attempt}/{MaxRetries}. Retrying in {DelayMs}ms... Cursor: PageId={PageId}, DateOnly={DateOnly}", 
                    attempt, maxRetries, retryDelayMs, lastPageId, lastDateOnly);
                Thread.Sleep(retryDelayMs);
            }
        }
        
        // Final attempt without retry
        try
        {
            return GetAllEagerBatchCursor(lastPageId, lastDateOnly, batchSize);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "PageView batch query failed after {MaxRetries} attempts. Cursor: PageId={PageId}, DateOnly={DateOnly}, BatchSize={BatchSize}", 
                maxRetries, lastPageId, lastDateOnly, batchSize);
            throw;
        }
    }

    private static bool IsRetriableException(Exception ex)
    {
        return ex.Message.Contains("Connection reset by peer") ||
               ex.Message.Contains("Reading from the stream has failed") ||
               ex.Message.Contains("Fatal error encountered during data read") ||
               ex.Message.Contains("timeout");
    }

    public IList<PageViewSummaryWithId> GetAllEagerSince(DateTime sinceDate)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly, Page_Id as PageId, MAX(DateCreated) as LastPageViewCreatedAt
        FROM pageview 
        WHERE DateCreated > :sinceDate
        GROUP BY 
            Page_Id, 
            DateOnly
        ORDER BY 
            Page_Id, 
            DateOnly;");

        query.SetParameter("sinceDate", sinceDate);

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(PageViewSummaryWithId)))
            .List<PageViewSummaryWithId>();

        return result;
    }

    public IList<int> GetRecentPagesForUser(int userId)
    {
        var query = _session.CreateSQLQuery(@"
            SELECT Page_id
            FROM pageview
            WHERE user_id = :userId AND Page_id IS NOT NULL
            GROUP BY Page_id
            ORDER BY MAX(DateCreated) DESC
            LIMIT 5");

        query.SetParameter("userId", userId);

        return query.List<int>();
    }
}



public record struct PageViewSummary(Int64 Count, DateTime DateOnly);
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Concurrent;
using System.Diagnostics;

public class QuestionViewRepository(ISession _session, QuestionViewMmapCache questionViewMmapCache) : RepositoryDbBase<QuestionView>(_session)
{
    public int GetViewCount(int questionId)
    {
        return _session.QueryOver<QuestionView>()
            .Select(Projections.RowCount())
            .Where(x => x.QuestionId == questionId)
            .FutureValue<int>()
            .Value;
    }

    public ConcurrentDictionary<DateTime, int> GetViewsForPastNDays(int days)
    {
        var watch = new Stopwatch();
        watch.Start();

        var query = _session.CreateSQLQuery(@"
            SELECT 
                COUNT(DateOnly) AS Count, 
                DateOnly 
            FROM QuestionView 
            WHERE DateOnly 
            BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE() 
            GROUP BY DateOnly");

        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummary)))
            .List<QuestionViewSummary>();
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Log.Information("GetViewsForLastNDays took " + elapsed + "ms");

        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();
        foreach (var item in result)
        {
            dictionaryResult[item.DateOnly] = Convert.ToInt32(item.Count);
        }

        return dictionaryResult;
    }

    public IList<QuestionViewSummaryWithId> GetViewsForLastNDaysGroupByQuestionId(int days)
    {
        var watch = new Stopwatch();
        watch.Start();

        var query = _session.CreateSQLQuery(@"
            SELECT 
                QuestionId, 
                DateOnly, 
                COUNT(DateOnly) AS Count 
            FROM QuestionView 
            WHERE DateOnly 
            BETWEEN CURDATE() - INTERVAL :days DAY AND CURDATE() 
            GROUP BY 
                QuestionId, 
                DateOnly 
            ORDER BY 
                QuestionId, 
                DateOnly;");
        query.SetParameter("days", days);
        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummaryWithId)))
            .List<QuestionViewSummaryWithId>();
        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Log.Information("GetViewsForLastNDaysGroupByQuestionId " + elapsed);

        return result;
    }

    public void DeleteForQuestion(int questionId)
    {
        Session.CreateSQLQuery("DELETE FROM questionview WHERE QuestionId = :questionId")
            .SetParameter("questionId", questionId).ExecuteUpdate();
    }

    public record struct QuestionViewSummary(Int64 Count, DateTime DateOnly);

    public IList<QuestionViewSummaryWithId> GetAllEager()
    {
        const int batchSize = 1000000;
        var allResults = new List<QuestionViewSummaryWithId>();
        int? lastQuestionId = null;
        DateTime? lastDateOnly = null;
        
        Log.Information("Loading question views in batches of {BatchSize} using cursor-based pagination", batchSize);
        
        while (true)
        {
            var batch = GetAllEagerBatchCursorWithRetry(lastQuestionId, lastDateOnly, batchSize);
            if (!batch.Any())
                break;
                
            allResults.AddRange(batch);
            
            // Update cursor to the last item in this batch
            var lastItem = batch.Last();
            lastQuestionId = lastItem.QuestionId;
            lastDateOnly = lastItem.DateOnly;
            
            Log.Information("Loaded batch with {Count} question views (total: {Total}), last cursor: QuestionId={QuestionId}, DateOnly={DateOnly}", 
                batch.Count, allResults.Count, lastQuestionId, lastDateOnly);
        }
        
        Log.Information("Finished loading {Total} question views", allResults.Count);
        return allResults;
    }

    private IList<QuestionViewSummaryWithId> GetAllEagerBatchCursor(int? lastQuestionId, DateTime? lastDateOnly, int batchSize)
    {
        string sqlQuery;
        
        if (lastQuestionId.HasValue && lastDateOnly.HasValue)
        {
            sqlQuery = @"
                SELECT COUNT(DateOnly) AS Count, DateOnly, QuestionId, MAX(DateCreated) as DateCreated
                FROM QuestionView 
                WHERE (QuestionId > :lastQuestionId) 
                   OR (QuestionId = :lastQuestionId AND DateOnly > :lastDateOnly)
                GROUP BY 
                    QuestionId, 
                    DateOnly
                ORDER BY 
                    QuestionId, 
                    DateOnly
                LIMIT :batchSize";
        }
        else
        {
            sqlQuery = @"
                SELECT COUNT(DateOnly) AS Count, DateOnly, QuestionId, MAX(DateCreated) as DateCreated
                FROM QuestionView 
                GROUP BY 
                    QuestionId, 
                    DateOnly
                ORDER BY 
                    QuestionId, 
                    DateOnly
                LIMIT :batchSize";
        }

        var query = _session.CreateSQLQuery(sqlQuery);

        if (lastQuestionId.HasValue && lastDateOnly.HasValue)
        {
            query.SetParameter("lastQuestionId", lastQuestionId.Value);
            query.SetParameter("lastDateOnly", lastDateOnly.Value);
        }
        query.SetParameter("batchSize", batchSize);
        query.SetTimeout(300); // 5 minutes timeout

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummaryWithId)))
            .List<QuestionViewSummaryWithId>();

        return result;
    }

    private IList<QuestionViewSummaryWithId> GetAllEagerBatchCursorWithRetry(int? lastQuestionId, DateTime? lastDateOnly, int batchSize)
    {
        const int maxRetries = 3;
        const int retryDelayMs = 5000; // 5 seconds
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                return GetAllEagerBatchCursor(lastQuestionId, lastDateOnly, batchSize);
            }
            catch (Exception ex) when (attempt < maxRetries && IsRetriableException(ex))
            {
                Log.Error(ex, "QuestionView batch query failed on attempt {Attempt}/{MaxRetries}. Retrying in {DelayMs}ms... Cursor: QuestionId={QuestionId}, DateOnly={DateOnly}", 
                    attempt, maxRetries, retryDelayMs, lastQuestionId, lastDateOnly);
                Thread.Sleep(retryDelayMs);
            }
        }
        
        // Final attempt without retry
        try
        {
            return GetAllEagerBatchCursor(lastQuestionId, lastDateOnly, batchSize);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "QuestionView batch query failed after {MaxRetries} attempts. Cursor: QuestionId={QuestionId}, DateOnly={DateOnly}, BatchSize={BatchSize}", 
                maxRetries, lastQuestionId, lastDateOnly, batchSize);
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

    public IList<QuestionViewSummaryWithId> GetAllEagerSince(DateTime sinceDate)
    {
        var query = _session.CreateSQLQuery(@"
        SELECT COUNT(DateOnly) AS Count, DateOnly, QuestionId, MAX(DateCreated) as DateCreated
        FROM QuestionView 
        WHERE DateCreated > :sinceDate
        GROUP BY 
            QuestionId, 
            DateOnly
        ORDER BY 
            QuestionId, 
            DateOnly;");

        query.SetParameter("sinceDate", sinceDate);

        var result = query.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(QuestionViewSummaryWithId)))
            .List<QuestionViewSummaryWithId>();

        return result;
    }
}
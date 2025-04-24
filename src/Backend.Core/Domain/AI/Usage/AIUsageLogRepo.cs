using NHibernate;
using System.Collections.Concurrent;
public class AiUsageLogRepo(ISession _session) : RepositoryDbBase<AiUsageLog>(_session)
{
    public void AddUsage(AnthropicApiResponse response, int userId, int pageId)
    {
        var aiUsageLog = new AiUsageLog
        {
            UserId = userId,
            PageId = pageId,
            TokenIn = response.Usage.InputTokens,
            TokenOut = response.Usage.OutputTokens,
            DateCreated = DateTime.UtcNow,
            Model = response.Model
        };
        base.Create(aiUsageLog);
    }

    public void AddUsage(int userId, int pageId, int tokenIn, int tokenOut, string model)
    {
        var aiUsageLog = new AiUsageLog
        {
            UserId = userId,
            PageId = pageId,
            TokenIn = tokenIn,
            TokenOut = tokenOut,
            DateCreated = DateTime.UtcNow,
            Model = model
        };

        base.Create(aiUsageLog);
    }

    public ConcurrentDictionary<DateTime, int> GetTokenUsageForUserFromPastNDays(int userId, int days)
    {
        var query = _session.CreateSQLQuery(@"
            SELECT DATE(DateCreated) AS DateOnly, SUM(TokenIn + TokenOut) AS TokenCount
            FROM ai_usage_log
            WHERE UserId = :userId
              AND DateCreated >= CURDATE() - INTERVAL :days DAY
            GROUP BY DateOnly
            ORDER BY DateOnly");

        query.SetParameter("userId", userId);
        query.SetParameter("days", days);

        var result = query
            .SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(TokenUsage)))
            .List<TokenUsage>();

        var dictionaryResult = new ConcurrentDictionary<DateTime, int>();
        foreach (var item in result)
        {
            dictionaryResult[item.Date] = Convert.ToInt32(item.TotalTokens);
        }

        return dictionaryResult;
    }
}

using NHibernate;
using System.Collections.Concurrent;

public class AiUsageLogRepo(ISession _session, TokenDeductionService _tokenDeductionService) : RepositoryDbBase<AiUsageLog>(_session)
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

        // Deduct tokens from user balance (with model-aware multiplier)
        _tokenDeductionService.DeductTokens(userId, response.Model, response.Usage.InputTokens, response.Usage.OutputTokens);
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

        // Deduct tokens from user balance (with model-aware multiplier)
        _tokenDeductionService.DeductTokens(userId, model, tokenIn, tokenOut);
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

    /// <summary>
    /// Get usage logs with calculated USD costs.
    /// Joins with aimodelwhitelist to find the pricing that was active at the time of each usage.
    /// </summary>
    public List<AiUsageWithCost> GetUsageWithCosts(DateTime? fromDate = null, DateTime? toDate = null, int? userId = null)
    {
        // This query finds the appropriate pricing version for each usage log entry
        // by matching on ModelId and finding the most recent DateCreated <= usage.DateCreated
        var sql = @"
            SELECT 
                u.Id,
                u.User_id AS UserId,
                u.Page_id AS PageId,
                u.TokenIn,
                u.TokenOut,
                u.DateCreated,
                u.Model AS ModelId,
                w.DisplayName,
                w.InputPricePerMillion,
                w.OutputPricePerMillion,
                w.TokenCostMultiplier,
                (u.TokenIn / 1000000.0) * COALESCE(w.InputPricePerMillion, 0) AS InputCostUsd,
                (u.TokenOut / 1000000.0) * COALESCE(w.OutputPricePerMillion, 0) AS OutputCostUsd,
                ((u.TokenIn / 1000000.0) * COALESCE(w.InputPricePerMillion, 0)) + 
                ((u.TokenOut / 1000000.0) * COALESCE(w.OutputPricePerMillion, 0)) AS TotalCostUsd
            FROM ai_usage_log u
            LEFT JOIN aimodelwhitelist w ON w.Id = (
                SELECT w2.Id 
                FROM aimodelwhitelist w2 
                WHERE w2.ModelId = u.Model 
                  AND w2.DateCreated <= u.DateCreated
                ORDER BY w2.DateCreated DESC 
                LIMIT 1
            )
            WHERE 1=1";

        if (fromDate.HasValue)
        {
            sql += " AND u.DateCreated >= :fromDate";
        }
        if (toDate.HasValue)
        {
            sql += " AND u.DateCreated <= :toDate";
        }
        if (userId.HasValue)
        {
            sql += " AND u.User_id = :userId";
        }

        sql += " ORDER BY u.DateCreated DESC";

        var query = _session.CreateSQLQuery(sql);

        if (fromDate.HasValue)
        {
            query.SetParameter("fromDate", fromDate.Value);
        }
        if (toDate.HasValue)
        {
            query.SetParameter("toDate", toDate.Value);
        }
        if (userId.HasValue)
        {
            query.SetParameter("userId", userId.Value);
        }

        return query
            .SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(AiUsageWithCost)))
            .List<AiUsageWithCost>()
            .ToList();
    }

    /// <summary>
    /// Get aggregated costs grouped by model for a date range
    /// </summary>
    public List<AiModelCostSummary> GetCostSummaryByModel(DateTime? fromDate = null, DateTime? toDate = null)
    {
        var sql = @"
            SELECT 
                u.Model AS ModelId,
                MAX(w.DisplayName) AS DisplayName,
                SUM(u.TokenIn) AS TotalInputTokens,
                SUM(u.TokenOut) AS TotalOutputTokens,
                COUNT(*) AS UsageCount,
                SUM((u.TokenIn / 1000000.0) * COALESCE(w.InputPricePerMillion, 0)) AS TotalInputCostUsd,
                SUM((u.TokenOut / 1000000.0) * COALESCE(w.OutputPricePerMillion, 0)) AS TotalOutputCostUsd,
                SUM(((u.TokenIn / 1000000.0) * COALESCE(w.InputPricePerMillion, 0)) + 
                    ((u.TokenOut / 1000000.0) * COALESCE(w.OutputPricePerMillion, 0))) AS TotalCostUsd
            FROM ai_usage_log u
            LEFT JOIN aimodelwhitelist w ON w.Id = (
                SELECT w2.Id 
                FROM aimodelwhitelist w2 
                WHERE w2.ModelId = u.Model 
                  AND w2.DateCreated <= u.DateCreated
                ORDER BY w2.DateCreated DESC 
                LIMIT 1
            )
            WHERE 1=1";

        if (fromDate.HasValue)
        {
            sql += " AND u.DateCreated >= :fromDate";
        }
        if (toDate.HasValue)
        {
            sql += " AND u.DateCreated <= :toDate";
        }

        sql += " GROUP BY u.Model ORDER BY TotalCostUsd DESC";

        var query = _session.CreateSQLQuery(sql);

        if (fromDate.HasValue)
        {
            query.SetParameter("fromDate", fromDate.Value);
        }
        if (toDate.HasValue)
        {
            query.SetParameter("toDate", toDate.Value);
        }

        return query
            .SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(AiModelCostSummary)))
            .List<AiModelCostSummary>()
            .ToList();
    }

    /// <summary>
    /// Get total costs for a date range
    /// </summary>
    public AiTotalCostSummary GetTotalCosts(DateTime? fromDate = null, DateTime? toDate = null)
    {
        var sql = @"
            SELECT 
                SUM(u.TokenIn) AS TotalInputTokens,
                SUM(u.TokenOut) AS TotalOutputTokens,
                COUNT(*) AS TotalRequests,
                SUM((u.TokenIn / 1000000.0) * COALESCE(w.InputPricePerMillion, 0)) AS TotalInputCostUsd,
                SUM((u.TokenOut / 1000000.0) * COALESCE(w.OutputPricePerMillion, 0)) AS TotalOutputCostUsd,
                SUM(((u.TokenIn / 1000000.0) * COALESCE(w.InputPricePerMillion, 0)) + 
                    ((u.TokenOut / 1000000.0) * COALESCE(w.OutputPricePerMillion, 0))) AS TotalCostUsd
            FROM ai_usage_log u
            LEFT JOIN aimodelwhitelist w ON w.Id = (
                SELECT w2.Id 
                FROM aimodelwhitelist w2 
                WHERE w2.ModelId = u.Model 
                  AND w2.DateCreated <= u.DateCreated
                ORDER BY w2.DateCreated DESC 
                LIMIT 1
            )
            WHERE 1=1";

        if (fromDate.HasValue)
        {
            sql += " AND u.DateCreated >= :fromDate";
        }
        if (toDate.HasValue)
        {
            sql += " AND u.DateCreated <= :toDate";
        }

        var query = _session.CreateSQLQuery(sql);

        if (fromDate.HasValue)
        {
            query.SetParameter("fromDate", fromDate.Value);
        }
        if (toDate.HasValue)
        {
            query.SetParameter("toDate", toDate.Value);
        }

        var result = query
            .SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(AiTotalCostSummary)))
            .UniqueResult<AiTotalCostSummary>();

        return result ?? new AiTotalCostSummary();
    }
}

/// <summary>
/// DTO for usage log with calculated costs
/// </summary>
public class AiUsageWithCost
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PageId { get; set; }
    public int TokenIn { get; set; }
    public int TokenOut { get; set; }
    public DateTime DateCreated { get; set; }
    public string ModelId { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public decimal InputPricePerMillion { get; set; }
    public decimal OutputPricePerMillion { get; set; }
    public decimal TokenCostMultiplier { get; set; }
    public decimal InputCostUsd { get; set; }
    public decimal OutputCostUsd { get; set; }
    public decimal TotalCostUsd { get; set; }
}

/// <summary>
/// DTO for cost summary by model
/// </summary>
public class AiModelCostSummary
{
    public string ModelId { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public long TotalInputTokens { get; set; }
    public long TotalOutputTokens { get; set; }
    public int UsageCount { get; set; }
    public decimal TotalInputCostUsd { get; set; }
    public decimal TotalOutputCostUsd { get; set; }
    public decimal TotalCostUsd { get; set; }
}

/// <summary>
/// DTO for total cost summary
/// </summary>
public class AiTotalCostSummary
{
    public long TotalInputTokens { get; set; }
    public long TotalOutputTokens { get; set; }
    public int TotalRequests { get; set; }
    public decimal TotalInputCostUsd { get; set; }
    public decimal TotalOutputCostUsd { get; set; }
    public decimal TotalCostUsd { get; set; }
}

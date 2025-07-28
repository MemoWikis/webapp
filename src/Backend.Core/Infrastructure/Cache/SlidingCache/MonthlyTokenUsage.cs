using System.Collections.Concurrent;

public class MonthlyTokenUsage()
{
    private ConcurrentDictionary<DateTime, int> _tokenUsages = new ConcurrentDictionary<DateTime, int>();

    public MonthlyTokenUsage(int userId, AiUsageLogRepo aiUsageLogRepo) : this()
    {
        var recentTokenUsages = aiUsageLogRepo.GetTokenUsageForUserFromPastNDays(userId, 30);
        _tokenUsages = recentTokenUsages;
    }

    public void AddTokenUsageEntry(TokenUsage tokenUsage)
    {
        _tokenUsages.AddOrUpdate(tokenUsage.Date, tokenUsage.TotalTokens, (key, oldValue) => oldValue + tokenUsage.TotalTokens);
        RemoveOldEntries();
    }

    public int CalculateMonthlyTokenUsage()
    {
        RemoveOldEntries();
        return _tokenUsages.Values.Sum();
    }

    public int CalculateMonthlyTokenUsageAsSubscriber(DateTime subscriptionStartDate)
    {
        var now = DateTime.UtcNow;
        var startOfCurrentPeriod = new DateTime(now.Year, now.Month, subscriptionStartDate.Day);

        if (now.Day < subscriptionStartDate.Day)
        {
            startOfCurrentPeriod = startOfCurrentPeriod.AddMonths(-1);
        }

        var cutoffDate = startOfCurrentPeriod.AddMonths(-1);

        return _tokenUsages
            .Where(entry => entry.Key >= cutoffDate && entry.Key < now)
            .Sum(entry => entry.Value);
    }

    private void RemoveOldEntries()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-30);
        foreach (var key in _tokenUsages.Keys)
        {
            if (key < cutoffDate)
            {
                _tokenUsages.TryRemove(key, out _);
            }
        }
    }
}

public record struct TokenUsage(int TotalTokens, DateTime Date);
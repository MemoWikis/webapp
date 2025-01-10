public class MonthlyTokenUsage()
{
    public int TotalTokens;
    private List<TokenUsage> _tokenUsages = new List<TokenUsage>();

    public MonthlyTokenUsage(int userId, AiUsageLogRepo aiUsageLogRepo) : this()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-30);
        //var recentUsages = aiUsageLogRepo.GetRecentTokenUsagesForUser(userId, cutoffDate);

        //_tokenUsages = recentUsages
        //    .Select(usage => new TokenUsage(usage.DateCreated, usage.TokenIn + usage.TokenOut));
    }


    public void AddTokenUsageEntry(TokenUsage entry)
    {
        _tokenUsages.Add(entry);
        RemoveOldEntries();
    }

    public int CalculateMonthlyTokenUsage()
    {
        RemoveOldEntries();
        return _tokenUsages.Sum(entry => entry.TotalTokens);
    }

    private void RemoveOldEntries()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-30);
        _tokenUsages = _tokenUsages
            .Where(entry => entry.DateCreated >= cutoffDate)
            .ToList();
    }
}

public record struct TokenUsage(int TotalTokens, DateTime DateCreated);
#nullable enable

public class AiUsageStoreController(
    SessionUser _sessionUser,
    AiUsageLogRepo _aiUsageLogRepo,
    TokenDeductionService _tokenDeductionService,
    AiModelRegistry _aiModelRegistry) : ApiBaseController
{
    public readonly record struct DailyUsageSummaryItem(
        DateTime Date,
        long RequestCount,
        decimal TotalTokensIn,
        decimal TotalTokensOut);

    public readonly record struct DailyModelUsageItem(
        DateTime Date,
        string ModelId,
        string? DisplayName,
        long RequestCount,
        decimal TokensIn,
        decimal TokensOut,
        decimal TokenCostMultiplier);

    public readonly record struct GetAiUsageResponse(
        bool Success,
        int TokenBalance,
        int SubscriptionTokensBalance,
        int PaidTokensBalance,
        List<DailyUsageSummaryItem> DailySummary,
        List<DailyModelUsageItem> DailyModelUsage);

    [HttpGet]
    [AccessOnlyAsLoggedIn]
    public GetAiUsageResponse GetAiUsage([FromQuery] int days = 30)
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return new GetAiUsageResponse(false, 0, 0, 0, [], []);
        }

        var userId = _sessionUser.UserId;
        var user = _sessionUser.User;

        var dailySummary = _aiUsageLogRepo.GetDailyUsageSummary(userId, days);
        var dailyModelUsage = _aiUsageLogRepo.GetDailyModelUsageSummary(userId, days);

        var dailySummaryItems = dailySummary.Select(s => new DailyUsageSummaryItem(
            s.Date,
            s.RequestCount,
            s.TotalTokensIn,
            s.TotalTokensOut
        )).ToList();

        var dailyModelUsageItems = dailyModelUsage.Select(s => new DailyModelUsageItem(
            s.Date,
            s.ModelId,
            s.DisplayName,
            s.RequestCount,
            s.TokensIn,
            s.TokensOut,
            _aiModelRegistry.GetTokenCostMultiplier(s.ModelId)
        )).ToList();

        return new GetAiUsageResponse(
            true,
            _tokenDeductionService.GetTotalTokenBalance(userId),
            user.SubscriptionTokensBalance,
            user.PaidTokensBalance,
            dailySummaryItems,
            dailyModelUsageItems
        );
    }
}

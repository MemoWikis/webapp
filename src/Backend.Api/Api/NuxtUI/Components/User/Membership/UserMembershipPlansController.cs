using static LimitCheck;

public class UserMembershipPlansController : ApiBaseController
{
    public readonly record struct PlanLimits(
        int MaxPrivatePageCount,
        int MaxPrivateQuestionCount,
        int MaxWishKnowledgeCount,
        int FreeWeeklyTokens,
        int SmartWeeklyTokens,
        int ExpertWeeklyTokens,
        int TokensPerA4Page);

    [HttpGet]
    public BasicLimits GetBasicLimits()
    {
        var limits = LimitCheck.GetBasicLimits();
        return limits;
    }

    [HttpGet]
    public PlanLimits GetPlanLimits()
    {
        var basicLimits = LimitCheck.GetBasicLimits();
        return new PlanLimits
        {
            MaxPrivatePageCount = basicLimits.MaxPrivatePageCount,
            MaxPrivateQuestionCount = basicLimits.MaxPrivateQuestionCount,
            MaxWishKnowledgeCount = basicLimits.MaxWishKnowledgeCount,
            FreeWeeklyTokens = TokenDeductionService.FreeWeeklyTokenLimit,
            SmartWeeklyTokens = TokenDeductionService.SmartWeeklyTokenLimit,
            ExpertWeeklyTokens = TokenDeductionService.ExpertWeeklyTokenLimit,
            TokensPerA4Page = TokenDeductionService.TokensPerA4Page
        };
    }
}
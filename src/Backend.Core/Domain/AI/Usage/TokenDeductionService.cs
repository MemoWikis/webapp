using NHibernate;

/// <summary>
/// Manages AI token usage and balance tracking for users.
/// Handles token estimation, affordability checks, and deductions from user token balances.
/// 
/// Weekly quota system:
/// - Subscribers: 10 million tokens per week
/// - Free-tier users: 1 million tokens per week
/// - Quota resets every Monday at 00:00
/// - Unused tokens don't accumulate
/// 
/// The service is model-aware and applies cost multipliers based on the AI model being used
/// (e.g., more expensive models like Claude Opus cost more tokens per actual token used).
/// 
/// Token estimation is based on conservative character-to-token ratios (~3.5 chars/token for Claude models)
/// and predefined output token estimates for different generation types.
/// </summary>
public class TokenDeductionService(ISession _session, AiModelRegistry _aiModelRegistry) : IRegisterAsInstancePerLifetime
{
    // Average characters per token for Claude models (conservative estimate)
    private const double CharactersPerToken = 3.5;

    /// <summary>
    /// Weekly quota for subscribers (10 million tokens)
    /// </summary>
    public const int SubscriberWeeklyTokenLimit = 10_000_000;

    /// <summary>
    /// Weekly quota for free tier users (1 million tokens)
    /// </summary>
    public const int FreeWeeklyTokenLimit = 1_000_000;

    /// <summary>
    /// Types of AI generation with expected output token estimates
    /// </summary>
    public enum GenerationType
    {
        /// <summary>Short page content - ~500 output tokens</summary>
        PageShort,

        /// <summary>Medium page content - ~1000 output tokens</summary>
        PageMedium,

        /// <summary>Detailed/long page content - ~2000 output tokens</summary>
        PageLong,

        /// <summary>Wiki with subpages - ~4000 output tokens</summary>
        WikiWithSubpages,

        /// <summary>Flashcard generation - ~800 output tokens (for ~3-5 cards)</summary>
        Flashcards
    }

    /// <summary>
    /// Gets the estimated output tokens for a generation type
    /// </summary>
    public static int GetEstimatedOutputTokens(GenerationType generationType)
    {
        return generationType switch
        {
            GenerationType.PageShort => 500,
            GenerationType.PageMedium => 1000,
            GenerationType.PageLong => 2000,
            GenerationType.WikiWithSubpages => 4000,
            GenerationType.Flashcards => 800,
            _ => 1000
        };
    }

    /// <summary>
    /// Estimates the number of tokens for a given text.
    /// Uses a conservative estimate based on average characters per token.
    /// </summary>
    public static int EstimateTokens(string text)
    {
        if (string.IsNullOrEmpty(text))
            return 0;

        return (int)Math.Ceiling(text.Length / CharactersPerToken);
    }

    /// <summary>
    /// Estimates total token usage (input + expected output) for an AI operation.
    /// </summary>
    public static int EstimateTotalTokenUsage(string prompt, GenerationType generationType)
    {
        var inputTokens = EstimateTokens(prompt);
        var outputTokens = GetEstimatedOutputTokens(generationType);
        return inputTokens + outputTokens;
    }

    /// <summary>
    /// Checks if user can afford the estimated token cost for a specific generation type.
    /// Uses the model's token cost multiplier.
    /// </summary>
    public bool CanAffordTokens(int userId, string modelId, string prompt, GenerationType generationType, int maxNegativeBalance = -10000)
    {
        var estimatedCost = EstimateTotalTokenUsage(prompt, generationType);
        var tokenCostMultiplier = _aiModelRegistry.GetTokenCostMultiplier(modelId);
        return CanAffordTokensInternal(userId, estimatedCost, tokenCostMultiplier, maxNegativeBalance);
    }

    /// <summary>
    /// Checks if user can afford a specific token cost with model-aware multiplier.
    /// </summary>
    public bool CanAffordTokens(int userId, string modelId, int estimatedCost, int maxNegativeBalance = -10000)
    {
        var tokenCostMultiplier = _aiModelRegistry.GetTokenCostMultiplier(modelId);
        return CanAffordTokensInternal(userId, estimatedCost, tokenCostMultiplier, maxNegativeBalance);
    }

    /// <summary>
    /// Checks if user can afford the estimated token cost (legacy method without model awareness).
    /// </summary>
    public bool CanAffordTokens(int userId, string prompt, GenerationType generationType = GenerationType.Flashcards, int maxNegativeBalance = -10000)
    {
        var estimatedCost = EstimateTotalTokenUsage(prompt, generationType);
        return CanAffordTokensInternal(userId, estimatedCost, 1m, maxNegativeBalance);
    }

    private bool CanAffordTokensInternal(int userId, int estimatedCost, decimal tokenCostMultiplier, int maxNegativeBalance)
    {
        var user = EntityCache.GetUserById(userId);
        if (user == null)
            return false;

        // Apply the token cost multiplier to the estimated cost
        var adjustedCost = (int)Math.Ceiling(estimatedCost * tokenCostMultiplier);

        // Get weekly quota based on subscription status
        var hasActiveSubscription = user.SubscriptionStartDate.HasValue && user.EndDate > DateTime.Now;
        var weeklyLimit = hasActiveSubscription ? SubscriberWeeklyTokenLimit : FreeWeeklyTokenLimit;

        // Get actual token usage this week from cache
        var tokensUsedThisWeek = user.CurrentWeekTokenUsage;

        // Calculate remaining balance
        var remainingBalance = weeklyLimit - tokensUsedThisWeek;
        var projectedBalance = remainingBalance - adjustedCost;

        // Allow if projected balance is above the max negative threshold
        return projectedBalance >= maxNegativeBalance;
    }

    /// <summary>
    /// Gets the total token usage for the current week from the cache.
    /// </summary>
    public static long GetCurrentWeekTokenUsage(int userId)
    {
        var user = EntityCache.GetUserById(userId);
        return user?.CurrentWeekTokenUsage ?? 0;
    }

    /// <summary>
    /// Deducts tokens from user balance. First tries subscription tokens, then paid tokens.
    /// Returns the actual amount deducted and from which balances.
    /// </summary>
    public TokenDeductionResult DeductTokens(int userId, int totalTokens)
    {
        return DeductTokensInternal(userId, totalTokens);
    }

    /// <summary>
    /// Deducts tokens from user balance with model-aware cost multiplier.
    /// The token cost multiplier from the model is applied to the total tokens.
    /// </summary>
    public TokenDeductionResult DeductTokens(int userId, string modelId, int inputTokens, int outputTokens)
    {
        var tokenCostMultiplier = _aiModelRegistry.GetTokenCostMultiplier(modelId);
        var totalTokens = inputTokens + outputTokens;
        var adjustedTokens = (int)Math.Ceiling(totalTokens * tokenCostMultiplier);

        Log.Information(
            "TokenDeduction: Model {ModelId} with multiplier {Multiplier}x - Raw tokens: {RawTokens}, Adjusted: {AdjustedTokens}",
            modelId, tokenCostMultiplier, totalTokens, adjustedTokens);

        return DeductTokensInternal(userId, adjustedTokens);
    }

    private TokenDeductionResult DeductTokensInternal(int userId, int totalTokens)
    {
        if (totalTokens <= 0)
        {
            return new TokenDeductionResult(true, 0, 0, 0);
        }

        var user = EntityCache.GetUserById(userId);
        if (user == null)
        {
            Log.Warning("TokenDeductionService: User {UserId} not found in cache", userId);
            return new TokenDeductionResult(false, 0, 0, totalTokens);
        }

        int tokensRemaining = totalTokens;
        int deductedFromSubscription = 0;
        int deductedFromPaid = 0;

        // First, deduct from subscription tokens
        if (user.SubscriptionTokensBalance > 0 && tokensRemaining > 0)
        {
            deductedFromSubscription = Math.Min(user.SubscriptionTokensBalance, tokensRemaining);
            tokensRemaining -= deductedFromSubscription;
        }

        // Then, deduct from paid tokens if needed
        if (tokensRemaining > 0 && user.PaidTokensBalance > 0)
        {
            deductedFromPaid = Math.Min(user.PaidTokensBalance, tokensRemaining);
            tokensRemaining -= deductedFromPaid;
        }

        // Update database
        if (deductedFromSubscription > 0 || deductedFromPaid > 0)
        {
            var success = UpdateUserTokenBalances(userId, deductedFromSubscription, deductedFromPaid);
            if (!success)
            {
                Log.Error("TokenDeductionService: Failed to deduct tokens for user {UserId}", userId);
                return new TokenDeductionResult(false, 0, 0, totalTokens);
            }
        }

        return new TokenDeductionResult(
            true,
            deductedFromSubscription,
            deductedFromPaid,
            tokensRemaining);
    }

    private bool UpdateUserTokenBalances(int userId, int subscriptionDeduction, int paidDeduction)
    {
        using var transaction = _session.BeginTransaction();
        try
        {
            // Use optimistic locking: UPDATE only if balances are sufficient
            // This prevents race conditions where multiple concurrent requests could overdraw the balance
            var query = _session.CreateSQLQuery(@"
                UPDATE user SET 
                    SubscriptionTokensBalance = SubscriptionTokensBalance - :subscriptionDeduction,
                    PaidTokensBalance = PaidTokensBalance - :paidDeduction
                WHERE Id = :userId
                AND SubscriptionTokensBalance >= :subscriptionDeduction
                AND PaidTokensBalance >= :paidDeduction");

            query.SetParameter("subscriptionDeduction", subscriptionDeduction);
            query.SetParameter("paidDeduction", paidDeduction);
            query.SetParameter("userId", userId);

            var rowsAffected = query.ExecuteUpdate();

            if (rowsAffected == 0)
            {
                Log.Warning(
                    "TokenDeductionService: Insufficient balance for user {UserId} - attempted to deduct {SubscriptionDeduction} subscription tokens and {PaidDeduction} paid tokens",
                    userId, subscriptionDeduction, paidDeduction);
                transaction.Rollback();
                return false;
            }

            transaction.Commit();

            // Only update cache after successful DB commit
            var userCacheItem = EntityCache.GetUserById(userId);
            if (userCacheItem != null)
            {
                userCacheItem.SubscriptionTokensBalance -= subscriptionDeduction;
                userCacheItem.PaidTokensBalance -= paidDeduction;
            }

            Log.Information(
                "TokenDeduction: User {UserId} - Deducted {SubscriptionDeduction} subscription tokens, {PaidDeduction} paid tokens",
                userId, subscriptionDeduction, paidDeduction);

            return true;
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            Log.Error(exception, "Failed to update token balances for user {UserId}", userId);
            return false;
        }
    }

    /// <summary>
    /// Checks if user has enough tokens for a given operation.
    /// Uses the dynamic weekly quota system with cached usage.
    /// </summary>
    public bool HasEnoughTokens(int userId, int requiredTokens)
    {
        var user = EntityCache.GetUserById(userId);
        if (user == null)
        {
            return false;
        }

        // Get weekly quota based on subscription status
        var hasActiveSubscription = user.SubscriptionStartDate.HasValue && user.EndDate > DateTime.Now;
        var weeklyLimit = hasActiveSubscription ? SubscriberWeeklyTokenLimit : FreeWeeklyTokenLimit;

        // Get actual token usage this week from cache
        var tokensUsedThisWeek = user.CurrentWeekTokenUsage;

        // Calculate remaining balance
        var remainingBalance = weeklyLimit - tokensUsedThisWeek;
        return remainingBalance >= requiredTokens;
    }

    /// <summary>
    /// Gets the total remaining token balance for a user (weekly limit - used this week from cache).
    /// </summary>
    public int GetTotalTokenBalance(int userId)
    {
        var user = EntityCache.GetUserById(userId);
        if (user == null)
        {
            return 0;
        }

        // Get weekly quota based on subscription status
        var hasActiveSubscription = user.SubscriptionStartDate.HasValue && user.EndDate > DateTime.Now;
        var weeklyLimit = hasActiveSubscription ? SubscriberWeeklyTokenLimit : FreeWeeklyTokenLimit;

        // Get actual token usage this week from cache
        var tokensUsedThisWeek = user.CurrentWeekTokenUsage;

        // Return remaining balance
        return Math.Max(0, (int)(weeklyLimit - tokensUsedThisWeek));
    }
}

public record struct TokenDeductionResult(
    bool Success,
    int DeductedFromSubscription,
    int DeductedFromPaid,
    int TokensNotCovered);

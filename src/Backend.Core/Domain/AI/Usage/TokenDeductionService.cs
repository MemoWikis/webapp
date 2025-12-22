using NHibernate;

public class TokenDeductionService(ISession _session, AiModelRegistry _aiModelRegistry) : IRegisterAsInstancePerLifetime
{
    // Average characters per token for Claude models (conservative estimate)
    private const double CharactersPerToken = 3.5;

    /// <summary>
    /// Types of AI generation with expected output token estimates
    /// </summary>
    public enum GenerationType
    {
        /// <summary>Short page content - ~500 output tokens</summary>
        PageShort,
        /// <summary>Medium page content - ~1500 output tokens</summary>
        PageMedium,
        /// <summary>Detailed/long page content - ~3000 output tokens</summary>
        PageLong,
        /// <summary>Wiki with subpages - ~8000 output tokens</summary>
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
            GenerationType.PageMedium => 1500,
            GenerationType.PageLong => 3000,
            GenerationType.WikiWithSubpages => 8000,
            GenerationType.Flashcards => 800,
            _ => 1500
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

        var currentBalance = user.SubscriptionTokensBalance + user.PaidTokensBalance;
        var projectedBalance = currentBalance - adjustedCost;

        // Allow if projected balance is above the max negative threshold
        return projectedBalance >= maxNegativeBalance;
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
            UpdateUserTokenBalances(userId, deductedFromSubscription, deductedFromPaid);
        }

        return new TokenDeductionResult(
            true,
            deductedFromSubscription,
            deductedFromPaid,
            tokensRemaining);
    }

    private void UpdateUserTokenBalances(int userId, int subscriptionDeduction, int paidDeduction)
    {
        try
        {
            var query = _session.CreateSQLQuery(@"
                UPDATE user SET 
                    SubscriptionTokensBalance = SubscriptionTokensBalance - :subscriptionDeduction,
                    PaidTokensBalance = PaidTokensBalance - :paidDeduction
                WHERE Id = :userId");

            query.SetParameter("subscriptionDeduction", subscriptionDeduction);
            query.SetParameter("paidDeduction", paidDeduction);
            query.SetParameter("userId", userId);
            query.ExecuteUpdate();

            _session.Flush();

            // Update cache
            var userCacheItem = EntityCache.GetUserById(userId);
            if (userCacheItem != null)
            {
                userCacheItem.SubscriptionTokensBalance -= subscriptionDeduction;
                userCacheItem.PaidTokensBalance -= paidDeduction;
            }

            Log.Information(
                "TokenDeduction: User {UserId} - Deducted {SubscriptionDeduction} subscription tokens, {PaidDeduction} paid tokens",
                userId, subscriptionDeduction, paidDeduction);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to update token balances for user {UserId}", userId);
        }
    }

    /// <summary>
    /// Checks if user has enough tokens for a given operation
    /// </summary>
    public bool HasEnoughTokens(int userId, int requiredTokens)
    {
        var user = EntityCache.GetUserById(userId);
        if (user == null)
        {
            return false;
        }

        var totalBalance = user.SubscriptionTokensBalance + user.PaidTokensBalance;
        return totalBalance >= requiredTokens;
    }

    /// <summary>
    /// Gets the total token balance for a user (subscription + paid)
    /// </summary>
    public int GetTotalTokenBalance(int userId)
    {
        var user = EntityCache.GetUserById(userId);
        if (user == null)
        {
            return 0;
        }

        return user.SubscriptionTokensBalance + user.PaidTokensBalance;
    }
}

public record struct TokenDeductionResult(
    bool Success,
    int DeductedFromSubscription,
    int DeductedFromPaid,
    int TokensNotCovered);

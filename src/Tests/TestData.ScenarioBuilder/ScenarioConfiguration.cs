/// <summary>
/// Immutable settings for the test scenario.
/// </summary>
public sealed record ScenarioConfiguration
(
    DefaultUserDefinition[] DefaultUsers,
    int TopLevelPagesPerUser,
    int MaximumPageNestingDepth,
    int QuestionsPerPageForNormalUsers,
    int QuestionsPerPageForContributor,
    int SeedForRandom,
    DateTime BaseDate
)
{
    public DateTime Now => BaseDate == default
        ? DateTime.Now
        : BaseDate;
}
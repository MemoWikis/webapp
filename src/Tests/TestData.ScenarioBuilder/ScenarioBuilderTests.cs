[TestFixture]
internal class ScenarioBuilderTests : BaseTestHarness
{
    [Test]
    public async Task BuildAsync_Creates_Deterministic_Scenario()
    {
        // Arrange
        var configuration = new ScenarioConfiguration(
            UserCount: 3,
            TopLevelPagesPerUser: 3,
            MaximumPageNestingDepth: 2,
            QuestionsPerPageForNormalUsers: 2,
            QuestionsPerPageForContributor: 5,
            SeedForRandom: 12345,
            BaseDate: new DateTime(2025, 1, 1, 0, 0, 0)
        );
        var performanceLogger = new PerformanceLogger(enabled: true);
        var scenarioBuilder = new ScenarioBuilder(_testHarness, configuration, performanceLogger);

        // Act
        await scenarioBuilder.BuildAsync();

        // Assert
        await Verify(new
        {
            allUsers = await _testHarness.DbData.AllUsersSummaryAsync(),
            allPages = await _testHarness.DbData.AllPagesSummaryAsync(),
            allQuestions = await _testHarness.DbData.AllQuestionsSummaryAsync()
        });
    }
}

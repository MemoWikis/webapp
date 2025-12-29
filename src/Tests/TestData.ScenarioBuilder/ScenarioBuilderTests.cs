/// <summary>
/// Is used to create scenarios. 
/// </summary>
[TestFixture]
[Category(TestCategories.ScenarioBuild)]
internal class ScenarioBuilderTests : BaseTestHarness
{
    public ScenarioBuilderTests()
    {
        _skipDefaultUsers = true;
    }

    [Test]
    public async Task Micro_Scenario()
    {
        await ScenarioDumpManager.CreateDumpAsync("micro_for_testing");
    }

    [Test]
    public async Task Deterministic_Tiny_Scenario()
    {
        await ClearData();

        // Arrange
        var configuration = new ScenarioConfiguration(
            UserCount: 1,
            TopLevelPagesPerUser: 1,
            MaximumPageNestingDepth: 2,
            QuestionsPerPageForNormalUsers: 2,
            QuestionsPerPageForContributor: 2,
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

        await ScenarioDumpManager.CreateDumpAsync(ScenarioDumpConstants.TagTiny);
    }

    [Test]
    public async Task Deterministic_Mid_Scenario()
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

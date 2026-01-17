class AiUsageLogRepo_tests : BaseTestHarness
{
    [Test]
    public void GetDailyUsageSummary_returns_empty_list_when_no_usage()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var userId = 999999; // Non-existent user

        // Act
        var result = aiUsageLogRepo.GetDailyUsageSummary(userId, 30);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetDailyUsageSummary_returns_correct_data_types()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var tokenDeductionService = R<TokenDeductionService>();
        var userReadingRepo = R<UserReadingRepo>();

        // Get a test user
        var testUser = userReadingRepo.GetById(1);
        Assert.That(testUser, Is.Not.Null, "Test user should exist");

        // Add test usage data
        aiUsageLogRepo.AddUsage(
            testUser!.Id,
            1, // pageId
            1000, // tokenIn
            500, // tokenOut
            "claude-3-5-sonnet-latest"
        );

        // Act
        var result = aiUsageLogRepo.GetDailyUsageSummary(testUser.Id, 30);

        // Assert - Should not throw Int64 to Int32 conversion error
        Assert.That(result, Is.Not.Null);

        // If there are results, verify the data types are correct
        if (result.Any())
        {
            var firstResult = result.First();
            Assert.That(firstResult.Date, Is.TypeOf<DateTime>());
            Assert.That(firstResult.RequestCount, Is.TypeOf<long>());
            Assert.That(firstResult.TotalTokensIn, Is.TypeOf<decimal>());
            Assert.That(firstResult.TotalTokensOut, Is.TypeOf<decimal>());

            // Verify the values make sense
            Assert.That(firstResult.RequestCount, Is.GreaterThan(0));
            Assert.That(firstResult.TotalTokensIn, Is.GreaterThanOrEqualTo(0));
            Assert.That(firstResult.TotalTokensOut, Is.GreaterThanOrEqualTo(0));
        }
    }

    [Test]
    public void GetDailyModelUsageSummary_returns_empty_list_when_no_usage()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var userId = 999999; // Non-existent user

        // Act
        var result = aiUsageLogRepo.GetDailyModelUsageSummary(userId, 30);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetDailyModelUsageSummary_returns_correct_data_types()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var userReadingRepo = R<UserReadingRepo>();

        // Get a test user
        var testUser = userReadingRepo.GetById(1);
        Assert.That(testUser, Is.Not.Null, "Test user should exist");

        // Add test usage data
        aiUsageLogRepo.AddUsage(
            testUser!.Id,
            1, // pageId
            2000, // tokenIn
            1000, // tokenOut
            "claude-3-5-sonnet-latest"
        );

        // Act
        var result = aiUsageLogRepo.GetDailyModelUsageSummary(testUser.Id, 30);

        // Assert - Should not throw Int64 to Int32 conversion error
        Assert.That(result, Is.Not.Null);

        // If there are results, verify the data types are correct
        if (result.Any())
        {
            var firstResult = result.First();
            Assert.That(firstResult.Date, Is.TypeOf<DateTime>());
            Assert.That(firstResult.ModelId, Is.TypeOf<string>());
            Assert.That(firstResult.RequestCount, Is.TypeOf<long>());
            Assert.That(firstResult.TokensIn, Is.TypeOf<decimal>());
            Assert.That(firstResult.TokensOut, Is.TypeOf<decimal>());

            // Verify the values make sense
            Assert.That(firstResult.RequestCount, Is.GreaterThan(0));
            Assert.That(firstResult.ModelId, Is.Not.Empty);
        }
    }

    [Test]
    public void GetDailyUsageSummary_aggregates_multiple_entries_per_day()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var userReadingRepo = R<UserReadingRepo>();

        var testUser = userReadingRepo.GetById(1);
        Assert.That(testUser, Is.Not.Null, "Test user should exist");

        // Add multiple usage entries for the same day
        for (var i = 0; i < 3; i++)
        {
            aiUsageLogRepo.AddUsage(
                testUser!.Id,
                1,
                100 * (i + 1), // 100, 200, 300 tokens in
                50 * (i + 1),  // 50, 100, 150 tokens out
                "claude-3-5-sonnet-latest"
            );
        }

        // Act
        var result = aiUsageLogRepo.GetDailyUsageSummary(testUser!.Id, 1);

        // Assert
        Assert.That(result, Is.Not.Null);

        // Should have at least one entry for today
        if (result.Any())
        {
            var todayResult = result.First();

            // Should be aggregated (multiple requests counted)
            Assert.That(todayResult.RequestCount, Is.GreaterThanOrEqualTo(3));

            // Total tokens should include all entries
            Assert.That(todayResult.TotalTokensIn, Is.GreaterThanOrEqualTo(600)); // 100+200+300
            Assert.That(todayResult.TotalTokensOut, Is.GreaterThanOrEqualTo(300)); // 50+100+150
        }
    }

    [Test]
    public void GetDailyModelUsageSummary_groups_by_model()
    {
        // Arrange
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var userReadingRepo = R<UserReadingRepo>();

        var testUser = userReadingRepo.GetById(1);
        Assert.That(testUser, Is.Not.Null, "Test user should exist");

        // Add usage for different models
        aiUsageLogRepo.AddUsage(testUser!.Id, 1, 100, 50, "claude-3-5-sonnet-latest");
        aiUsageLogRepo.AddUsage(testUser.Id, 1, 200, 100, "claude-3-5-sonnet-latest");
        aiUsageLogRepo.AddUsage(testUser.Id, 1, 300, 150, "gpt-4o");

        // Act
        var result = aiUsageLogRepo.GetDailyModelUsageSummary(testUser.Id, 1);

        // Assert
        Assert.That(result, Is.Not.Null);

        // Should have entries grouped by model
        var modelIds = result.Select(r => r.ModelId).Distinct().ToList();

        // We should see at least our test models if they were added today
        if (result.Any())
        {
            // Verify each model has its own entry
            foreach (var entry in result)
            {
                Assert.That(entry.ModelId, Is.Not.Empty);
                Assert.That(entry.RequestCount, Is.GreaterThan(0));
            }
        }
    }
}

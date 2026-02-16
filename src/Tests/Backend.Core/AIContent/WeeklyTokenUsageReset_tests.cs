class WeeklyTokenUsageReset_tests
{
    [Test]
    public void CurrentWeekTokenUsage_returns_set_value_within_same_week()
    {
        var userCacheItem = new UserCacheItem();
        userCacheItem.CurrentWeekTokenUsage = 5000;

        Assert.That(userCacheItem.CurrentWeekTokenUsage, Is.EqualTo(5000));
    }

    [Test]
    public void CurrentWeekTokenUsage_resets_to_zero_when_week_boundary_crossed()
    {
        var userCacheItem = new UserCacheItem();
        userCacheItem.CurrentWeekTokenUsage = 15000;

        // Simulate that the usage was set in a previous week
        userCacheItem.CurrentWeekTokenUsageWeekStart = UserCacheItem.GetCurrentWeekStart().AddDays(-7);

        Assert.That(userCacheItem.CurrentWeekTokenUsage, Is.EqualTo(0));
    }

    [Test]
    public void CurrentWeekTokenUsage_tracks_new_usage_after_week_reset()
    {
        var userCacheItem = new UserCacheItem();
        userCacheItem.CurrentWeekTokenUsage = 15000;

        // Simulate a previous week
        userCacheItem.CurrentWeekTokenUsageWeekStart = UserCacheItem.GetCurrentWeekStart().AddDays(-7);

        // Read triggers the reset
        Assert.That(userCacheItem.CurrentWeekTokenUsage, Is.EqualTo(0));

        // New usage in the current week should be tracked
        userCacheItem.CurrentWeekTokenUsage += 3000;

        Assert.That(userCacheItem.CurrentWeekTokenUsage, Is.EqualTo(3000));
    }

    [Test]
    public void CurrentWeekTokenUsage_does_not_reset_within_same_week()
    {
        var userCacheItem = new UserCacheItem();
        userCacheItem.CurrentWeekTokenUsage = 5000;

        // Week start should be set to current week
        Assert.That(userCacheItem.CurrentWeekTokenUsageWeekStart, Is.EqualTo(UserCacheItem.GetCurrentWeekStart()));

        // Reading multiple times should not reset
        var firstRead = userCacheItem.CurrentWeekTokenUsage;
        var secondRead = userCacheItem.CurrentWeekTokenUsage;

        Assert.That(firstRead, Is.EqualTo(5000));
        Assert.That(secondRead, Is.EqualTo(5000));
    }

    [Test]
    public void CurrentWeekTokenUsage_resets_after_multiple_weeks()
    {
        var userCacheItem = new UserCacheItem();
        userCacheItem.CurrentWeekTokenUsage = 20000;

        // Simulate usage from 3 weeks ago
        userCacheItem.CurrentWeekTokenUsageWeekStart = UserCacheItem.GetCurrentWeekStart().AddDays(-21);

        Assert.That(userCacheItem.CurrentWeekTokenUsage, Is.EqualTo(0));
    }

    [Test]
    public void GetCurrentWeekStart_returns_monday()
    {
        var weekStart = UserCacheItem.GetCurrentWeekStart();

        Assert.That(weekStart.DayOfWeek, Is.EqualTo(DayOfWeek.Monday));
        Assert.That(weekStart.TimeOfDay, Is.EqualTo(TimeSpan.Zero));
        Assert.That(weekStart, Is.LessThanOrEqualTo(DateTime.Today));
    }

    [Test]
    public void Quota_not_depleted_after_week_boundary_crossing()
    {
        var userCacheItem = new UserCacheItem();

        // Simulate: user had full quota used last week
        userCacheItem.CurrentWeekTokenUsage = TokenDeductionService.FreeWeeklyTokenLimit;

        // Simulate that usage was from last week
        userCacheItem.CurrentWeekTokenUsageWeekStart = UserCacheItem.GetCurrentWeekStart().AddDays(-7);

        // After week boundary, usage should be 0 → quota should NOT be depleted
        var tokensUsedThisWeek = userCacheItem.CurrentWeekTokenUsage;
        var remainingBalance = TokenDeductionService.FreeWeeklyTokenLimit - tokensUsedThisWeek;

        Assert.That(tokensUsedThisWeek, Is.EqualTo(0));
        Assert.That(remainingBalance, Is.EqualTo(TokenDeductionService.FreeWeeklyTokenLimit));
    }
}

/// <summary>
/// Test to demonstrate ExtendedUserCache conversion from session-based to global cache
/// </summary>
class ExtendedUserCache_conversion_tests : BaseTestHarness
{
    [Test]
    public async Task Should_store_and_retrieve_extended_user_from_global_cache()
    {
        // Arrange
        await ReloadCaches();
        var extendedUserCache = R<ExtendedUserCache>();
        var userId = 1;

        // Act - Add user to extended cache (this will create on-demand)
        var extendedUser = extendedUserCache.Add(userId);

        // Assert - Verify user is stored in global EntityCache
        var retrievedUser = EntityCache.GetExtendedUserByIdNullable(userId);
        var retrievedAgain = extendedUserCache.GetUser(userId);

        await Verify(new
        {
            UserId = userId,
            ExtendedUser = extendedUser,
            RetrievedUser = retrievedUser,
            UserStoredInCache = retrievedUser != null,
            UserIdMatches = retrievedUser?.Id == userId,
            SameInstanceReturned = ReferenceEquals(retrievedAgain, extendedUser)
        });
    }

    [Test]
    public async Task Should_persist_across_multiple_requests_without_expiration()
    {
        // Arrange
        await ReloadCaches();
        var extendedUserCache = R<ExtendedUserCache>();
        var userId = 1;

        // Create a test page first
        var context = NewPageContext();
        var creator = new User { Id = userId };

        context.Add("testPage", creator: creator, isWiki: true).Persist();
        var testPage = context.All.ByName("testPage");


        var originalUser = extendedUserCache.Add(userId);
        originalUser.PageValuations.TryAdd(testPage.Id, new PageValuation { PageId = testPage.Id, UserId = userId });

        await ReloadCaches();

        // Simulate new request/session by getting fresh instance
        var newCacheInstance = R<ExtendedUserCache>();
        var retrievedUser = newCacheInstance.GetUser(userId);

        // Assert - Data persists without expiration
        await Verify(new
        {
            UserId = userId,
            TestPageId = testPage.Id,
            OriginalUser = originalUser,
            RetrievedUser = retrievedUser,
            UserPersists = retrievedUser != null,
            PageValuationsPersist = retrievedUser?.PageValuations.ContainsKey(testPage.Id) == true,
            PageValuationDataCorrect = retrievedUser?.PageValuations[testPage.Id]?.PageId == testPage.Id
        });
    }

    [Test]
    public async Task Should_initialize_empty_cache_on_first_access()
    {
        // Arrange
        await ReloadCaches();

        // Act - Access ExtendedUsers for first time
        var allExtendedUsers = EntityCache.GetAllExtendedUsers();

        // Assert - Should return empty collection, not null
        await Verify(new
        {
            AllExtendedUsers = allExtendedUsers,
            CollectionNotNull = allExtendedUsers != null,
            CollectionEmpty = allExtendedUsers?.Count == 0
        });
    }

    [Test]
    public async Task Should_support_concurrent_access()
    {
        // Arrange
        await ReloadCaches();
        var extendedUserCache = R<ExtendedUserCache>();
        var userIds = new[] { 1, 2, 3, 4, 5 };

        // Act - Add multiple users concurrently
        Parallel.ForEach(userIds, userId =>
        {
            var user = extendedUserCache.Add(userId);
            user.PageValuations.TryAdd(userId * 100, new PageValuation
            {
                PageId = userId * 100,
                UserId = userId
            });
        });

        // Assert - All users should be present
        var allUsers = EntityCache.GetAllExtendedUsers();
        var userResults = userIds.Select(userId => new
        {
            UserId = userId,
            User = EntityCache.GetExtendedUserByIdNullable(userId),
            UserExists = EntityCache.GetExtendedUserByIdNullable(userId) != null,
            HasPageValuation = EntityCache.GetExtendedUserByIdNullable(userId)?.PageValuations.ContainsKey(userId * 100) == true
        }).ToList();

        await Verify(new
        {
            UserIds = userIds,
            AllUsers = allUsers,
            TotalUsersCount = allUsers.Count,
            ExpectedCount = userIds.Length,
            UserResults = userResults
        });
    }
}

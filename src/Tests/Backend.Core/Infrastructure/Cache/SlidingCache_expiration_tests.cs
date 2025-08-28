/// <summary>
/// Test to verify SlidingCache expiration behavior and active user enumeration
/// </summary>
class SlidingCache_expiration_tests : BaseTestHarness
{
    [Test]
    public async Task Should_expire_users_with_different_expiration_times()
    {
        // Arrange
        await ReloadCaches();

        var userId1 = 1;
        var userId2 = 2;

        // Create test users and their wikis
        var context = NewPageContext();
        var user1 = new User { Id = userId1 };
        var user2 = new User { Id = userId2 };

        context
            .Add("testWiki1", creator: user1, isWiki: true)
            .Add("testWiki2", creator: user2, isWiki: true)
            .Persist();

        // Create test extended users
        var extendedUser1 = new ExtendedUserCacheItem { Id = userId1 };
        var extendedUser2 = new ExtendedUserCacheItem { Id = userId2 };

        // Act - Add users with different expiration times
        SlidingCache.AddOrUpdate(extendedUser1, 15); // 15 seconds expiration
        SlidingCache.AddOrUpdate(extendedUser2, 300); // 300 seconds (5 minutes) expiration

        // Get initial active users count
        var initialActiveUsers = SlidingCache.GetAllActiveExtendedUsers();

        // Wait for first user to expire (16 seconds to be safe)
        await Task.Delay(16000);

        // Get active users after expiration
        var activeUsersAfterExpiration = SlidingCache.GetAllActiveExtendedUsers();

        // Assert - Only the second user should remain active
        await Verify(new
        {
            InitialActiveUsers = initialActiveUsers,
            ActiveUsersAfterExpiration = activeUsersAfterExpiration
        });
    }

    [Test]
    public async Task Should_extend_expiration_on_cache_access()
    {
        // Arrange
        await ReloadCaches();

        var userId = 3;

        // Create test user and wiki
        var context = NewPageContext();
        var user = new User { Id = userId };

        context.Add("testWiki3", creator: user, isWiki: true).Persist();

        var extendedUser = new ExtendedUserCacheItem { Id = userId };

        // Act - Add user with short expiration
        SlidingCache.AddOrUpdate(extendedUser, 10); // 10 seconds expiration

        // Wait 5 seconds, then access the user (should extend expiration)
        await Task.Delay(5000);
        var accessedUser = SlidingCache.GetExtendedUserByIdNullable(userId);

        // Wait another 8 seconds (total 13 seconds, but access should have extended it)
        await Task.Delay(8000);
        var activeUsers = SlidingCache.GetAllActiveExtendedUsers();

        // Assert - User should still be active due to sliding expiration
        await Verify(new { ActiveUsers = activeUsers });
    }

    [Test]
    public async Task Should_remove_users_from_tracking_when_explicitly_removed()
    {
        // Arrange
        await ReloadCaches();

        var userId1 = 1;
        var userId2 = 2;

        // Create test users and their wikis
        var context = NewPageContext();
        var user1 = new User { Id = userId1 };
        var user2 = new User { Id = userId2 };

        context
            .Add("testWiki1", creator: user1, isWiki: true)
            .Add("testWiki2", creator: user2, isWiki: true)
            .Persist();

        // Create test extended users
        var extendedUser1 = new ExtendedUserCacheItem { Id = userId1 };
        var extendedUser2 = new ExtendedUserCacheItem { Id = userId2 };

        // Act - Add users with different expiration times
        SlidingCache.AddOrUpdate(extendedUser1);
        SlidingCache.AddOrUpdate(extendedUser2);

        // Get initial active users count
        var initialActiveUsers = SlidingCache.GetAllActiveExtendedUsers();

        SlidingCache.RemoveExtendedUser(userId1);

        // Get active users after expiration
        var activeUsersAfterExpiration = SlidingCache.GetAllActiveExtendedUsers();

        // Assert - Only the second user should remain active
        await Verify(new
        {
            InitialActiveUsers = initialActiveUsers,
            ActiveUsersAfterExpiration = activeUsersAfterExpiration
        });
    }
}
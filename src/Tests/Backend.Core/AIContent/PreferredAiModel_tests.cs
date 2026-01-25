class PreferredAiModel_tests : BaseTestHarness
{
    [Test]
    public void User_PreferredAiModelId_can_be_set_and_persisted()
    {
        // Arrange
        var userReadingRepo = R<UserReadingRepo>();
        var userWritingRepo = R<UserWritingRepo>();
        
        var testUser = userReadingRepo.GetById(1);
        Assert.That(testUser, Is.Not.Null, "Test user should exist");
        
        var originalPreferredModel = testUser!.PreferredAiModelId;
        var newPreferredModel = "claude-3-5-sonnet-latest";

        // Act
        testUser.PreferredAiModelId = newPreferredModel;
        userWritingRepo.Update(testUser);
        
        // Re-read from database
        var updatedUser = userReadingRepo.GetById(1);

        // Assert
        Assert.That(updatedUser!.PreferredAiModelId, Is.EqualTo(newPreferredModel));
        
        // Cleanup - restore original value
        testUser.PreferredAiModelId = originalPreferredModel;
        userWritingRepo.Update(testUser);
    }

    [Test]
    public void User_PreferredAiModelId_defaults_to_null()
    {
        // Arrange
        var userReadingRepo = R<UserReadingRepo>();
        
        // Act - Get a fresh user that hasn't set a preference
        var users = userReadingRepo.GetAll();
        var userWithoutPreference = users.FirstOrDefault(u => u.PreferredAiModelId == null);

        // Assert
        Assert.That(userWithoutPreference, Is.Not.Null, "There should be at least one user without a preferred model");
        Assert.That(userWithoutPreference!.PreferredAiModelId, Is.Null);
    }

    [Test]
    public void UserCacheItem_includes_PreferredAiModelId()
    {
        // Arrange
        var userReadingRepo = R<UserReadingRepo>();
        var userWritingRepo = R<UserWritingRepo>();
        
        var testUser = userReadingRepo.GetById(1);
        Assert.That(testUser, Is.Not.Null, "Test user should exist");
        
        var testModelId = "test-model-for-cache";
        var originalPreferredModel = testUser!.PreferredAiModelId;

        // Act
        testUser.PreferredAiModelId = testModelId;
        userWritingRepo.Update(testUser);
        EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(testUser));
        
        var cachedUser = EntityCache.GetUserById(testUser.Id);

        // Assert
        Assert.That(cachedUser, Is.Not.Null);
        Assert.That(cachedUser!.PreferredAiModelId, Is.EqualTo(testModelId));
        
        // Cleanup
        testUser.PreferredAiModelId = originalPreferredModel;
        userWritingRepo.Update(testUser);
        EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(testUser));
    }
}

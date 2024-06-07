public class HideOrShowTextTests : BaseTest
{
    [Test]
    public void TestIsHide_text_value_cache_and_database_consistency()
    {
        //Arrange
        //visibleTopic
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };
        var contextCategory = ContextCategory.New(false);
        var publicTopicName = "category1";
        var publicTopic = contextCategory
            .Add(publicTopicName, creator: creator)
            .Persist()
            .GetTopicByName(publicTopicName);


        //NotVisibleTopic
        var creator1 = new User { Name = "Daniel" };
        ContextUser.New(R<UserWritingRepo>()).Add(creator1).Persist();
        var privateTopicName = "category2";
        var privateTopic = contextCategory
            .Add(privateTopicName, creator: creator1, visibility: CategoryVisibility.Owner)
            .Persist()
            .GetTopicByName(privateTopicName);


        //Act
        var resultNotVisibleTopic = R<CategoryUpdater>().HideOrShowTopicText(true, privateTopic.Id);
        var resultVisibleTopic = R<CategoryUpdater>().HideOrShowTopicText(true, publicTopic.Id);

        var dbCategory = R<CategoryRepository>().GetById(publicTopic.Id);
        var cacheCategory = EntityCache.GetCategory(publicTopic.Id);
        //Assert
        Assert.NotNull(dbCategory);
        Assert.NotNull(cacheCategory);
        Assert.True(resultVisibleTopic);
        Assert.True(dbCategory.TextIsHidden);
        Assert.True(cacheCategory.TextIsHidden);

        Assert.NotNull(privateTopic);
        Assert.AreEqual(privateTopic.Visibility, CategoryVisibility.Owner);
        Assert.AreNotEqual(privateTopic.Creator.Id, sessionUser.UserId);
        Assert.False(resultNotVisibleTopic);
    }
}


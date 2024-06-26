﻿public class HideOrShowTextTests : BaseTest
{
    [Test]
    public void Ensure_TextIsHidden_consistency_between_cache_and_db()
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
        var resultVisibleTopic = R<CategoryUpdater>().HideOrShowTopicText(hideText: true, publicTopic.Id);

        var dbCategory = R<CategoryRepository>().GetById(publicTopic.Id);
        var cacheCategory = EntityCache.GetCategory(publicTopic.Id);
        //Assert
        Assert.NotNull(dbCategory);
        Assert.NotNull(cacheCategory);
        Assert.True(resultVisibleTopic);
        Assert.True(dbCategory.TextIsHidden);
        Assert.True(cacheCategory.TextIsHidden);

        var ex = Assert.Throws<AccessViolationException>(() => R<CategoryUpdater>().HideOrShowTopicText(hideText: true, privateTopic.Id));
        Assert.That(ex.Message, Is.EqualTo($"{nameof(CategoryUpdater.HideOrShowTopicText)}: No permission for user"));
    }
}



using TrueOrFalse.Domain.Category.Ops;

namespace TrueOrFalse.Tests8._0._2_Domain.Topics
{
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
            var categoryName = "category1";
            var visibletopic = contextCategory
                .Add(categoryName, creator: creator)
                .Persist()
                .GetTopicByName(categoryName);


            //NotVisibleTopic
            var creator1 = new User { Name = "Daniel" };
            ContextUser.New(R<UserWritingRepo>()).Add(creator1).Persist();
            var categoryNameNotVisible = "category2";
            var notVisibletopic = contextCategory
                .Add(categoryNameNotVisible, creator: creator1, visibility: CategoryVisibility.Owner)
                .Persist()
                .GetTopicByName(categoryNameNotVisible);


            //Act
            var resultNotVisibleTopic = R<CategoryUpdater>().HideOrShowTopicText(true, notVisibletopic.Id);
            var resultVisibleTopic = R<CategoryUpdater>().HideOrShowTopicText(true, visibletopic.Id);

            var dbCategory = R<CategoryRepository>().GetById(visibletopic.Id);
            var cacheCategory = EntityCache.GetCategory(visibletopic.Id);
            //Assert
            Assert.NotNull(dbCategory);
            Assert.NotNull(cacheCategory);
            Assert.True(resultVisibleTopic);
            Assert.True(dbCategory.IsHideText);
            Assert.True(cacheCategory.IsHideText);

            Assert.NotNull(notVisibletopic);
            Assert.AreEqual(notVisibletopic.Visibility, CategoryVisibility.Owner);
            Assert.AreNotEqual(notVisibletopic.Creator.Id, sessionUser.UserId);
            Assert.False(resultNotVisibleTopic);
        }
    }
}

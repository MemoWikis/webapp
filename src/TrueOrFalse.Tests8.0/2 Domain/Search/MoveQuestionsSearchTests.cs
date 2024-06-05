using Microsoft.AspNetCore.Http;

namespace TrueOrFalse.Tests8._0._2_Domain.Search
{
    public class MoveQuestionsSearchTests : BaseTest
    {
        [Test]
        public async Task All_questions_private()
        {
            //Arrange
            var sessionUser = R<SessionUser>();
            var creator = new User { Id = sessionUser.UserId };

            var contextCategory = ContextCategory.New();

            var questionNameCat3 = "category3";
            contextCategory.Add("category1", visibility: CategoryVisibility.All, creator: creator);
            contextCategory.Add("category2", visibility: CategoryVisibility.Owner, creator: creator);
            contextCategory.Add(questionNameCat3, visibility: CategoryVisibility.Owner, creator: creator);
            var questionCategory = contextCategory.GetTopicByName(questionNameCat3);
            contextCategory.Persist();

            var question = ContextQuestion.New(
                    R<QuestionWritingRepo>(),
                    R<AnswerRepo>(),
                    R<AnswerQuestion>(),
                    R<UserWritingRepo>(),
                    R<CategoryRepository>())
                .AddQuestion(creator: creator,
                    categories: new List<Category> { questionCategory },
                    persistImmediately: true,
                    questionVisibility: QuestionVisibility.Owner);
            Thread.Sleep(1000);

            //Act
            var search = R<IGlobalSearch>();
            var topics = await search.GoAllCategoriesAsync("category");
            var items = new List<SearchTopicItem>();
            new SearchHelper(R<ImageMetaDataReadingRepo>(),
                R<IHttpContextAccessor>(),
                R<QuestionReadingRepo>()).AddTopicItems(items, topics, R<PermissionCheck>(),
                creator.Id);

            //Assert
            Assert.AreEqual(topics.Categories.Count, 3);
            Assert.AreEqual(items.Count, 3);
        }


        [Test]
        public async Task One_question_is_public()
        {
            //Arrange
            var sessionUser = R<SessionUser>();
            var creator = new User { Id = sessionUser.UserId };

            var contextCategory = ContextCategory.New();

            var questionNameCat3 = "category3";
            var publicCategory = "category1";
            contextCategory.Add(publicCategory, visibility: CategoryVisibility.All, creator: creator);
            contextCategory.Add("category2", visibility: CategoryVisibility.Owner, creator: creator);
            contextCategory.Add(questionNameCat3, visibility: CategoryVisibility.Owner, creator: creator);
            var questionCategory = contextCategory.GetTopicByName(questionNameCat3);
            contextCategory.Persist();

            var question = ContextQuestion.New(
                    R<QuestionWritingRepo>(),
                    R<AnswerRepo>(),
                    R<AnswerQuestion>(),
                    R<UserWritingRepo>(),
                    R<CategoryRepository>())
                .AddQuestion(creator: creator,
                    categories: new List<Category> { questionCategory },
                    persistImmediately: true,
                    questionVisibility: QuestionVisibility.All);
            Thread.Sleep(1000);

            //Act
            var search = R<IGlobalSearch>();
            var topics = await search.GoAllCategoriesAsync("category");
            var items = new List<SearchTopicItem>();
            new SearchHelper(R<ImageMetaDataReadingRepo>(),
                R<IHttpContextAccessor>(),
                R<QuestionReadingRepo>()).AddMoveQuestionsTopics(items, topics, R<PermissionCheck>(),
                creator.Id, questionCategory.Id);

            //Assert
            Assert.AreEqual(topics.Categories.Count, 3);
            Assert.AreEqual(items.Count, 1);
            Assert.AreEqual(EntityCache.GetCategory(items.First().Id).Name, publicCategory);
            Assert.AreEqual(items.First().Visibility, (int)CategoryVisibility.All);
        }
    }
}

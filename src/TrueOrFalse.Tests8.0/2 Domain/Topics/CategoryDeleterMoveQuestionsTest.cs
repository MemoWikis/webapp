using ISession = NHibernate.ISession;

namespace TrueOrFalse.Tests8._0._2_Domain.Topics
{
    public class CategoryDeleterMoveQuestionsTest : BaseTest
    {
        [Test]
        public void Should_Move_Questions_To_Parent()
        {
            //Arrange
            var contextTopic = ContextCategory.New();
            var parentName = "parent name";
            var childName = "child name";
            var sessionUser = R<SessionUser>();
            var creator = new User { Id = sessionUser.UserId };

            var parent = contextTopic.Add(
                    parentName,
                    CategoryType.Standard,
                    creator)
                .GetTopicByName(parentName);

            var child = contextTopic.Add(childName,
                    CategoryType.Standard,
                    creator)
                .GetTopicByName(childName);

            contextTopic.Persist();
            contextTopic.AddChild(parent, child);

            var questionContext = ContextQuestion.New(
                R<QuestionWritingRepo>(),
                R<AnswerRepo>(),
                R<AnswerQuestion>(),
                R<UserWritingRepo>(),
                R<CategoryRepository>(),
                true);

            questionContext.AddQuestion("Frage1", creator: creator, categories: new List<Category> { child });

            RecycleContainerAndEntityCache();
            var catRepo = R<CategoryRepository>();
            catRepo.ClearAllItemCache();
            R<ISession>().Clear();


            var categoryDeleter = R<CategoryDeleter>();
            //Act
            var requestResult = categoryDeleter.DeleteTopic(child.Id, parent.Id);
            var allQuestions = EntityCache.GetAllQuestions();


            //Assert
            Assert.IsNotNull(requestResult);
            Assert.IsTrue(requestResult.Success);
            Assert.IsFalse(requestResult.HasChildren);
            Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
            Assert.That(parent.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        }

        [Test]
        public void DeleteCategoryWithQuestionTest()
        {
            var contextTopic = ContextCategory.New();
            var parentName = "parent name";
            var childName = "child name";
            var sessionUser = R<SessionUser>();
            var creator = new User { Id = sessionUser.UserId };

            var parent = contextTopic.Add(
                    parentName,
                    CategoryType.Standard,
                    creator)
                .GetTopicByName(parentName);

            var child = contextTopic.Add(childName,
                    CategoryType.Standard,
                    creator)
                .GetTopicByName(childName);

            contextTopic.Persist();
            contextTopic.AddChild(parent, child);

            var categoryRepo = R<CategoryRepository>();

            var questionContext = ContextQuestion.New(
                R<QuestionWritingRepo>(),
                R<AnswerRepo>(),
                R<AnswerQuestion>(),
                R<UserWritingRepo>(),
                categoryRepo,
                true);

            questionContext.AddQuestion("Frage1", creator: creator, categories: new List<Category> { parent });

            var childCacheItem = EntityCache.GetCategory(child.Id);

            var modifyRelationsForCategory =
                new ModifyRelationsForCategory(categoryRepo, R<CategoryRelationRepo>());

            ModifyRelationsEntityCacheAndDb.RemoveRelationsForCategoryDeleter(childCacheItem, creator.Id,
                modifyRelationsForCategory);

            categoryRepo.Delete(child);
        }
    }
}

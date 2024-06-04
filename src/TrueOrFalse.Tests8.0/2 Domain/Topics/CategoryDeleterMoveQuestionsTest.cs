using static TopicDataManager;
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
            var categoryDeleter = R<CategoryDeleter>();
            //Act
            categoryDeleter.DeleteTopic(child.Id, parent.Id);
            RecycleContainerAndEntityCache();
            var parentFromDb = R<CategoryRepository>().GetByIdEager(parent.Id);
            var questionFromDb = R<QuestionReadingRepo>().GetById(questionContext.All.First().Id);
            var parentFromCache = EntityCache.GetCategory(parentFromDb.Id);
            var questionFromCache = EntityCache.GetQuestionById(questionFromDb.Id);

            var categoryChange = R<CategoryChangeRepo>().GetForCategory(parent.Id);
            var questionChange = R<QuestionChangeRepo>().GetByQuestionId(questionFromDb.Id);

            //Assert
            Assert.IsNotNull(parentFromDb);
            Assert.AreEqual(CategoryChangeType.Create, categoryChange.First().Type);
            Assert.AreEqual(CategoryChangeType.Relations, categoryChange[1].Type);
            Assert.AreEqual(CategoryChangeType.Update, categoryChange.Last().Type);
            Assert.NotNull(questionChange);
            Assert.AreEqual(QuestionChangeType.Create, questionChange.Type);

            Assert.AreEqual(parentFromDb.CountQuestions, 1);
            Assert.AreEqual(parentFromDb.Id, questionFromDb.Categories.First().Id);
            Assert.AreEqual(questionFromDb.Categories.Count(), 1);

            Assert.IsNotNull(parentFromCache);
            Assert.AreEqual(parentFromCache.CountQuestions, 1);
            Assert.AreEqual(parentFromCache.Id, questionFromDb.Categories.First().Id);
            Assert.AreEqual(questionFromCache.Categories.Count(), 1);
        }

        [Test]
        public void MoveQuestionNoParent()
        {
            var contextTopic = ContextCategory.New();

            var sessionUser = R<SessionUser>();
            var creator = new User { Id = sessionUser.UserId };


            var child = contextTopic.Add("child",
                    CategoryType.Standard,
                    creator)
                .GetTopicByName("child");

            contextTopic.Persist();


            var categoryRepo = R<CategoryRepository>();

            var questionContext = ContextQuestion.New(
                R<QuestionWritingRepo>(),
                R<AnswerRepo>(),
                R<AnswerQuestion>(),
                R<UserWritingRepo>(),
                categoryRepo,
                true);


            questionContext.AddQuestion("Frage1", creator: creator, categories: new List<Category> { child });
            var parentId = 0;
            RecycleContainerAndEntityCache();

            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                R<CategoryDeleter>().DeleteTopic(child.Id, parentId);
            });
            Assert.AreEqual(exception.Message, "parent is null");
        }
    }
}

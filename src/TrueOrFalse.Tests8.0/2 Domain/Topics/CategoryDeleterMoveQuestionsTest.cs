namespace TrueOrFalse.Tests;

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
                PageType.Standard,
                creator)
            .GetTopicByName(parentName);

        var child = contextTopic.Add(childName,
                PageType.Standard,
                creator)
            .GetTopicByName(childName);

        contextTopic.Persist();
        contextTopic.AddChild(parent, child);

        var questionContext = ContextQuestion.New(persistImmediately: true);

        questionContext.AddQuestion("question1", creator: creator, categories: new List<Page> { child });
        var categoryDeleter = R<PageDeleter>();
        //Act
        categoryDeleter.DeleteTopic(child.Id, parent.Id);
        RecycleContainerAndEntityCache();
        var parentFromDb = R<PageRepository>().GetByIdEager(parent.Id);
        var questionFromDb = R<QuestionReadingRepo>().GetById(questionContext.All.First().Id);
        var parentFromCache = EntityCache.GetPage(parentFromDb.Id);
        var questionFromCache = EntityCache.GetQuestionById(questionFromDb.Id);

        var categoryChange = R<PageChangeRepo>().GetForCategory(parent.Id);
        var questionChange = R<QuestionChangeRepo>().GetByQuestionId(questionFromDb.Id);

        //Assert
        Assert.IsNotNull(parentFromDb);
        Assert.AreEqual(PageChangeType.Create, categoryChange.First().Type);
        Assert.AreEqual(PageChangeType.Relations, categoryChange[1].Type);
        Assert.AreEqual(PageChangeType.ChildTopicDeleted, categoryChange.Last().Type);
        Assert.NotNull(questionChange);
        Assert.AreEqual(QuestionChangeType.Create, questionChange.Type);

        Assert.AreEqual(parentFromDb.CountQuestionsAggregated, 1);
        Assert.AreEqual(parentFromDb.Id, questionFromDb.Categories.First().Id);
        Assert.AreEqual(questionFromDb.Categories.Count(), 1);

        Assert.IsNotNull(parentFromCache);
        Assert.AreEqual(parentFromCache.CountQuestionsAggregated, 1);
        Assert.AreEqual(parentFromCache.Id, questionFromDb.Categories.First().Id);
        Assert.AreEqual(questionFromCache.Pages.Count(), 1);
    }

    [Test]
    public void MoveQuestionNoParent()
    {
        var contextTopic = ContextCategory.New();

        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };


        var child = contextTopic.Add("child",
                PageType.Standard,
                creator)
            .GetTopicByName("child");

        contextTopic.Persist();

        var categoryRepo = R<PageRepository>();

        var questionContext = ContextQuestion.New();

        questionContext.AddQuestion("question1", creator: creator, categories: new List<Page> { child });
        var parentId = 0;
        RecycleContainerAndEntityCache();

        var result = R<PageDeleter>().DeleteTopic(child.Id, parentId);

        Assert.AreEqual(result.Success, false);
        Assert.AreEqual(result.MessageKey, FrontendMessageKeys.Error.Page.TopicNotSelected);

    }
}


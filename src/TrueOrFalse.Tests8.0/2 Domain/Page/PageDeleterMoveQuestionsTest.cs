﻿namespace TrueOrFalse.Tests;

public class PageDeleterMoveQuestionsTest : BaseTest
{
    [Test]
    public void Should_Move_Questions_To_Parent()
    {
        //Arrange
        var contextPage = ContextPage.New();
        var parentName = "parent name";
        var childName = "child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage.Add(
                parentName,
                PageType.Standard,
                creator)
            .GetPageByName(parentName);

        var child = contextPage.Add(childName,
                PageType.Standard,
                creator)
            .GetPageByName(childName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);

        var questionContext = ContextQuestion.New(persistImmediately: true);

        questionContext.AddQuestion("question1", creator: creator, pages: new List<Page> { child });
        var pageDeleter = R<PageDeleter>();
        //Act
        pageDeleter.DeletePage(child.Id, parent.Id);
        RecycleContainerAndEntityCache();
        var parentFromDb = R<PageRepository>().GetByIdEager(parent.Id);
        var questionFromDb = R<QuestionReadingRepo>().GetById(questionContext.All.First().Id);
        var parentFromCache = EntityCache.GetPage(parentFromDb.Id);
        var questionFromCache = EntityCache.GetQuestionById(questionFromDb.Id);

        var pageChange = R<PageChangeRepo>().GetForPage(parent.Id);
        var questionChange = R<QuestionChangeRepo>().GetByQuestionId(questionFromDb.Id);

        //Assert
        Assert.IsNotNull(parentFromDb);
        Assert.AreEqual(PageChangeType.Create, pageChange.First().Type);
        Assert.AreEqual(PageChangeType.Relations, pageChange[1].Type);
        Assert.AreEqual(PageChangeType.ChildPageDeleted, pageChange.Last().Type);
        Assert.NotNull(questionChange);
        Assert.AreEqual(QuestionChangeType.Create, questionChange.Type);

        Assert.AreEqual(parentFromDb.CountQuestionsAggregated, 1);
        Assert.AreEqual(parentFromDb.Id, questionFromDb.Pages.First().Id);
        Assert.AreEqual(questionFromDb.Pages.Count(), 1);

        Assert.IsNotNull(parentFromCache);
        Assert.AreEqual(parentFromCache.CountQuestionsAggregated, 1);
        Assert.AreEqual(parentFromCache.Id, questionFromDb.Pages.First().Id);
        Assert.AreEqual(questionFromCache.Pages.Count(), 1);
    }

    [Test]
    public void MoveQuestionNoParent()
    {
        var contextPage = ContextPage.New();

        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };


        var child = contextPage.Add("child",
                PageType.Standard,
                creator)
            .GetPageByName("child");

        contextPage.Persist();

        var pageRepo = R<PageRepository>();

        var questionContext = ContextQuestion.New();

        questionContext.AddQuestion("question1", creator: creator, pages: new List<Page> { child });
        var parentId = 0;
        RecycleContainerAndEntityCache();

        var result = R<PageDeleter>().DeletePage(child.Id, parentId);

        Assert.AreEqual(result.Success, false);
        Assert.AreEqual(result.MessageKey, FrontendMessageKeys.Error.Page.PageNotSelected);

    }
}


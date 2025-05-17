internal class PageDeleterMoveQuestionsTest : BaseTestHarness
{
    [Test]
    public async Task Should_Move_Questions_To_Parent()
    {
        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);

        var questionContext = NewQuestionContext(persistImmediately: true);

        questionContext.AddQuestion("question1", creator: creator, pages: new List<Page> { child });
        var pageDeleter = R<PageDeleter>();
        
        //Act
        pageDeleter.DeletePage(child.Id, parent.Id);
        
        
        // Assert
        await ReloadCaches();
        var parentFromDb = R<PageRepository>().GetByIdEager(parent.Id);
        var questionFromDb = R<QuestionReadingRepo>().GetById(questionContext.All.First().Id);
        var parentFromCache = EntityCache.GetPage(parentFromDb!.Id);
        var questionFromCache = EntityCache.GetQuestionById(questionFromDb.Id);

        var pageChange = R<PageChangeRepo>().GetForPage(parent.Id);
        var questionChange = R<QuestionChangeRepo>().GetByQuestionId(questionFromDb.Id);

        Assert.That(parentFromDb, Is.Not.Null);
        Assert.That(PageChangeType.Create, Is.EqualTo(pageChange.First().Type));
        Assert.That(PageChangeType.Relations, Is.EqualTo(pageChange[1].Type));
        Assert.That(PageChangeType.ChildPageDeleted, Is.EqualTo(pageChange.Last().Type));
        Assert.That(questionChange, Is.Not.Null);
        Assert.That(QuestionChangeType.Create, Is.EqualTo(questionChange.Type));

        Assert.That(parentFromDb.CountQuestionsAggregated, Is.EqualTo(1));
        Assert.That(parentFromDb.Id, Is.EqualTo(questionFromDb.Pages.First().Id));
        Assert.That(questionFromDb.Pages.Count, Is.EqualTo(1));

        Assert.That(parentFromCache, Is.Not.Null);
        Assert.That(parentFromCache.CountQuestionsAggregated, Is.EqualTo(1));
        Assert.That(parentFromCache.Id, Is.EqualTo(questionFromDb.Pages.First().Id));
        Assert.That(questionFromCache.Pages.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task MoveQuestionNoParent()
    {
        var contextPage = NewPageContext();

        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var child = contextPage
            .Add("child", creator)
            .GetPageByName("child");

        contextPage.Persist();

        var questionContext = NewQuestionContext();
        questionContext.AddQuestion("question1", creator: creator, pages: new List<Page> { child });
        var parentId = 0;
        
        await ReloadCaches();

        var result = R<PageDeleter>().DeletePage(child.Id, parentId);

        Assert.That(result.Success, Is.EqualTo(false));
        Assert.That(result.MessageKey, Is.EqualTo(FrontendMessageKeys.Error.Page.PageNotSelected));
    }
}


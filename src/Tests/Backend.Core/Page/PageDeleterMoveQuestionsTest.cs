internal class PageDeleterMoveQuestionsTest : BaseTestHarness
{
    private UserLoginApiWrapper _userLoginApi => _testHarness.ApiUserLogin;

    [Test]
    public async Task Should_Move_Questions_To_Parent()
    {
        await ClearData();
        
        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";
        var creator = new User { Id = _testHarness.DefaultSessionUserId };

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
        
        await _userLoginApi.LoginAsSessionUser();
        var pageDeleter = R<PageDeleter>();
        
        //Act
        pageDeleter.DeletePage(child.Id, parent.Id);


        // Assert
        await ReloadCaches();

        await Verify(new
        {
            allPages = await _testHarness.DbData.AllPagesAsync(),
            allPagrelations = await _testHarness.DbData.AllPageRelationsAsync(),
            allQuestions = await _testHarness.DbData.AllQuestionsAsync(),
            allPagesToQuestions = await _testHarness.DbData.AllPageQuestionsRelationsAsync(),
            allPagesInCache = EntityCache.GetAllPagesList(),
            allQuestionsInCache = EntityCache.GetAllQuestions(),
            childQuestions = EntityCache.GetQuestionsForPage(parent.Id),
            pageChanges = R<PageChangeRepo>().GetForPage(parent.Id).Select(_pageChange => _pageChange.Type),
            questionChanges = R<QuestionChangeRepo>().GetByQuestionId(questionContext.All.First().Id)
        });
    }

    [Test]
    public async Task MoveQuestionNoParent()
    {
        var contextPage = NewPageContext();

        var creator = new User { Id = _testHarness.DefaultSessionUserId };

        var child = contextPage
            .Add("child", creator)
            .GetPageByName("child");

        contextPage.Persist();

        var questionContext = NewQuestionContext();
        questionContext.AddQuestion("question1", creator: creator, pages: new List<Page> { child });
        var parentId = 0;
        
        await ReloadCaches();

        await _userLoginApi.LoginAsSessionUser();
        var result = R<PageDeleter>().DeletePage(child.Id, parentId);

        Assert.That(result.Success, Is.EqualTo(false));
        Assert.That(result.MessageKey, Is.EqualTo(FrontendMessageKeys.Error.Page.PageNotSelected));
    }
}


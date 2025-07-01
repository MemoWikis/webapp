internal class CreatePage_tests : BaseTestHarness
{
    [Test]
    public async Task Should_create_Page_in_Db()
    {
        //Arrange
        var context = NewPageContext();
        var parentName = "Parent";

        var sessionUser = R<SessionUser>();
        var pageViewRepo = R<PageViewRepo>();

        var sessionUserDbUser = R<UserReadingRepo>().GetById(_testHarness.DefaultSessionUserId)!;

        var parent = context
            .Add(parentName, creator: sessionUserDbUser, isWiki: true)
            .Persist().All
            .Single(c => c.Name.Equals(parentName));

        sessionUser.Login(sessionUserDbUser, pageViewRepo);

        var childName = "child";

        //Act
        R<PageCreator>().Create(childName, parent.Id, sessionUser);

        //Assert
        await ReloadCaches();

        var childFromDatabase = R<PageRepository>().GetByName(childName).Single();
        DateTime referenceDate = DateTime.Now;
        var relations = R<PageRelationRepo>().GetByRelationId(parent.Id);

        Assert.That(childFromDatabase, Is.Not.Null);
        Assert.That(sessionUser, Is.Not.Null);
        Assert.That(childFromDatabase.Creator, Is.Not.Null);
        Assert.That(childFromDatabase.Name, Is.EqualTo(childName));
        Assert.That(childFromDatabase.Creator.Id, Is.EqualTo(sessionUser.User.Id));
        Assert.That(childFromDatabase.Creator.Name, Is.EqualTo(sessionUser.User.Name));
        Assert.That(childFromDatabase.DateCreated,
            Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1)));
        Assert.That(childFromDatabase.DateModified,
            Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1)));
        Assert.That(relations.First().Child.Id, Is.EqualTo(childFromDatabase.Id));
    }

    [Test]
    public async Task Should_create_Page_in_EntityCache()
    {
        await ClearData();

        //Arrange
        var context = NewPageContext();

        var sessionUser = R<SessionUser>();
        var sessionDbUser = R<UserReadingRepo>().GetById(_testHarness.DefaultSessionUserId)!;;

        var parentName = "Parent";
        var parent = context
            .Add(parentName, isWiki: true, creator: sessionDbUser)
            .Persist()
            .All
            .Single(c => c.Name.Equals(parentName));
        var pageViewRepo = R<PageViewRepo>();

        sessionUser.Login(sessionDbUser, pageViewRepo);

        var childName = "child";

        //Act
        R<PageCreator>().Create(childName, parent.Id, sessionUser);

        //Arrange
        var childFromEntityCache = EntityCache.GetByPageName(childName).Single();
        DateTime referenceDate = DateTime.Now;

        Assert.That(childFromEntityCache, Is.Not.Null);
        Assert.That(sessionUser, Is.Not.Null);
        Assert.That(childFromEntityCache.Creator, Is.Not.Null);
        Assert.That(childFromEntityCache.Name, Is.EqualTo(childName));
        Assert.That(childFromEntityCache.Creator.Id, Is.EqualTo(sessionUser.User.Id));
        Assert.That(childFromEntityCache.Creator.Name, Is.EqualTo(sessionUser.User.Name));
        Assert.That(childFromEntityCache.DateCreated,
            Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1)));
        Assert.That(childFromEntityCache.ParentRelations.Count, Is.EqualTo(1));
        Assert.That(
            parent.Name, Is.EqualTo(GraphService
                .VisibleAscendants(childFromEntityCache.Id, R<PermissionCheck>())
                .First().Name));
    }
}
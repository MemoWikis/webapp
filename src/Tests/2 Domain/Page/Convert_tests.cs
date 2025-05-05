[TestFixture]
public class Convert_tests : BaseTest
{
    [Test]
    public void ConvertPageToWiki_Should_Succeed_With_ValidInputs()
    {
        // Arrange
        var permissionCheck = R<PermissionCheck>();
        var pageRepository = R<PageRepository>();
        var pageRelationRepo = R<PageRelationRepo>();
        var userWritingRepo = R<UserWritingRepo>();

        var userId = 1;

        var context = ContextPage.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1", creator: new User { Id = userId })
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));
        context.AddChild(root, children.ByName("Sub2"));

        var pageConversion = new PageConversion(permissionCheck, pageRepository, pageRelationRepo, userWritingRepo);

        var page = EntityCache.GetPage(children.ByName("Sub1").Id);
        // Add page and user to EntityCache
        EntityCache.AddOrUpdate(page);
        var userCacheItem = new UserCacheItem { Id = userId };
        EntityCache.AddOrUpdate(userCacheItem);

        // Act
        pageConversion.ConvertPageToWiki(page, userId, keepParents: false);

        // Assert
        Assert.That(page.IsWiki, Is.True, "Page should be marked as Wiki.");

        // Verify that the page entity is updated in the repository
        var updatedPageEntity = pageRepository.GetByIdEager(page.Id);
        Assert.That(updatedPageEntity, Is.Not.Null, "Page entity should exist in repository.");
        Assert.That(updatedPageEntity.IsWiki, Is.True, "Page entity should be marked as Wiki.");

        // Verify that the EntityCache was updated
        var cachedPage = EntityCache.GetPage(page.Id);
        Assert.That(cachedPage, Is.Not.Null, "Updated page should exist in EntityCache.");
        Assert.That(cachedPage.IsWiki, Is.True, "Cached page should be marked as Wiki.");

        // Verify that the user was updated
        Assert.That(userCacheItem.WikiIds, Is.Not.Empty, "User's WikiIds should have been cleaned up.");
    }

    [Test]
    public void ConvertPageToWiki_Should_Throw_ArgumentNullException_When_PageIsNull()
    {
        // Arrange
        var permissionCheck = R<PermissionCheck>();
        var pageRepository = R<PageRepository>();
        var pageRelationRepo = R<PageRelationRepo>();
        var userWritingRepo = R<UserWritingRepo>();

        var pageConversion = new PageConversion(permissionCheck, pageRepository, pageRelationRepo, userWritingRepo);

        PageCacheItem? page = null;
        var userId = 1;

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => pageConversion.ConvertPageToWiki(page, userId));
    }

    [Test]
    public void ConvertPageToWiki_Should_Throw_ArgumentException_When_UserIdIsInvalid()
    {
        // Arrange
        var permissionCheck = R<PermissionCheck>();
        var pageRepository = R<PageRepository>();
        var pageRelationRepo = R<PageRelationRepo>();
        var userWritingRepo = R<UserWritingRepo>();

        var pageConversion = new PageConversion(permissionCheck, pageRepository, pageRelationRepo, userWritingRepo);

        var page = new PageCacheItem { Id = 1, IsWiki = false };
        var userId = 0;

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => pageConversion.ConvertPageToWiki(page, userId));
        Assert.That(ex.Message, Does.Contain("Invalid user ID."));
    }

    [Test]
    public void ConvertPageToWiki_Should_Throw_UnauthorizedAccessException_When_PermissionDenied()
    {
        // Arrange
        var permissionCheck = R<PermissionCheck>();
        var pageRepository = R<PageRepository>();
        var pageRelationRepo = R<PageRelationRepo>();
        var userWritingRepo = R<UserWritingRepo>();

        var userId = 1;

        var context = ContextPage.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));
        context.AddChild(root, children.ByName("Sub2"));

        var pageConversion = new PageConversion(permissionCheck, pageRepository, pageRelationRepo, userWritingRepo);

        var page = EntityCache.GetPage(children.ByName("Sub1").Id);
        // Add page and user to EntityCache
        EntityCache.AddOrUpdate(page);
        var userCacheItem = new UserCacheItem { Id = userId };
        EntityCache.AddOrUpdate(userCacheItem);

        var userId2 = 42;
        // Act & Assert
        var ex = Assert.Throws<UnauthorizedAccessException>(() => pageConversion.ConvertPageToWiki(page, userId2));
        Assert.That(ex.Message, Is.EqualTo("User does not have permission to convert the page to a Wiki."));
    }

    [Test]
    public void ConvertWikiToPage_Should_Succeed_With_ValidInputs()
    {
        // Arrange
        var permissionCheck = R<PermissionCheck>();
        var pageRepository = R<PageRepository>();
        var pageRelationRepo = R<PageRelationRepo>();
        var userWritingRepo = R<UserWritingRepo>();

        var userId = 1;

        var context = ContextPage.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1", creator: new User { Id = userId })
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));
        context.AddChild(root, children.ByName("Sub2"));

        var pageConversion = new PageConversion(permissionCheck, pageRepository, pageRelationRepo, userWritingRepo);

        var page = EntityCache.GetPage(children.ByName("Sub1").Id);
        page.IsWiki = true;
        // Add page and user to EntityCache
        EntityCache.AddOrUpdate(page);
        var userCacheItem = new UserCacheItem { Id = userId };
        userCacheItem.WikiIds = new List<int> { page.Id };
        EntityCache.AddOrUpdate(userCacheItem);


        // Act
        pageConversion.ConvertWikiToPage(page, userId);

        // Assert
        Assert.That(page.IsWiki, Is.False, "Page should no longer be a Wiki.");

        // Verify that the page entity is updated in the repository
        var updatedPageEntity = pageRepository.GetByIdEager(page.Id);
        Assert.That(updatedPageEntity, Is.Not.Null, "Page entity should exist in repository.");
        Assert.That(updatedPageEntity.IsWiki, Is.False, "Page entity should no longer be a Wiki.");

        // Verify that the EntityCache was updated
        var cachedPage = EntityCache.GetPage(page.Id);
        Assert.That(cachedPage, Is.Not.Null, "Updated page should exist in EntityCache.");
        Assert.That(cachedPage.IsWiki, Is.False, "Cached page should no longer be a Wiki.");

        // Verify that the user was updated
        Assert.That(userCacheItem.FavoriteIds, Is.Null.Or.Empty, "User's FavoriteIds should have been cleaned up.");
        Assert.That(userCacheItem.WikiIds, Is.Null.Or.Empty, "User's WikiIds should have been cleaned up.");
    }

    [Test]
    public void ConvertWikiToPage_Should_Throw_ArgumentNullException_When_PageIsNull()
    {
        // Arrange
        var permissionCheck = R<PermissionCheck>();
        var pageRepository = R<PageRepository>();
        var pageRelationRepo = R<PageRelationRepo>();
        var userWritingRepo = R<UserWritingRepo>();

        var pageConversion = new PageConversion(permissionCheck, pageRepository, pageRelationRepo, userWritingRepo);

        PageCacheItem? page = null;
        var userId = 1;

        // Act & Assert
        var ex = Assert.Throws<NullReferenceException>(() => pageConversion.ConvertWikiToPage(page, userId));
    }

    [Test]
    public void ConvertWikiToPage_Should_Throw_ArgumentException_When_UserIdIsInvalid()
    {
        // Arrange
        var permissionCheck = R<PermissionCheck>();
        var pageRepository = R<PageRepository>();
        var pageRelationRepo = R<PageRelationRepo>();
        var userWritingRepo = R<UserWritingRepo>();

        var pageConversion = new PageConversion(permissionCheck, pageRepository, pageRelationRepo, userWritingRepo);

        var page = new PageCacheItem { Id = 1, IsWiki = true };
        var userId = 0;

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => pageConversion.ConvertWikiToPage(page, userId));
    }

    [Test]
    public void ConvertWikiToPage_Should_Throw_UnauthorizedAccessException_When_PermissionDenied()
    {
        // Arrange
        var permissionCheck = R<PermissionCheck>();
        var pageRepository = R<PageRepository>();
        var pageRelationRepo = R<PageRelationRepo>();
        var userWritingRepo = R<UserWritingRepo>();
        var userId = 1;

        var context = ContextPage.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: PageVisibility.Private)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));
        context.AddChild(root, children.ByName("Sub2"));

        var pageConversion = new PageConversion(permissionCheck, pageRepository, pageRelationRepo, userWritingRepo);

        var page = EntityCache.GetPage(children.ByName("Sub1").Id);
        page.IsWiki = true;
        // Add page and user to EntityCache
        EntityCache.AddOrUpdate(page);
        var userCacheItem = new UserCacheItem { Id = userId };
        EntityCache.AddOrUpdate(userCacheItem);

        var userId2 = 42;
        // Act & Assert
        var ex = Assert.Throws<UnauthorizedAccessException>(() => pageConversion.ConvertWikiToPage(page, userId2));
        Assert.That(ex.Message, Is.EqualTo("User does not have permission to convert the Wiki to a page."));
    }
}

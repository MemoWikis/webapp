using Meilisearch;

internal class PageDeleter_tests : BaseTestHarness
{
    [Test]
    public async Task Should_delete_child()
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

        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(child.Id, parent.Id);

        //Assert
        Assert.That(requestResult.Success);
        Assert.That(requestResult.HasChildren, Is.False);
        Assert.That(requestResult.IsNotCreatorOrAdmin, Is.False);
        Assert.That(parent.Id, Is.EqualTo(requestResult.RedirectParent.Id));
    }

    [Test]
    public async Task Should_delete_child_of_child_and_remove_relation()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        var childOfChild = contextPage
            .Add(childOfChildName, creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);

        await ReloadCaches();
        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(childOfChild.Id, parent.Id);

        //Assert
        await ReloadCaches();

        var pageRepo = R<PageRepository>();
        var allAvailablePages = pageRepo.GetAll();
        var parentChildren = pageRepo.GetChildren(parent.Id);
        var childrenOfChild = pageRepo.GetChildren(child.Id);

        Assert.That(requestResult, Is.Not.False);
        Assert.That(requestResult.Success);
        Assert.That(requestResult.HasChildren, Is.False);
        Assert.That(requestResult.IsNotCreatorOrAdmin, Is.False);
        Assert.That(child.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        Assert.That(allAvailablePages.Any());
        Assert.That(allAvailablePages.Contains(parent));
        Assert.That(allAvailablePages.Contains(child));
        Assert.That(parentChildren, Is.Not.Empty);
        Assert.That(parentChildren.Count == 1);
        Assert.That(childName, Is.EqualTo(parentChildren.First().Name));
        Assert.That(childrenOfChild, Is.Empty);
    }

    [Test]
    public async Task Should_delete_child_of_child_and_remove_relations_in_EntityCache()
    {
        await ClearData();

        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);
        var childOfChild = contextPage
            .Add(childOfChildName, creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(childOfChild.Id, parent.Id);
        await ReloadCaches();

        //Assert
        var allPagesInEntityCache = EntityCache.GetAllPagesList();
        var cachedParent = EntityCache.GetPage(parent.Id);
        var cachedChild = EntityCache.GetPage(child.Id);

        Assert.That(requestResult.Success);
        Assert.That(requestResult.HasChildren, Is.False);
        Assert.That(requestResult.IsNotCreatorOrAdmin, Is.False);
        Assert.That(child.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        Assert.That(allPagesInEntityCache.Any());
        Assert.That(allPagesInEntityCache.Any(c => c.Id == parent.Id));
        Assert.That(allPagesInEntityCache.Any(c => c.Id == child.Id));
        Assert.That(allPagesInEntityCache.Any(c => c.Name.Equals(childOfChildName)), Is.False);
        Assert.That(cachedParent.ChildRelations, Is.Not.Empty);
        Assert.That(cachedChild.Id,
            Is.EqualTo(cachedParent.ChildRelations.Single().ChildId));
        Assert.That(cachedParent.ParentRelations, Is.Empty);
        Assert.That(cachedChild.ChildRelations, Is.Empty);
        Assert.That(cachedParent.Id, Is.EqualTo(cachedChild.ParentRelations.Single().Id));

        var allRelationsInEntityCache = EntityCache.GetAllRelations();
        Assert.That(allRelationsInEntityCache.Any(r => r.ChildId == childOfChild.Id), Is.False);
    }

    [Test]
    [Description("child of child has extra parent")]
    public async Task Should_delete_child_and_remove_relations_in_EntityCache_child_of_child_has_extra_parent()
    {
        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var firstChildName = "first child name";
        var secondChildName = "second child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var firstChild = contextPage
            .Add(firstChildName, creator)
            .GetPageByName(firstChildName);

        var secondChild = contextPage
            .Add(secondChildName, creator)
            .GetPageByName(secondChildName);

        var childOfChild = contextPage
            .Add(childOfChildName, creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, firstChild);
        contextPage.AddChild(parent, secondChild);
        contextPage.AddChild(firstChild, childOfChild);
        contextPage.AddChild(secondChild, childOfChild);
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(firstChild.Id, parent.Id);
        await ReloadCaches();

        //Assert
        Assert.That(requestResult.Success);
    }

    [Test]
    [Description("child has a child, so it can't be deleted or removed")]
    public async Task Should_fail_delete_child_and_remove_relations_in_EntityCache_child_has_child()
    {
        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        var childOfChild = contextPage
            .Add(childOfChildName, creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);
        await ReloadCaches();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(child.Id, parent.Id);
        await ReloadCaches();

        //Assert
        Assert.That(requestResult.Success, Is.False);
        Assert.That(requestResult.HasChildren);
        Assert.That(requestResult.IsNotCreatorOrAdmin, Is.False);
    }

    [Test]
    [Description("no rights")]
    public async Task Should_fail_delete_child_and_remove_relations_in_EntityCache_no_rights()
    {
        //Arrange
        var contextPage = NewPageContext();
        var parentName = "parent name";
        var childName = "child name";

        var creator = new User { Id = 2, IsInstallationAdmin = false, Name = "Creator" };

        var parent = contextPage
            .Add(parentName, creator)
            .GetPageByName(parentName);

        var child = contextPage
            .Add(childName, creator)
            .GetPageByName(childName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        await ReloadCaches();

        //Act
        var pageDeleter = R<PageDeleter>();
        var requestResult = pageDeleter.DeletePage(child.Id, parent.Id);


        //Assert
        await ReloadCaches();

        await Verify(requestResult);
    }

    [Test]
    [Description("Verify page is removed from Meilisearch after deletion")]
    public async Task Should_delete_page_from_meilisearch()
    {
        await ClearData();

        // Arrange
        var contextPage = NewPageContext();
        var pageName = "meilisearch test page";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var root = contextPage
            .Add("root", creator)
            .GetPageByName("root");

        var page = contextPage
            .Add(pageName, creator)
            .GetPageByName(pageName);

        contextPage.Persist();
        await ReloadCaches();

        // Create a client to directly verify Meilisearch state
        var client = new MeilisearchClient(_testHarness.MeilisearchUrl, TestHarness.MeilisearchMasterKey);
        var index = client.Index(MeilisearchIndices.Pages);

        // Allow time for initial indexing
        await Task.Delay(500);

        // Search for the page before deletion
        var beforeQuery = new SearchQuery { Q = pageName };
        var beforeResult = await index.SearchAsync<MeilisearchPageMap>(beforeQuery.ToString());

        // Act
        var pageDeleter = R<PageDeleter>();
        var deleteResult = pageDeleter.DeletePage(page.Id, null);

        // Allow time for deletion to propagate
        await Task.Delay(500);

        // Search for the page after deletion
        var afterQuery = new SearchQuery { Q = pageName };
        var afterResult = await index.SearchAsync<MeilisearchPageMap>(afterQuery.ToString());

        // Verify the before and after states
        await Verify(new
        {
            Page = new
            {
                Id = page.Id,
                Name = pageName
            },
            BeforeDelete = new
            {
                ResultCount = beforeResult.Hits.Count,
                ContainsPage = beforeResult.Hits.Any(p => p.Id == page.Id)
            },
            DeleteResult = deleteResult,
            AfterDelete = new
            {
                ResultCount = afterResult.Hits.Count,
                ContainsPage = afterResult.Hits.Any(p => p.Id == page.Id)
            }
        });
    }
}

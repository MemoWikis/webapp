namespace TrueOrFalse.Tests;
public class PageDeleter_tests : BaseTest
{
    [Test]
    public void Should_delete_child()
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

        RecycleContainerAndEntityCache();

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
    public void Should_delete_child_of_child_and_remove_relation()
    {
        //Arrange
        var contextPage = ContextPage.New();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
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

        var childOfChild = contextPage.Add(childOfChildName,
                PageType.Standard,
                creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);

        RecycleContainerAndEntityCache();
        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(childOfChild.Id, parent.Id);

        //Assert
        RecycleContainerAndEntityCache();

        var pageRepo = R<PageRepository>();
        var allAvailablePages = pageRepo.GetAll();
        var parentChildren =
            pageRepo.GetChildren(PageType.Standard, PageType.Standard, parent.Id);
        var childrenOfChild = pageRepo.GetChildren(PageType.Standard,
            PageType.Standard, child.Id);

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
    public void Should_delete_child_of_child_and_remove_relations_in_EntityCache()
    {
        //Arrange
        var contextPage = ContextPage.New();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
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
        var childOfChild = contextPage.Add(childOfChildName,
                PageType.Standard,
                creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);
        RecycleContainerAndEntityCache();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(childOfChild.Id, parent.Id);
        RecycleContainerAndEntityCache();

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
    public void
        Should_delete_child_and_remove_relations_in_EntityCache_child_of_child_has_extra_parent()
    {
        //Arrange
        var contextPage = ContextPage.New();
        var parentName = "parent name";
        var firstChildName = "first child name";
        var secondChildName = "second child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextPage.Add(
                parentName,
                PageType.Standard,
                creator)
            .GetPageByName(parentName);

        var firstChild = contextPage.Add(firstChildName,
                PageType.Standard,
                creator)
            .GetPageByName(firstChildName);

        var secondChild = contextPage.Add(secondChildName,
                PageType.Standard,
                creator)
            .GetPageByName(secondChildName);

        var childOfChild = contextPage.Add(childOfChildName,
                PageType.Standard,
                creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, firstChild);
        contextPage.AddChild(parent, secondChild);
        contextPage.AddChild(firstChild, childOfChild);
        contextPage.AddChild(secondChild, childOfChild);
        RecycleContainerAndEntityCache();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(firstChild.Id, parent.Id);
        RecycleContainerAndEntityCache();

        //Assert
        Assert.That(requestResult.Success);
    }

    [Test]
    [Description("child has a child, so it can't be deleted or removed")]
    public void Should_fail_delete_child_and_remove_relations_in_EntityCache_child_has_child()
    {
        //Arrange
        var contextPage = ContextPage.New();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
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
        var childOfChild = contextPage.Add(childOfChildName,
                PageType.Standard,
                creator)
            .GetPageByName(childOfChildName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        contextPage.AddChild(child, childOfChild);
        RecycleContainerAndEntityCache();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(child.Id, parent.Id);
        RecycleContainerAndEntityCache();

        //Assert
        Assert.That(requestResult.Success, Is.False);
        Assert.That(requestResult.HasChildren);
        Assert.That(requestResult.IsNotCreatorOrAdmin, Is.False);
    }

    [Test]
    [Description("no rights")]
    public void Should_fail_delete_child_and_remove_relations_in_EntityCache_no_rights()
    {
        //Arrange
        var contextPage = ContextPage.New();
        var parentName = "parent name";
        var childName = "child name";

        var creator = new User { Id = 2, IsInstallationAdmin = false, Name = "Creator" };

        var parent = contextPage.Add(
                parentName,
                PageType.Standard,
                creator)
            .GetPageByName(parentName);

        var child = contextPage.Add(
                childName,
                PageType.Standard,
                creator)
            .GetPageByName(childName);

        contextPage.Persist();
        contextPage.AddChild(parent, child);
        RecycleContainerAndEntityCache();

        var pageDeleter = R<PageDeleter>();

        //Act
        var requestResult = pageDeleter.DeletePage(child.Id, parent.Id);
        RecycleContainerAndEntityCache();

        //Assert
        Assert.That(requestResult.Success);
        Assert.That(requestResult.IsNotCreatorOrAdmin);
    }
}

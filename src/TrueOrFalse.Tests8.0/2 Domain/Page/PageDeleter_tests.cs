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

        var categoryDeleter = R<PageDeleter>();

        //Act
        var requestResult = categoryDeleter.DeletePage(child.Id, parent.Id);

        //Assert
        Assert.IsNotNull(requestResult);
        Assert.IsTrue(requestResult.Success);
        Assert.IsFalse(requestResult.HasChildren);
        Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
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
        var categoryDeleter = R<PageDeleter>();

        //Act
        var requestResult = categoryDeleter.DeletePage(childOfChild.Id, parent.Id);

        //Assert
        RecycleContainerAndEntityCache();

        var categoryRepo = R<PageRepository>();
        var allAvailablePages = categoryRepo.GetAll();
        var parentChildren =
            categoryRepo.GetChildren(PageType.Standard, PageType.Standard, parent.Id);
        var childrenOfChild = categoryRepo.GetChildren(PageType.Standard,
            PageType.Standard, child.Id);

        Assert.IsNotNull(requestResult);
        Assert.IsTrue(requestResult.Success);
        Assert.IsFalse(requestResult.HasChildren);
        Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
        Assert.That(child.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        Assert.IsTrue(allAvailablePages.Any());
        Assert.IsTrue(allAvailablePages.Contains(parent));
        Assert.IsTrue(allAvailablePages.Contains(child));
        Assert.IsNotEmpty(parentChildren);
        Assert.IsTrue(parentChildren.Count == 1);
        Assert.That(childName, Is.EqualTo(parentChildren.First().Name));
        Assert.IsEmpty(childrenOfChild);
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

        var categoryDeleter = R<PageDeleter>();

        //Act
        var requestResult = categoryDeleter.DeletePage(childOfChild.Id, parent.Id);
        RecycleContainerAndEntityCache();

        //Assert
        var allPagesInEntityCache = EntityCache.GetAllPagesList();
        var cachedParent = EntityCache.GetPage(parent.Id);
        var cachedChild = EntityCache.GetPage(child.Id);

        Assert.IsNotNull(requestResult);
        Assert.IsTrue(requestResult.Success);
        Assert.IsFalse(requestResult.HasChildren);
        Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
        Assert.That(child.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        Assert.IsTrue(allPagesInEntityCache.Any());
        Assert.IsTrue(allPagesInEntityCache.Any(c => c.Id == parent.Id));
        Assert.IsTrue(allPagesInEntityCache.Any(c => c.Id == child.Id));
        Assert.False(allPagesInEntityCache.Any(c => c.Name.Equals(childOfChildName)));
        Assert.IsNotEmpty(cachedParent.ChildRelations);
        Assert.That(cachedChild.Id,
            Is.EqualTo(cachedParent.ChildRelations.Single().ChildId));
        Assert.IsEmpty(cachedParent.ParentRelations);
        Assert.IsEmpty(cachedChild.ChildRelations);
        Assert.That(cachedParent.Id, Is.EqualTo(cachedChild.ParentRelations.Single().Id));

        var allRelationsInEntityCache = EntityCache.GetAllRelations();
        Assert.False(allRelationsInEntityCache.Any(r => r.ChildId == childOfChild.Id));
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

        var categoryDeleter = R<PageDeleter>();

        //Act
        var requestResult = categoryDeleter.DeletePage(firstChild.Id, parent.Id);
        RecycleContainerAndEntityCache();

        //Assert

        Assert.IsNotNull(requestResult);
        Assert.IsTrue(requestResult.Success);
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

        var categoryDeleter = R<PageDeleter>();

        //Act
        var requestResult = categoryDeleter.DeletePage(child.Id, parent.Id);
        RecycleContainerAndEntityCache();

        //Assert

        Assert.IsNotNull(requestResult);
        Assert.IsFalse(requestResult.Success);
        Assert.IsTrue(requestResult.HasChildren);
        Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
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

        var categoryDeleter = R<PageDeleter>();

        //Act
        var requestResult = categoryDeleter.DeletePage(child.Id, parent.Id);
        RecycleContainerAndEntityCache();

        //Assert

        Assert.IsNotNull(requestResult);
        Assert.IsFalse(requestResult.Success);
        Assert.IsTrue(requestResult.IsNotCreatorOrAdmin);
    }
}

namespace TrueOrFalse.Tests;
public class CategoryDeleter_tests : BaseTest
{
    [Test]
    public void Should_delete_child()
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

        RecycleContainerAndEntityCache();

        var categoryDeleter = R<CategoryDeleter>();

        //Act
        var requestResult = categoryDeleter.DeleteTopic(child.Id, parent.Id);

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
        var contextTopic = ContextCategory.New();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
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

        var childOfChild = contextTopic.Add(childOfChildName,
                CategoryType.Standard,
                creator)
            .GetTopicByName(childOfChildName);

        contextTopic.Persist();
        contextTopic.AddChild(parent, child);
        contextTopic.AddChild(child, childOfChild);

        RecycleContainerAndEntityCache();
        var categoryDeleter = R<CategoryDeleter>();

        //Act
        var requestResult = categoryDeleter.DeleteTopic(childOfChild.Id, parent.Id);

        //Assert
        RecycleContainerAndEntityCache();

        var categoryRepo = R<CategoryRepository>();
        var allAvailableTopics = categoryRepo.GetAll();
        var parentChildren =
            categoryRepo.GetChildren(CategoryType.Standard, CategoryType.Standard, parent.Id);
        var childrenOfChild = categoryRepo.GetChildren(CategoryType.Standard,
            CategoryType.Standard, child.Id);

        Assert.IsNotNull(requestResult);
        Assert.IsTrue(requestResult.Success);
        Assert.IsFalse(requestResult.HasChildren);
        Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
        Assert.That(child.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        Assert.IsTrue(allAvailableTopics.Any());
        Assert.IsTrue(allAvailableTopics.Contains(parent));
        Assert.IsTrue(allAvailableTopics.Contains(child));
        Assert.IsNotEmpty(parentChildren);
        Assert.IsTrue(parentChildren.Count == 1);
        Assert.That(childName, Is.EqualTo(parentChildren.First().Name));
        Assert.IsEmpty(childrenOfChild);
    }

    [Test]
    public void Should_delete_child_of_child_and_remove_relations_in_EntityCache()
    {
        //Arrange
        var contextTopic = ContextCategory.New();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
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
        var childOfChild = contextTopic.Add(childOfChildName,
                CategoryType.Standard,
                creator)
            .GetTopicByName(childOfChildName);

        contextTopic.Persist();
        contextTopic.AddChild(parent, child);
        contextTopic.AddChild(child, childOfChild);
        RecycleContainerAndEntityCache();

        var categoryDeleter = R<CategoryDeleter>();

        //Act
        var requestResult = categoryDeleter.DeleteTopic(childOfChild.Id, parent.Id);
        RecycleContainerAndEntityCache();

        //Assert
        var allCategoriesInEntityCache = EntityCache.GetAllCategoriesList();
        var cachedParent = EntityCache.GetCategory(parent.Id);
        var cachedChild = EntityCache.GetCategory(child.Id);

        Assert.IsNotNull(requestResult);
        Assert.IsTrue(requestResult.Success);
        Assert.IsFalse(requestResult.HasChildren);
        Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
        Assert.That(child.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        Assert.IsTrue(allCategoriesInEntityCache.Any());
        Assert.IsTrue(allCategoriesInEntityCache.Any(c => c.Id == parent.Id));
        Assert.IsTrue(allCategoriesInEntityCache.Any(c => c.Id == child.Id));
        Assert.False(allCategoriesInEntityCache.Any(c => c.Name.Equals(childOfChildName)));
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
        var contextTopic = ContextCategory.New();
        var parentName = "parent name";
        var firstChildName = "first child name";
        var secondChildName = "second child name";
        var childOfChildName = "child of child name";
        var sessionUser = R<SessionUser>();
        var creator = new User { Id = sessionUser.UserId };

        var parent = contextTopic.Add(
                parentName,
                CategoryType.Standard,
                creator)
            .GetTopicByName(parentName);

        var firstChild = contextTopic.Add(firstChildName,
                CategoryType.Standard,
                creator)
            .GetTopicByName(firstChildName);

        var secondChild = contextTopic.Add(secondChildName,
                CategoryType.Standard,
                creator)
            .GetTopicByName(secondChildName);

        var childOfChild = contextTopic.Add(childOfChildName,
                CategoryType.Standard,
                creator)
            .GetTopicByName(childOfChildName);

        contextTopic.Persist();
        contextTopic.AddChild(parent, firstChild);
        contextTopic.AddChild(parent, secondChild);
        contextTopic.AddChild(firstChild, childOfChild);
        contextTopic.AddChild(secondChild, childOfChild);
        RecycleContainerAndEntityCache();

        var categoryDeleter = R<CategoryDeleter>();

        //Act
        var requestResult = categoryDeleter.DeleteTopic(firstChild.Id, parent.Id);
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
        var contextTopic = ContextCategory.New();
        var parentName = "parent name";
        var childName = "child name";
        var childOfChildName = "child of child name";
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
        var childOfChild = contextTopic.Add(childOfChildName,
                CategoryType.Standard,
                creator)
            .GetTopicByName(childOfChildName);

        contextTopic.Persist();
        contextTopic.AddChild(parent, child);
        contextTopic.AddChild(child, childOfChild);
        RecycleContainerAndEntityCache();

        var categoryDeleter = R<CategoryDeleter>();

        //Act
        var requestResult = categoryDeleter.DeleteTopic(child.Id, parent.Id);
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
        var contextTopic = ContextCategory.New();
        var parentName = "parent name";
        var childName = "child name";

        var creator = new User { Id = 2, IsInstallationAdmin = false, Name = "Creator" };

        var parent = contextTopic.Add(
                parentName,
                CategoryType.Standard,
                creator)
            .GetTopicByName(parentName);

        var child = contextTopic.Add(
                childName,
                CategoryType.Standard,
                creator)
            .GetTopicByName(childName);

        contextTopic.Persist();
        contextTopic.AddChild(parent, child);
        RecycleContainerAndEntityCache();

        var categoryDeleter = R<CategoryDeleter>();

        //Act
        var requestResult = categoryDeleter.DeleteTopic(child.Id, parent.Id);
        RecycleContainerAndEntityCache();

        //Assert

        Assert.IsNotNull(requestResult);
        Assert.IsFalse(requestResult.Success);
        Assert.IsTrue(requestResult.IsNotCreatorOrAdmin);
    }
}

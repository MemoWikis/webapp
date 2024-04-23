namespace TrueOrFalse.Tests8._0._2_Domain.Topics
{
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

            var firstChild = contextTopic.Add(childName,
                    CategoryType.Standard,
                    creator)
                .GetTopicByName(childName);

            contextTopic.Persist();
            contextTopic.AddChild(parent, firstChild);
            var categoryDeleter = R<CategoryDeleter>();

            //Act
            var requestResult = categoryDeleter.DeleteTopic(firstChild.Id);

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

            //RecycleContainerAndEntityCache();
            var categoryDeleter = R<CategoryDeleter>();

            //Act
            var requestResult = categoryDeleter.DeleteTopic(childOfChild.Id);

            //Assert
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

            var categoryDeleter = R<CategoryDeleter>();

            //Act
            var requestResult = categoryDeleter.DeleteTopic(childOfChild.Id);
            RecycleContainerAndEntityCache();

            //Assert
            var allCategoriesInEntityCache = EntityCache.GetAllCategoriesList();
            var cacheParent = EntityCache.GetCategory(parent.Id);
            var cachedFirstChild = EntityCache.GetCategory(child.Id);

            Assert.IsNotNull(requestResult);
            Assert.IsTrue(requestResult.Success);
            Assert.IsFalse(requestResult.HasChildren);
            Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
            Assert.That(child.Id, Is.EqualTo(requestResult.RedirectParent.Id));
            Assert.IsTrue(allCategoriesInEntityCache.Any());
            Assert.IsTrue(allCategoriesInEntityCache.Any(c => c.Id == parent.Id));
            Assert.IsTrue(allCategoriesInEntityCache.Any(c => c.Id == child.Id));
            Assert.False(allCategoriesInEntityCache.Any(c => c.Name.Equals(childOfChildName)));
            Assert.IsNotEmpty(cacheParent.ChildRelations);
            Assert.That(cachedFirstChild.Id,
                Is.EqualTo(cacheParent.ChildRelations.Single().ChildId));
            Assert.IsEmpty(cacheParent.ParentRelations);
            Assert.IsEmpty(cachedFirstChild.ChildRelations);
            Assert.That(cacheParent.Id, Is.EqualTo(cachedFirstChild.ParentRelations.Single().Id));

            var allRelationsInEntityCache = EntityCache.GetAllRelations();
            Assert.False(allRelationsInEntityCache.Any(r => r.ChildId == childOfChild.Id));
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

            var categoryDeleter = R<CategoryDeleter>();

            //Act
            var requestResult = categoryDeleter.DeleteTopic(child.Id);
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

            var categoryDeleter = R<CategoryDeleter>();

            //Act
            var requestResult = categoryDeleter.DeleteTopic(child.Id);
            RecycleContainerAndEntityCache();

            //Assert

            Assert.IsNotNull(requestResult);
            Assert.IsFalse(requestResult.Success);
            Assert.IsTrue(requestResult.IsNotCreatorOrAdmin);
        }
    }
}
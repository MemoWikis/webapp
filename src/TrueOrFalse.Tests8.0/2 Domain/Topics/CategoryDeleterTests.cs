using NUnit.Framework.Internal;
using Stripe;
using Ubiety.Dns.Core.Records;

namespace TrueOrFalse.Tests8._0._2_Domain.Topics
{
    public class CategoryDeleterTests : BaseTest
    {
        [Test]
        public void Delete_topic_and_remove_child()
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

            //Act
            var categoryDeleter = R<CategoryDeleter>();
            var requestResult = categoryDeleter.DeleteTopic(firstChild.Id);

            //Assert
            Assert.IsNotNull(requestResult);
            Assert.IsTrue(requestResult.Success);
            Assert.IsFalse(requestResult.HasChildren);
            Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
            Assert.That(parent.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        }

        [Test]
        public void Delete_topic_and_remove_second_child()
        {
            //Arrange
            var contextTopic = ContextCategory.New();
            var parentName = "Parent";
            var firstChildName = "FirstChild";
            var secondChildName = "SecondChild";
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

            contextTopic.Persist();
            contextTopic.AddChild(parent, firstChild);
            contextTopic.AddChild(firstChild, secondChild);

            RecycleContainer();

            //Act
            var categoryDeleter = R<CategoryDeleter>();
            var requestResult = categoryDeleter.DeleteTopic(secondChild.Id);

            //Assert
            var categoryRepo = R<CategoryRepository>();
            var allAvailableTopics = categoryRepo.GetAll();
            var parentChildren =
                categoryRepo.GetChildren(CategoryType.Standard, CategoryType.Standard, parent.Id);
            var firstChildChildren = categoryRepo.GetChildren(CategoryType.Standard,
                CategoryType.Standard, firstChild.Id);

            Assert.IsNotNull(requestResult);
            Assert.IsTrue(requestResult.Success);
            Assert.IsFalse(requestResult.HasChildren);
            Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
            Assert.That(parent.Id, Is.EqualTo(requestResult.RedirectParent.Id));
            Assert.IsTrue(allAvailableTopics.Any());
            Assert.IsTrue(allAvailableTopics.Contains(parent));
            Assert.IsTrue(allAvailableTopics.Contains(firstChild));
            Assert.IsNotEmpty(parentChildren);
            Assert.IsTrue(parentChildren.Count == 1);
            Assert.That(firstChildName, Is.EqualTo(parentChildren.First().Name));
            Assert.IsEmpty(firstChildChildren);
        }

        [Test]
        public void Delete_topic_and_remove_relations_for_third_child_test_EntityCache()
        {
            var contextTopic = ContextCategory.New();
            var parentName = "Parent";
            var firstChildName = "FirstChild";
            var secondChildName = "SecondChild";
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

            contextTopic.Persist();
            contextTopic.AddChild(parent, firstChild);
            contextTopic.AddChild(firstChild, secondChild);

            var categoryDeleter = R<CategoryDeleter>();
            var requestResult = categoryDeleter.DeleteTopic(secondChild.Id);
            RecycleContainerAndEntityCache();

            var entityCache = EntityCache.GetAllCategoriesList();
            var cacheParent = EntityCache.GetCategory(parent.Id);
            var cacheFirstchild = EntityCache.GetCategory(firstChild.Id);

            Assert.IsNotNull(requestResult);
            Assert.IsTrue(requestResult.Success);
            Assert.IsFalse(requestResult.HasChildren);
            Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
            Assert.That(parent.Id, Is.EqualTo(requestResult.RedirectParent.Id));
            Assert.IsTrue(entityCache.Any());
            Assert.IsTrue(entityCache.Any(c => c.Id == parent.Id));
            Assert.IsTrue(entityCache.Any(c => c.Id == firstChild.Id));
            Assert.False(entityCache.Any(c => c.Name.Equals(secondChildName)));
            Assert.IsNotEmpty(cacheParent.ChildRelations);
            Assert.That(cacheFirstchild.Id,
                Is.EqualTo(cacheParent.ChildRelations.Single().ChildId));
            Assert.IsEmpty(cacheParent.ParentRelations);
            Assert.IsEmpty(cacheFirstchild.ChildRelations);
            Assert.That(cacheParent.Id, Is.EqualTo(cacheFirstchild.ParentRelations.Single().Id));
        }

        [Test]
        public void Delete_topic_has_child()
        {
            var contextTopic = ContextCategory.New();
            var parentName = "Parent";
            var firstChildName = "FirstChild";
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

            contextTopic.Persist();
            contextTopic.AddChild(parent, firstChild);

            var categoryDeleter = R<CategoryDeleter>();
            var requestResult = categoryDeleter.DeleteTopic(firstChild.Id);
            Assert.IsNotNull(requestResult);
            Assert.IsTrue(requestResult.Success);
            Assert.IsFalse(requestResult.HasChildren);
            Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
            Assert.That(parent.Id, Is.EqualTo(requestResult.RedirectParent.Id));
        }
    }
}
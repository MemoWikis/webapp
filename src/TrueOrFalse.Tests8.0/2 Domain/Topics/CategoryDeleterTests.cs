namespace TrueOrFalse.Tests8._0._2_Domain.Topics
{
    public class CategoryDeleterTests : BaseTest
    {
        [Test]
        [Description("DeleteTopic and RemoveRelations for second child")]
        public void Run_Test()
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

        [Test]
        [Description("DeleteTopic and RemoveRelations for third child")]
        public void Run1_Test()
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
            var categoryRepo = R<CategoryRepository>();
            var allAvailableTopics = categoryRepo.GetAll();
            var parentChildren = categoryRepo.GetChildren(CategoryType.Standard,CategoryType.Standard, parent.Id);
            var firstChildChildren = categoryRepo.GetChildren(CategoryType.Standard, CategoryType.Standard, firstChild.Id);

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
        [Description("DeleteTopic and RemoveRelations for third child Test EntityCache")]
        public void Run1EntityCache_Test()
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
           
            var allAvailableTopics = EntityCache.GetAllCategoriesList();
            //var parentChildren = EntityCache.GetChildren(CategoryType.Standard, CategoryType.Standard, parent.Id);
            //var firstChildChildren = categoryRepo.GetChildren(CategoryType.Standard, CategoryType.Standard, firstChild.Id);

            //Assert.IsNotNull(requestResult);
            //Assert.IsTrue(requestResult.Success);
            //Assert.IsFalse(requestResult.HasChildren);
            //Assert.IsFalse(requestResult.IsNotCreatorOrAdmin);
            //Assert.That(parent.Id, Is.EqualTo(requestResult.RedirectParent.Id));
            //Assert.IsTrue(allAvailableTopics.Any());
            //Assert.IsTrue(allAvailableTopics.Contains(parent));
            //Assert.IsTrue(allAvailableTopics.Contains(firstChild));
            //Assert.IsNotEmpty(parentChildren);
            //Assert.IsTrue(parentChildren.Count == 1);
            //Assert.That(firstChildName, Is.EqualTo(parentChildren.First().Name));
            //Assert.IsEmpty(firstChildChildren);

        }

        [Test]
        [Description("Delete topic has Child")]
        public void Run2_Test()
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

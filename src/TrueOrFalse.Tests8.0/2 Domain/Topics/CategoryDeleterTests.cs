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

           // var categoryDeleter = R<CategoryDeleter>();
           //var requestResult =  categoryDeleter.DeleteTopic(firstChild.Id);
           //var responseData = requestResult.data;
           //var success = responseData.hasChildren;
           // Assert.IsNotNull(requestResult);
           //Assert.IsTrue(requestResult.success);
           //Assert.IsTrue(success);
           //Assert.IsFalse(requestResult.data.hasChildren);
           //Assert.IsFalse(requestResult.data.isNotCreatorOrAdmin);
           //Assert.AreEqual(requestResult.data.redirectParent.Id, parent.Id);


        }

        public class DataObject
        {
            public bool Success { get; set; }
            public bool HasChildren { get; set; }
            public bool IsNotCreatorOrAdmin { get; set; }
            public ParentObject RedirectParent { get; set; }
        }
        public class ParentObject
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
    }
}

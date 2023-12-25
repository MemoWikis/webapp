namespace TrueOrFalse.Tests8._0.Topics
{
    internal class BaseTopicTests : BaseTest
    {

        [Test]
        public void TopicShouldInDatabase()
        {
            var firstTopic = ContextCategory.New().Add("A").Persist().All.First();
            var categoryRepo = R<CategoryRepository>();
            var topicFromDatabase = categoryRepo.GetById(firstTopic.Id); 
            Assert.IsNotNull(firstTopic);
            Assert.IsNotNull(topicFromDatabase);
            Assert.AreEqual(topicFromDatabase?.Name, firstTopic.Name);
        }

        [Test]
        public void TopicsShouldInDatabase()
        {
            var topicIds = ContextCategory.New().Add(5).Persist().All.Select(c => c.Id).ToList();
            var categoryRepo = R<CategoryRepository>();
            var idsFromDatabase = categoryRepo.GetAllIds().ToList(); 

            CollectionAssert.AreEquivalent(topicIds, idsFromDatabase);
        }
    }
}

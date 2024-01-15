namespace TrueOrFalse.Tests8._0.Domain.Topics
{
    internal class CreateTopicTests : BaseTest
    {
        [Test]
        public void CreateTopicInDatabase_Test()
        {
            var context = ContextCategory.New();
            var parentname = "Parent";
            var parent = context.Add(parentname).Persist().All.Single(c => c.Name.Equals(parentname));
            var sessionUser = R<SessionUser>();


            var childName = "child";
            R<CategoryCreator>().Create(childName, parent.Id, sessionUser);

            var childFromDatabase = R<CategoryRepository>().GetByName(childName).Single();
            DateTime referenceDate = DateTime.Now;


            Assert.IsNotNull(childFromDatabase);
            Assert.IsNotNull(sessionUser);
            Assert.IsNotNull(childFromDatabase.Creator);
            Assert.IsNotNull(childFromDatabase.DateCreated);
            Assert.IsNotNull(childFromDatabase.DateModified);
            Assert.AreEqual(childName, childFromDatabase.Name);
            Assert.AreEqual(sessionUser.User.Id, childFromDatabase.Creator.Id);
            Assert.AreEqual(sessionUser.User.Name, childFromDatabase.Creator.Name);
            Assert.That(childFromDatabase.DateCreated, Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1)));
            Assert.That(childFromDatabase.DateModified, Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1))); 
            Assert.AreEqual(childFromDatabase.CategoryRelations.Count, 1);
            Assert.AreEqual(childFromDatabase.CategoryRelations.First().RelatedCategory.Name, parent.Name);
        }

        [Test]
        public void CreateTopicInEntityCache_Test()
        {
            var context = ContextCategory.New();
            var parentname = "Parent";
            var parent = context.Add(parentname).Persist().All.Single(c => c.Name.Equals(parentname));
            var sessionUser = R<SessionUser>();


            var childName = "child";
           R<CategoryCreator>().Create(childName, parent.Id, sessionUser);

            var childFromEntityCache = EntityCache.GetCategoryByName(childName).Single();
            DateTime referenceDate = DateTime.Now;
            

            Assert.IsNotNull(childFromEntityCache);
            Assert.IsNotNull(sessionUser);
            Assert.IsNotNull(childFromEntityCache.Creator);
            Assert.IsNotNull(childFromEntityCache.DateCreated);
            Assert.AreEqual(childName, childFromEntityCache.Name);
            Assert.AreEqual(sessionUser.User.Id, childFromEntityCache.Creator.Id);
            Assert.AreEqual(sessionUser.User.Name, childFromEntityCache.Creator.Name);
            Assert.That(childFromEntityCache.DateCreated, Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1)));
            Assert.AreEqual(childFromEntityCache.CategoryRelations.Count, 1);
            Assert.AreEqual(EntityCache.GetAllParents(childFromEntityCache.Id, R<PermissionCheck>()).First().Name, parent.Name);
        }
    }
}
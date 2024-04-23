using NHibernate.Criterion;

namespace TrueOrFalse.Tests8._0.Domain.Topics
{
    internal class CreateTopicTests : BaseTest
    {
        [Test]
        public void CreateTopicInDatabase_Test()
        {
            //Arrange
            var context = ContextCategory.New();
            var parentName = "Parent";

            var sessionUser = R<SessionUser>();
            sessionUser.Login(_sessionUser);
            var parent = context.Add(parentName, creator: _sessionUser).Persist().All
                .Single(c => c.Name.Equals(parentName));

            var childName = "child";
            R<CategoryCreator>().Create(childName, parent.Id, sessionUser);

            RecycleContainer();

            var childFromDatabase = R<CategoryRepository>().GetByName(childName).Single();
            DateTime referenceDate = DateTime.Now;
            var relations = R<CategoryRelationRepo>().GetByRelationId(parent.Id);

            //Assert
            Assert.IsNotNull(childFromDatabase);
            Assert.IsNotNull(sessionUser);
            Assert.IsNotNull(childFromDatabase.Creator);
            Assert.IsNotNull(childFromDatabase.DateCreated);
            Assert.IsNotNull(childFromDatabase.DateModified);
            Assert.That(childFromDatabase.Name, Is.EqualTo(childName));
            Assert.That(childFromDatabase.Creator.Id, Is.EqualTo(sessionUser.User.Id));
            Assert.That(childFromDatabase.Creator.Name, Is.EqualTo(sessionUser.User.Name));
            Assert.That(childFromDatabase.DateCreated,
                Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1)));
            Assert.That(childFromDatabase.DateModified,
                Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1)));
            Assert.That(relations.First().Child.Id, Is.EqualTo(childFromDatabase.Id));
        }

        [Test]
        public void CreateTopicInEntityCache_Test()
        {
            var context = ContextCategory.New();
            var parentname = "Parent";
            var parent = context.Add(parentname).Persist().All
                .Single(c => c.Name.Equals(parentname));
            var sessionUser = R<SessionUser>();
            sessionUser.Login(_sessionUser);

            var childName = "child";
            R<CategoryCreator>().Create(childName, parent.Id, sessionUser);

            var childFromEntityCache = EntityCache.GetCategoryByName(childName).Single();
            DateTime referenceDate = DateTime.Now;

            Assert.IsNotNull(childFromEntityCache);
            Assert.IsNotNull(sessionUser);
            Assert.IsNotNull(childFromEntityCache.Creator);
            Assert.IsNotNull(childFromEntityCache.DateCreated);
            Assert.That(childFromEntityCache.Name, Is.EqualTo(childName));
            Assert.That(childFromEntityCache.Creator.Id, Is.EqualTo(sessionUser.User.Id));
            Assert.That(childFromEntityCache.Creator.Name, Is.EqualTo(sessionUser.User.Name));
            Assert.That(childFromEntityCache.DateCreated,
                Is.InRange(referenceDate.AddHours(-1), referenceDate.AddHours(1)));
            Assert.That(1, Is.EqualTo(childFromEntityCache.ParentRelations.Count));
            Assert.That(
                parent.Name, Is.EqualTo(GraphService
                    .VisibleAscendants(childFromEntityCache.Id, R<PermissionCheck>())
                    .First().Name));
        }
    }
}
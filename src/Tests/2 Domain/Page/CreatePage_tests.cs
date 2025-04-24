namespace TrueOrFalse.Tests8._0.Domain.Pages
{
    internal class CreatePage_tests : BaseTest
    {
        [Test]
        public void Should_create_Page_in_Db()
        {
            //Arrange
            var context = ContextPage.New();
            var parentName = "Parent";

            var sessionUser = R<SessionUser>();
            var pageViewRepo = R<PageViewRepo>();
            sessionUser.Login(_sessionUser, pageViewRepo);
            var parent = context
                .Add(parentName, creator: _sessionUser)
                .Persist().All
                .Single(c => c.Name.Equals(parentName));

            var childName = "child";

            //Act
            R<PageCreator>().Create(childName, parent.Id, sessionUser);

            //Assert
            RecycleContainerAndEntityCache();

            var childFromDatabase = R<PageRepository>().GetByName(childName).Single();
            DateTime referenceDate = DateTime.Now;
            var relations = R<PageRelationRepo>().GetByRelationId(parent.Id);

            Assert.That(childFromDatabase, Is.Not.Null);
            Assert.That(sessionUser, Is.Not.Null);
            Assert.That(childFromDatabase.Creator, Is.Not.Null);
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
        public void Should_create_Page_in_EntityCache()
        {
            //Arrange
            var context = ContextPage.New();
            var parentname = "Parent";
            var parent = context
                .Add(parentname)
                .Persist()
                .All
                .Single(c => c.Name.Equals(parentname));
            var pageViewRepo = R<PageViewRepo>();

            var sessionUser = R<SessionUser>();
            sessionUser.Login(_sessionUser, pageViewRepo);

            var childName = "child";

            //Act
            R<PageCreator>().Create(childName, parent.Id, sessionUser);

            //Arrange
            var childFromEntityCache = EntityCache.GetByPageName(childName).Single();
            DateTime referenceDate = DateTime.Now;

            Assert.That(childFromEntityCache, Is.Not.Null);
            Assert.That(sessionUser, Is.Not.Null);
            Assert.That(childFromEntityCache.Creator, Is.Not.Null);
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
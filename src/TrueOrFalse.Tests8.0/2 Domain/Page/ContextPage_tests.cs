namespace TrueOrFalse.Tests8._0.Pages;
internal class ContextPage_tests : BaseTest
{
    [Test]
    public void Page_should_be_persisted()
    {
        var firstPage = ContextPage.New().Add("A").Persist().All.First();
        var pageRepo = R<PageRepository>();
        var topicFromDatabase = pageRepo.GetById(firstPage.Id);
        Assert.IsNotNull(firstPage);
        Assert.IsNotNull(topicFromDatabase);
        Assert.AreEqual(topicFromDatabase?.Name, firstPage.Name);
    }

    [Test]
    public void PagesShouldInDatabase()
    {
        var topicIds = ContextPage.New().Add(5).Persist().All.Select(c => c.Id).ToList();
        var pageRepo = R<PageRepository>();
        var idsFromDatabase = pageRepo.GetAllIds().ToList();

        CollectionAssert.AreEquivalent(topicIds, idsFromDatabase);
    }

    [Test]
    public void PageShouldUpdated()
    {
        var pageName = "C1";
        var contextPage = ContextPage.New();
        var pageRepo = R<PageRepository>();

        contextPage.Add(pageName).Persist();
        var pageBeforUpdated = pageRepo
            .GetByName(pageName)
            .Single();
        var newPageName = "newC2";
        pageBeforUpdated.Name = newPageName;
        contextPage.Update(pageBeforUpdated);

        var pageAfterUpdate = pageRepo.GetByName(newPageName).SingleOrDefault();

        Assert.IsNotNull(pageAfterUpdate);
        Assert.AreEqual(newPageName, pageAfterUpdate.Name);
    }

    [Test]
    public void PagesShouldUpdated()
    {
        var pageNameOne = "C1";
        var pageNameTwo = "C2";
        var contextPage = ContextPage.New();
        var pageRepo = R<PageRepository>();

        contextPage.Add(pageNameOne).Persist();
        contextPage.Add(pageNameTwo).Persist();
        var pageOneBeforUpdated = contextPage
            .All
            .Single(c => c.Name.Equals(pageNameOne));
        var pageTwoBeforUpdated = contextPage
            .All
            .Single(c => c.Name.Equals(pageNameTwo));

        var newPageOneName = "newC1";
        var newPageTwoName = "newC2";
        pageOneBeforUpdated.Name = newPageOneName;
        pageTwoBeforUpdated.Name = newPageTwoName;

        contextPage.UpdateAll();

        var pageOneAfterUpdate = pageRepo.GetByName(newPageOneName).SingleOrDefault();
        Assert.IsNotNull(pageOneAfterUpdate);
        Assert.AreEqual(newPageOneName, pageOneAfterUpdate.Name);

        var pageTwoAfterUpdate = pageRepo.GetByName(newPageTwoName).SingleOrDefault();
        Assert.IsNotNull(pageTwoAfterUpdate);
        Assert.AreEqual(newPageTwoName, pageTwoAfterUpdate.Name);
    }


}

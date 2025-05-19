using NUnit.Framework.Legacy;

internal class PagePersistenceTests : BaseTestHarness
{
    [Test]
    public void Page_should_be_persisted()
    {
        var firstPage = NewPageContext().Add("A").Persist().All.First();
        var pageRepo = R<PageRepository>();
        var pageFromDatabase = pageRepo.GetById(firstPage.Id);

        Assert.That(firstPage, Is.Not.Null);
        Assert.That(pageFromDatabase, Is.Not.Null);
        Assert.That(pageFromDatabase?.Name, Is.EqualTo(firstPage.Name));
    }

    [Test]
    public async Task Pages_should_be_in_database()
    {
        await ClearData();
        
        var pageIds = NewPageContext().Add(5).Persist().All.Select(c => c.Id).ToList();
        var pageRepo = R<PageRepository>();
        var idsFromDatabase = pageRepo.GetAllIds().ToList();

        CollectionAssert.AreEquivalent(pageIds, idsFromDatabase);
    }

    [Test]
    public void Page_should_be_updated()
    {
        var pageName = "C1";
        var contextPage = NewPageContext();
        var pageRepo = R<PageRepository>();

        contextPage.Add(pageName).Persist();
        var pageBeforeUpdate = pageRepo
            .GetByName(pageName)
            .Single();
        var newPageName = "newC2";
        pageBeforeUpdate.Name = newPageName;
        contextPage.Update(pageBeforeUpdate);

        var pageAfterUpdate = pageRepo.GetByName(newPageName).SingleOrDefault();

        Assert.That(pageAfterUpdate, Is.Not.Null);
        Assert.That(newPageName, Is.EqualTo(pageAfterUpdate.Name));
    }

    [Test]
    public async Task Pages_should_be_updated()
    {
        await ClearData();

        var pageNameOne = "C1";
        var pageNameTwo = "C2";
        var contextPage = NewPageContext();
        var pageRepo = R<PageRepository>();

        contextPage.Add(pageNameOne).Persist();
        contextPage.Add(pageNameTwo).Persist();
        var pageOneBeforeUpdate = contextPage
            .All
            .Single(c => c.Name.Equals(pageNameOne));
        var pageTwoBeforeUpdate = contextPage
            .All
            .Single(c => c.Name.Equals(pageNameTwo));

        var newPageOneName = "newC1";
        var newPageTwoName = "newC2";
        pageOneBeforeUpdate.Name = newPageOneName;
        pageTwoBeforeUpdate.Name = newPageTwoName;

        contextPage.UpdateAll();

        var pageOneAfterUpdate = pageRepo.GetByName(newPageOneName).SingleOrDefault();
        Assert.That(pageOneAfterUpdate, Is.Not.Null);
        Assert.That(newPageOneName, Is.EqualTo(pageOneAfterUpdate.Name));

        var pageTwoAfterUpdate = pageRepo.GetByName(newPageTwoName).SingleOrDefault();
        Assert.That(pageTwoAfterUpdate, Is.Not.Null);
        Assert.That(newPageTwoName, Is.EqualTo(pageTwoAfterUpdate.Name));
    }
}

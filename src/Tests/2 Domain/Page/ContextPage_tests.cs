using NUnit.Framework.Legacy;

namespace TrueOrFalse.Tests8._0.Pages;
internal class ContextPage_tests : BaseTest
{
    [Test]
    public void Page_should_be_persisted()
    {
        var firstPage = ContextPage.New().Add("A").Persist().All.First();
        var pageRepo = R<PageRepository>();
        var pageFromDatabase = pageRepo.GetById(firstPage.Id);
        Assert.That(firstPage, Is.Not.Null);
        Assert.That(pageFromDatabase, Is.Not.Null);
        Assert.That(pageFromDatabase?.Name, Is.EqualTo(firstPage.Name));
    }

    [Test]
    public void PagesShouldInDatabase()
    {
        var pageIds = ContextPage.New().Add(5).Persist().All.Select(c => c.Id).ToList();
        var pageRepo = R<PageRepository>();
        var idsFromDatabase = pageRepo.GetAllIds().ToList();

        CollectionAssert.AreEquivalent(pageIds, idsFromDatabase);
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

        Assert.That(pageAfterUpdate, Is.Not.Null);
        Assert.That(newPageName, Is.EqualTo(pageAfterUpdate.Name));
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
        Assert.That(pageOneAfterUpdate, Is.Not.Null);
        Assert.That(newPageOneName, Is.EqualTo(pageOneAfterUpdate.Name));

        var pageTwoAfterUpdate = pageRepo.GetByName(newPageTwoName).SingleOrDefault();
        Assert.That(pageTwoAfterUpdate, Is.Not.Null);
        Assert.That(newPageTwoName, Is.EqualTo(pageTwoAfterUpdate.Name));
    }


}

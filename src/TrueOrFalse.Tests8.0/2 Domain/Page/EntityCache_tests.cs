class EntityCache_tests : BaseTest
{
    [Test]
    public void Should_be_added_to_EntityCache()
    {
        var context = ContextPage.New(false);
        var page = new Page
        {
            Id = 15,
            Name = "Test",
            Creator = new User
            {
                Id = 2,
                Name = "Daniel"
            }
        };
        context.AddToEntityCache(page);

        var cachedPage = EntityCache.GetPage(page);

        Assert.NotNull(cachedPage);
        Assert.That(page.Id, Is.EqualTo(cachedPage.Id));
        Assert.That(page.Name, Is.EqualTo(cachedPage.Name));
        Assert.That(page.Creator.Name, Is.EqualTo(cachedPage.Creator.Name));
        Assert.That(page.Creator.Id, Is.EqualTo(cachedPage.Creator.Id));
        Assert.That(0, Is.Not.EqualTo(cachedPage.Creator.Id));
        Assert.That(0, Is.Not.EqualTo(cachedPage.Id));
        Assert.That("", Is.Not.EqualTo(cachedPage.Creator.Name));
        Assert.That("", Is.Not.EqualTo(cachedPage.Name));
    }
}
using NHibernate;

class EntityCache_tests : BaseTest
{
    [Test]
    public void Should_be_added_to_EntityCache()
    {
        var context = ContextCategory.New(false);
        var category = new Category
        {
            Id = 15,
            Name = "Test",
            Creator = new User
            {
                Id = 2,
                Name = "Daniel"
            }
        };
        context.AddToEntityCache(category);

        var cacheCategory = EntityCache.GetCategory(category);

        Assert.NotNull(cacheCategory);
        Assert.That(category.Id, Is.EqualTo(cacheCategory.Id));
        Assert.That(category.Name, Is.EqualTo(cacheCategory.Name));
        Assert.That(category.Creator.Name, Is.EqualTo(cacheCategory.Creator.Name));
        Assert.That(category.Creator.Id, Is.EqualTo(cacheCategory.Creator.Id));
        Assert.That(0, Is.Not.EqualTo(cacheCategory.Creator.Id));
        Assert.That(0, Is.Not.EqualTo(cacheCategory.Id));
        Assert.That("", Is.Not.EqualTo(cacheCategory.Creator.Name));
        Assert.That("", Is.Not.EqualTo(cacheCategory.Name));
    }
}
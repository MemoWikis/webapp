namespace TrueOrFalse.Tests8._0.Domain.Topics;
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

    [Test]
    public void TopicShouldUpdated()
    {
        var categoryName = "C1";
        var contextCategory = ContextCategory.New();
        var categoryRepo = R<CategoryRepository>();

        contextCategory.Add(categoryName).Persist();
        var categoryBeforUpdated = categoryRepo
            .GetByName(categoryName)
            .Single();
        var newCategoryName = "newC2";
        categoryBeforUpdated.Name = newCategoryName;
        contextCategory.Update(categoryBeforUpdated);

        var categoryAfterUpdate = categoryRepo.GetByName(newCategoryName).SingleOrDefault();

        Assert.IsNotNull(categoryAfterUpdate);
        Assert.AreEqual(newCategoryName, categoryAfterUpdate.Name);
    }

    [Test]
    public void TopicsShouldUpdated()
    {
        var categoryNameOne = "C1";
        var categoryNameTwo = "C2";
        var contextCategory = ContextCategory.New();
        var categoryRepo = R<CategoryRepository>();

        contextCategory.Add(categoryNameOne).Persist();
        contextCategory.Add(categoryNameTwo).Persist();
        var categoryOneBeforUpdated = contextCategory
            .All
            .Single(c => c.Name.Equals(categoryNameOne));
        var categoryTwoBeforUpdated = contextCategory
            .All
            .Single(c => c.Name.Equals(categoryNameTwo));

        var newCategoryOneName = "newC1";
        var newCategoryTwoName = "newC2";
        categoryOneBeforUpdated.Name = newCategoryOneName;
        categoryTwoBeforUpdated.Name = newCategoryTwoName;

        contextCategory.UpdateAll();

        var categoryOneAfterUpdate = categoryRepo.GetByName(newCategoryOneName).SingleOrDefault();
        Assert.IsNotNull(categoryOneAfterUpdate);
        Assert.AreEqual(newCategoryOneName, categoryOneAfterUpdate.Name);

        var categoryTwoAfterUpdate = categoryRepo.GetByName(newCategoryTwoName).SingleOrDefault();
        Assert.IsNotNull(categoryTwoAfterUpdate);
        Assert.AreEqual(newCategoryTwoName, categoryTwoAfterUpdate.Name);
    }

    [Test]
    public void TopicShouldAddToEntityCache_Test()
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
        Assert.AreEqual(cacheCategory.Id, category.Id);
        Assert.AreEqual(cacheCategory.Name, category.Name);
        Assert.AreEqual(cacheCategory.Creator.Name, category.Creator.Name);
        Assert.AreEqual(cacheCategory.Creator.Id, category.Creator.Id);
        Assert.AreNotEqual(cacheCategory.Creator.Id, 0);
        Assert.AreNotEqual(cacheCategory.Id, 0);
        Assert.AreNotEqual(cacheCategory.Creator.Name, "");
        Assert.AreNotEqual(cacheCategory.Name, "");
    }
}

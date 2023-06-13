using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;

class CategoryVisibilityTests : BaseTest
{
    [Test]
    public void ImpenetrablePrivateCategories()
    {
        //Arrange
        var context = ContextCategory.New();
        var parentA = context
            .Add("Root")
            .Persist()
            .All.First();

        var subCategories = context
            .Add("Child", parent:parentA)
            .Persist()
            .All;

        subCategories = context
            .Add("GrandChild", parent: subCategories.ByName("Child"))
            .Persist()
            .All;

        context
            .Add(subCategories.ByName("GrandChild").Name, parent: subCategories.ByName("Child"));
            
        EntityCacheInitializer.Init();

        subCategories.ByName("Child").Visibility = global::CategoryVisibility.Owner;
        subCategories.ByName("GrandChild").Visibility = global::CategoryVisibility.All;

        ContextQuestion.New().AddQuestions(15, 
            categoriesQuestions: subCategories.Where(sc => sc.Name == "GrandChild").ToList(), 
            persistImmediately: true);
            
        EntityCacheInitializer.Init();

        Assert.That(EntityCache.GetCategory(subCategories.ByName("GrandChild").Id).GetCountQuestionsAggregated(), Is.EqualTo(15));
        Assert.That(EntityCache.GetCategory(subCategories.ByName("Child").Id).GetCountQuestionsAggregated(), Is.EqualTo(15));
        Assert.That(EntityCache.GetCategory(parentA.Id).GetCountQuestionsAggregated(), Is.EqualTo(0));
    }
}
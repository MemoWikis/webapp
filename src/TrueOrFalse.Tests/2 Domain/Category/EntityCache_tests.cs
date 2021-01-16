using System.Linq;
using NUnit.Framework;
using Quartz.Util;
using TrueOrFalse.Tests;

class EntityCache_tests : BaseTest
{

    [Test]
    public void Should_get_direct_childrens()
    {
        var context = ContextCategory.New();

        var parent = context.Add("RootElement").Persist().All.First();

        var firstChildrens = context
            .Add("Sub1", parent: parent)
            .Persist()
            .All;

        var secondChildren = context.
            Add("SubSub1", parent: firstChildrens.ByName("Sub1"))
            .Persist()
            .All
            .ByName("SubSub1");

        var directChildren = EntityCache.GetChildren(parent.Id).First();
        Assert.That(directChildren.Name, Is.EqualTo("Sub1"));
    }


    [Test]
    public void Should_get_all_children()
    {
        var context = ContextCategory.New();

        var parent = context.Add("RootElement").Persist().All.First();

        var firstChildrens = context
            .Add("Sub1", parent: parent)
            .Persist()
            .All;

        context.
            Add("SubSub1", parent: firstChildrens.ByName("Sub1"))
            .Persist()
            .All
            .ByName("SubSub1");

        var allChildren = EntityCache.GetDescendants(parent.Id);
        Assert.That(allChildren.Count(c => c.Name == "Sub1"), Is.EqualTo(1));
        Assert.That(allChildren.Count(c => c.Name == "SubSub1"), Is.EqualTo(1));
    }


    [Test]
    public void Should_delete_all_child_of_relations()
    {
        ContextCategory.New().AddCaseThreeToCache();
        var allCacheCategories = EntityCache.GetAllCategories();
        var deleteCategory = allCacheCategories.ByName("E");
        var idFromDeleteCategory = deleteCategory.Id;

        Sl.CategoryRepo.Delete(deleteCategory);

        var relatedCategories = EntityCache.GetAllCategories().SelectMany(c => c.CategoryRelations.Where(cr => cr.RelatedCategory.Id == idFromDeleteCategory && cr.Category.Id == idFromDeleteCategory)).ToList();
        Assert.That(relatedCategories.Count, Is.EqualTo(0));
    }


    [Test]
    public void Should_able_to_deep_clone_cache_items()
    {
        var contexCategory = ContextCategory.New();
        var contextQuestion = ContextQuestion.New();

        var rootCategory = contexCategory.Add("root").Persist().All.First();

        var question1 = contextQuestion.AddQuestion().Persist().All.First();
        question1.Categories.Add(rootCategory);
        Sl.QuestionRepo.Update(question1);

        RecycleContainer();

        EntityCache.Init();

        EntityCache.GetAllCategories().First().DeepClone();
    }

}
using System.Linq;
using NUnit.Framework;
using TrueOrFalse.Tests;


class Automatic_inclusion_tests : BaseTest
{
    [Test]
    public void Test_Subcategory_add_correct_to_parent()
    {
        var context = ContextCategory.New();
        var parentA = context
            .Add("Category")
            .Persist()
            .All.First();

        var subCategories = ContextCategory
            .New()
            .Add("Sub1", parent: parentA)
            .Add("Sub2", parent: parentA)
            .Add("Sub3", parent: parentA)
            .Persist()
            .All;

        context.Add(subCategories.ByName("Sub1").Name, subCategories[0].Type, parent: subCategories.ByName("Sub3"));
        EntityCache.Init();
        GraphService.AutomaticInclusionOfChildCategories(subCategories.ByName("Sub1"));

        Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub1").Id).ParentCategories().Count, Is.EqualTo(2));
        Assert.That(Sl.CategoryRepo.GetById(parentA.Id).CategoryRelations.Count(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf), Is.EqualTo(3));
        Assert.That(Sl.CategoryRepo.GetById(subCategories.ByName("Sub3").Id).CategoryRelations.Count(cr => cr.CategoryRelationType == CategoryRelationType.IncludesContentOf), Is.EqualTo(1));
        Assert.That(EntityCache.GetByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));

    }
    [Test]
    public void Test_Cached_Data()
    {
        var context = ContextCategory.New();
        var parentA = context
            .Add("Category")
            .Persist()
            .All.First();

        var subCategories = context
            .Add("Sub1", parent: parentA)
            .Add("Sub2", parent: parentA)
            .Add("Sub3", parent: parentA)
            .Persist()
            .All;

        EntityCache.Init();
        Assert.That(EntityCache.GetByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));

        Sl.CategoryRepo.Delete(context.All.ByName("Sub1"));
        Assert.That(EntityCache.GetByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(2));
        context.Add("Sub1", parent: parentA).Persist();
        Assert.That(EntityCache.GetByName("Category").First().CachedData.ChildrenIds.Count, Is.EqualTo(3));


    }
}


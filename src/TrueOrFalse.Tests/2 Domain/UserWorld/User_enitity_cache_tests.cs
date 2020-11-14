using System.Linq;
using System.Threading;
using NUnit.Framework;
using TrueOrFalse.Tests;

class User_entity_cache_tests : BaseTest
{
    [Test]
    public void Should_return_correct_categories()
    {
        ContextCategory.New().AddCaseThreeToCache();
        var user = Sl.SessionUser.User;
       
       UserEntityCache.Init();
        var userEntityCacheCategories = UserEntityCache.GetCategories(user.Id).Values.ToList();
        var entityCacheCategories = EntityCache.GetAllCategories().ToList();

        // entityCacheCategories is uncut case and userEntityCacheCategoriess is cut case  https://app.diagrams.net/#G1CEMMm1iIhfNKvuKng5oM6erR0bVDWHr6

        //EntityCache
        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "E").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "G").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("H").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("G").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("F").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("E").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "C").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("D").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "B").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("B").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("C").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("C").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X2").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("C").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X1").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X2").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X1").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X1").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(entityCacheCategories.ByName("X3").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));

        //userEntityCache
        Assert.That(userEntityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "G").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("I").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("G").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("G").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("F").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("F").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "X3").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("X").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("X3").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
        Assert.That(userEntityCacheCategories.ByName("B").CategoryRelations.Where(cr => cr.RelatedCategory.Name == "A").Count, Is.EqualTo(1));
    }

    [Test]
    public void Give_correct_number_of_cache_items()
    {
        ContextCategory.New().AddCaseThreeToCache();
        ContextCategory.New(false).AddCaseTwoToCache();
        Thread.Sleep(100);

        Assert.That(UserEntityCache.GetCategories(2).Values.ToList().Count, Is.EqualTo(6));
        Assert.That(UserEntityCache.GetCategories(3).Values.ToList().Count, Is.EqualTo(4));
    }
}


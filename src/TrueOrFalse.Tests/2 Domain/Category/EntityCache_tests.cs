using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NUnit.Framework;
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

        var questions = Sl.QuestionRepo.GetAll();
        var categories = Sl.CategoryRepo.GetAllEager();

        ObjectExtensions.DeepClone(EntityCache.GetAllCategories().First());
        FluentNHibernate.Utils.Extensions.DeepClone(EntityCache.GetAllCategories().First());  
    }

    [Test]
    [Ignore("Not done yet. The goal of the test is to explorer methods for nhibernate introspection ")]
    public void Entity_in_cache_should_be_detached_from_NHibernate_session()
    {
        var contexCategory = ContextCategory.New();
        var contextQuestion = ContextQuestion.New();

        var rootCategory = contexCategory.Add("root").Persist().All.First();

        var question1 = contextQuestion.AddQuestion().Persist().All.First();
        question1.Categories.Add(rootCategory);
        Sl.QuestionRepo.Update(question1);

        //((PersistentGenericBag<Category>)question1.Categories).session

        var session = typeof(PersistentGenericBag<Category>).GetField("session", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(question1.Categories);
        var session2 = question1.Categories.GetFieldValue<object>("session");

        var persistentGenericBag = ((PersistentGenericBag<Category>) question1.Categories);
        var session3 = persistentGenericBag.GetFieldValue<ISessionImplementor>("session");

        Assert.IsTrue(NHibernateUtil.IsInitialized(question1.Categories));


        Sl.QuestionRepo.Session.Evict(question1.Categories);

        RecycleContainer();

        Assert.IsTrue(NHibernateUtil.IsInitialized(question1.Categories));
    }
    [Test]
    public void Should_have_correct_children()
    {
        var context = ContextCategory.New(); 
        context.AddCaseThreeToCache();
        var categories = context.All;

        EntityCache.Init();
        
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), "X3"), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), "X2"), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), "X1"), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), "X"), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), "B"), Is.EqualTo(true));
        Assert.That(EntityCache.GetCategory(categories.ByName("A").Id).CachedData.Children.Count, Is.EqualTo(5));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X3").Id), "X1"), Is.EqualTo(true));
        Assert.That(EntityCache.GetCategory(categories.ByName("X3").Id).CachedData.Children.Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X1").Id), "C"), Is.EqualTo(true));
        Assert.That(EntityCache.GetCategory(categories.ByName("X1").Id).CachedData.Children.Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X2").Id), "C"), Is.EqualTo(true));
        Assert.That(EntityCache.GetCategory(categories.ByName("X2").Id).CachedData.Children.Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X").Id), "C"), Is.EqualTo(true));
        Assert.That(EntityCache.GetCategory(categories.ByName("X").Id).CachedData.Children.Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), "E"), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), "F"), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), "G"), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), "H"), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), "I"), Is.EqualTo(true));
        Assert.That(EntityCache.GetCategory(categories.ByName("A").Id).CachedData.Children.Count, Is.EqualTo(5));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("B").Id), "D"), Is.EqualTo(true));
        Assert.That(EntityCache.GetCategory(categories.ByName("X").Id).CachedData.Children.Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("E").Id), "I"), Is.EqualTo(true));
        Assert.That(EntityCache.GetCategory(categories.ByName("X").Id).CachedData.Children.Count, Is.EqualTo(1));


        Assert.That(EntityCache.GetCategory(categories.ByName("D").Id).CachedData.Children.Count, Is.EqualTo(0));
        Assert.That(EntityCache.GetCategory(categories.ByName("F").Id).CachedData.Children.Count, Is.EqualTo(0));
        Assert.That(EntityCache.GetCategory(categories.ByName("H").Id).CachedData.Children.Count, Is.EqualTo(0));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("G").Id), "I"), Is.EqualTo(true));
        Assert.That(EntityCache.GetCategory(categories.ByName("X").Id).CachedData.Children.Count, Is.EqualTo(1));


    }
}

public static class ReflectionExtensions
{
    public static T GetFieldValue<T>(this object obj, string name)
    {
        // Set the flags so that private and public fields from instances will be found
        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var field = obj.GetType().GetField(name, bindingFlags);
        return (T)field?.GetValue(obj);
    }
}
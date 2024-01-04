using System.Linq;
using System.Reflection;
using Autofac;
using NHibernate;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NUnit.Framework;
using TrueOrFalse.Tests;

class EntityCache_tests : BaseTest
{

    [Test, Sequential]
    public void Should_get_direct_childrens()
    {
        var context = ContextCategory.New();

        var parent = context.Add("RootElement").Persist().All.First();

        var firstChildren = context
            .Add("Sub1", parent: parent)
            .Persist()
            .All;

        var secondChildren = context.
            Add("SubSub1", parent: firstChildren.ByName("Sub1"))
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

        var allChildren = EntityCache.GetAllChildren(parent.Id);
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
        var catRepo = LifetimeScope.Resolve<CategoryRepository>(); 

        catRepo.Delete( catRepo.GetByIdEager(deleteCategory));

        var relatedCategories = EntityCache.GetAllCategories().SelectMany(c => c.CategoryRelations.Where(cr => cr.RelatedCategoryId == idFromDeleteCategory && cr.CategoryId == idFromDeleteCategory)).ToList();
        Assert.That(relatedCategories.Count, Is.EqualTo(0));
    }

    [Test]
    public void Should_able_to_deep_clone_cache_items()
    {
        var contexCategory = ContextCategory.New();
        var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(), 
            R<AnswerRepo>(), 
            R<AnswerQuestion>(), 
            R<UserWritingRepo>(), 
            R<CategoryRepository>());

        var rootCategory = contexCategory.Add("root").Persist().All.First();

        var question1 = contextQuestion.AddQuestion().Persist().All.First();
        question1.Categories.Add(rootCategory);

       
        R<QuestionWritingRepo>().UpdateOrMerge(question1, false);

        RecycleContainer();

        Resolve<EntityCacheInitializer>().Init();

        var questions = R<QuestionReadingRepo>().GetAll();
        var categories = LifetimeScope.Resolve<CategoryRepository>().GetAllEager();

        ObjectExtensions.DeepClone(EntityCache.GetAllCategories().First());
        FluentNHibernate.Utils.Extensions.DeepClone(EntityCache.GetAllCategories().First());
    }

    [Test]
    [Ignore("Not done yet. The goal of the test is to explorer methods for nhibernate introspection ")]
    public void Entity_in_cache_should_be_detached_from_NHibernate_session()
    {
        var contexCategory = ContextCategory.New();
        var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(),
            R<AnswerRepo>(),
            R<AnswerQuestion>(),
            R<UserWritingRepo>(),
            R<CategoryRepository>()); 

        var rootCategory = contexCategory.Add("root").Persist().All.First();

        var question1 = contextQuestion.AddQuestion().Persist().All.First();
        question1.Categories.Add(rootCategory);
        R<QuestionWritingRepo>().UpdateOrMerge(question1, false);

        Assert.IsTrue(NHibernateUtil.IsInitialized(question1.Categories));

        R<ISession>().Evict(question1.Categories);

        RecycleContainer();

        Assert.IsTrue(NHibernateUtil.IsInitialized(question1.Categories));
    }

    [Test]
    public void Should_have_correct_children()
    {
        var context = ContextCategory.New(); 
        context.AddCaseThreeToCache();
        var categories = context.All;

        Resolve<EntityCacheInitializer>().Init();
        
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id),GetCategoryId("X3", context) ), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), GetCategoryId("X2", context)), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), GetCategoryId("X1", context)), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), GetCategoryId("X", context)), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), GetCategoryId("B", context)), Is.EqualTo(true));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("A").Id).Count, Is.EqualTo(5));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X3").Id), GetCategoryId("X1", context)), Is.EqualTo(true));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("X3").Id).Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X1").Id), GetCategoryId("C", context)), Is.EqualTo(true));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("X1").Id).Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X2").Id), GetCategoryId("C", context)), Is.EqualTo(true));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("X2").Id).Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X").Id), GetCategoryId("C", context)), Is.EqualTo(true));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("X").Id).Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("E", context)), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("F", context)), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("G", context)), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("H", context)), Is.EqualTo(true));
        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("I", context)), Is.EqualTo(true));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("A").Id).Count, Is.EqualTo(5));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("B").Id), GetCategoryId("D", context)), Is.EqualTo(true));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("X").Id).Count, Is.EqualTo(1));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("E").Id), GetCategoryId("I", context)), Is.EqualTo(true));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("X").Id).Count, Is.EqualTo(1));


        Assert.That(EntityCache.GetAllChildren(categories.ByName("D").Id).Count, Is.EqualTo(0));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("F").Id).Count, Is.EqualTo(0));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("H").Id).Count, Is.EqualTo(0));

        Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("G").Id), GetCategoryId("I", context)), Is.EqualTo(true));
        Assert.That(EntityCache.GetAllChildren(categories.ByName("X").Id).Count, Is.EqualTo(1));
    }

    private int GetCategoryId(string topicName, ContextCategory categoryContext)
    {
        return categoryContext.All.Single(c => c.Name.Equals(topicName)).Id; 
    }
}

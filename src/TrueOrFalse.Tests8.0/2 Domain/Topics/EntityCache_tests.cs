﻿
using NHibernate;

class EntityCache_tests : BaseTest
{
    [Test]
    public void GetAll()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();
        var child = context
            .Add("Sub1")
            .Persist()
            .All.Last();

        RecycleContainer();

        var categoryRepo = R<CategoryRepository>();
        var categories = categoryRepo.GetAll();
        Assert.IsNotNull(categories);
    }

    [Test]
    public void GetAll2()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();
        var child = context
            .Add("Sub1")
            .Persist()
            .All.Last();

        context.AddChild(root, child);

        RecycleContainer();

        var categoryRepo = R<CategoryRepository>();
        var categories = categoryRepo.GetAll();
        Assert.IsNotNull(categories);
    }

    [Test]
    public void GetAll3()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();
        var child = context
            .Add("Sub1")
            .Persist()
            .All.Last();

        context.AddChild(root, child);

        RecycleContainer();

        var initializer = Resolve<EntityCacheInitializer>();
        initializer.Init(" (started in entitycache_tests) ");

        var directChildren = EntityCache.GetChildren(root.Id).First();
        Assert.That(directChildren.Name, Is.EqualTo("Sub1"));
    }

    [Test, Sequential]
    public async Task Should_get_direct_children()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Persist()
            .All;

        //context.AddChild(root, children.ByName("Sub1"));
        //context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));

        var categoryRepo = R<CategoryRepository>();
        var relationModifier = new ModifyRelationsForCategory(R<CategoryRepository>());

        var sub1 = children.ByName("Sub1");
        relationModifier.AddParentCategory(sub1, root.Id);

        categoryRepo.Update(sub1, 2, type: CategoryChangeType.Relations);
        categoryRepo.Update(root, 2, type: CategoryChangeType.Relations);

        var subsub1 = children.ByName("SubSub1");

        relationModifier.AddParentCategory(subsub1, sub1.Id);

        categoryRepo.Update(subsub1, 2, type: CategoryChangeType.Relations);
        categoryRepo.Update(sub1, 2, type: CategoryChangeType.Relations);

        RecycleContainer();

        var initializer = Resolve<EntityCacheInitializer>();
        initializer.Init(" (started in entitycache_tests) ");

        var directChildren = EntityCache.GetChildren(root.Id).First();
        Assert.That(directChildren.Name, Is.EqualTo("Sub1"));
    }

    [Test]
    public void Should_get_direct_visible_children()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility:CategoryVisibility.Owner)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));

        context.AddChild(root, children.ByName("Sub2"));

        //RecycleContainer();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        //context.AddToEntityCache(root);
        //context.AddToEntityCache(children.ByName("Sub1"));
        //context.AddToEntityCache(children.ByName("SubSub1"));
        //context.AddToEntityCache(children.ByName("Sub2"));

        var defaultUserId = -1;
        var permissionCheck = new PermissionCheck(defaultUserId);

        var directChildren = EntityCache.GetVisibleChildren(root.Id, permissionCheck, defaultUserId).First();
        Assert.That(directChildren.Name, Is.EqualTo("Sub1"));
    }


    //[Test]
    //public void Should_get_all_children()
    //{
    //    var context = ContextCategory.New();

    //    var parent = context.Add("RootElement").Persist().All.First();

    //    var firstChildrens = context
    //        .Add("Sub1", parent: parent)
    //        .Persist()
    //        .All;

    //    context.
    //        Add("SubSub1", parent: firstChildrens.ByName("Sub1"))
    //        .Persist()
    //        .All
    //        .ByName("SubSub1");

    //    var allChildren = EntityCache.GetAllChildren(parent.Id);
    //    Assert.That(allChildren.Count(c => c.Name == "Sub1"), Is.EqualTo(1));
    //    Assert.That(allChildren.Count(c => c.Name == "SubSub1"), Is.EqualTo(1));
    //}


    //[Test]
    //public void Should_delete_all_child_of_relations()
    //{
    //    ContextCategory.New().AddCaseThreeToCache();
    //    var allCacheCategories = EntityCache.GetAllCategories();
    //    var deleteCategory = allCacheCategories.ByName("E");
    //    var idFromDeleteCategory = deleteCategory.Id;
    //    var catRepo = LifetimeScope.Resolve<CategoryRepository>(); 

    //    catRepo.Delete( catRepo.GetByIdEager(deleteCategory));

    //    var relatedCategories = EntityCache.GetAllCategories().SelectMany(c => c.CategoryRelations.Where(cr => cr.RelatedCategoryId == idFromDeleteCategory && cr.CategoryId == idFromDeleteCategory)).ToList();
    //    Assert.That(relatedCategories.Count, Is.EqualTo(0));
    //}

    //[Test]
    //public void Should_able_to_deep_clone_cache_items()
    //{
    //    var contexCategory = ContextCategory.New();
    //    var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(), 
    //        R<AnswerRepo>(), 
    //        R<AnswerQuestion>(), 
    //        R<UserWritingRepo>(), 
    //        R<CategoryRepository>());

    //    var rootCategory = contexCategory.Add("root").Persist().All.First();

    //    var question1 = contextQuestion.AddQuestion().Persist().All.First();
    //    question1.Categories.Add(rootCategory);


    //    R<QuestionWritingRepo>().UpdateOrMerge(question1, false);

    //    RecycleContainer();

    //    Resolve<EntityCacheInitializer>().Init();

    //    var questions = R<QuestionReadingRepo>().GetAll();
    //    var categories = LifetimeScope.Resolve<CategoryRepository>().GetAllEager();

    //    ObjectExtensions.DeepClone(EntityCache.GetAllCategories().First());
    //    FluentNHibernate.Utils.Extensions.DeepClone(EntityCache.GetAllCategories().First());
    //}

    //[Test]
    //[Ignore("Not done yet. The goal of the test is to explorer methods for nhibernate introspection ")]
    //public void Entity_in_cache_should_be_detached_from_NHibernate_session()
    //{
    //    var contexCategory = ContextCategory.New();
    //    var contextQuestion = ContextQuestion.New(R<QuestionWritingRepo>(),
    //        R<AnswerRepo>(),
    //        R<AnswerQuestion>(),
    //        R<UserWritingRepo>(),
    //        R<CategoryRepository>()); 

    //    var rootCategory = contexCategory.Add("root").Persist().All.First();

    //    var question1 = contextQuestion.AddQuestion().Persist().All.First();
    //    question1.Categories.Add(rootCategory);
    //    R<QuestionWritingRepo>().UpdateOrMerge(question1, false);

    //    Assert.IsTrue(NHibernateUtil.IsInitialized(question1.Categories));

    //    R<ISession>().Evict(question1.Categories);

    //    RecycleContainer();

    //    Assert.IsTrue(NHibernateUtil.IsInitialized(question1.Categories));
    //}

    //[Test]
    //public void Should_have_correct_children()
    //{
    //    var context = ContextCategory.New(); 
    //    context.AddCaseThreeToCache();
    //    var categories = context.All;

    //    Resolve<EntityCacheInitializer>().Init();

    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id),GetCategoryId("X3", context) ), Is.EqualTo(true));
    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), GetCategoryId("X2", context)), Is.EqualTo(true));
    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), GetCategoryId("X1", context)), Is.EqualTo(true));
    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), GetCategoryId("X", context)), Is.EqualTo(true));
    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("A").Id), GetCategoryId("B", context)), Is.EqualTo(true));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("A").Id).Count, Is.EqualTo(5));

    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X3").Id), GetCategoryId("X1", context)), Is.EqualTo(true));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("X3").Id).Count, Is.EqualTo(1));

    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X1").Id), GetCategoryId("C", context)), Is.EqualTo(true));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("X1").Id).Count, Is.EqualTo(1));

    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X2").Id), GetCategoryId("C", context)), Is.EqualTo(true));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("X2").Id).Count, Is.EqualTo(1));

    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("X").Id), GetCategoryId("C", context)), Is.EqualTo(true));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("X").Id).Count, Is.EqualTo(1));

    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("E", context)), Is.EqualTo(true));
    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("F", context)), Is.EqualTo(true));
    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("G", context)), Is.EqualTo(true));
    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("H", context)), Is.EqualTo(true));
    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("C").Id), GetCategoryId("I", context)), Is.EqualTo(true));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("A").Id).Count, Is.EqualTo(5));

    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("B").Id), GetCategoryId("D", context)), Is.EqualTo(true));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("X").Id).Count, Is.EqualTo(1));

    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("E").Id), GetCategoryId("I", context)), Is.EqualTo(true));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("X").Id).Count, Is.EqualTo(1));


    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("D").Id).Count, Is.EqualTo(0));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("F").Id).Count, Is.EqualTo(0));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("H").Id).Count, Is.EqualTo(0));

    //    Assert.That(ContextCategory.HasCorrectChild(EntityCache.GetCategory(categories.ByName("G").Id), GetCategoryId("I", context)), Is.EqualTo(true));
    //    Assert.That(EntityCache.GetAllChildren(categories.ByName("X").Id).Count, Is.EqualTo(1));
    //}

    //private int GetCategoryId(string topicName, ContextCategory categoryContext)
    //{
    //    return categoryContext.All.Single(c => c.Name.Equals(topicName)).Id; 
    //}
}


using NHibernate;

class EntityCache_tests : BaseTest
{
    [Test]
    public void Should_get_direct_children()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: CategoryVisibility.Owner)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));
        context.AddChild(root, children.ByName("Sub2"));

        RecycleContainerAndEntityCache();

        var directChildren = EntityCache.GetChildren(root.Id);
        Assert.That(directChildren.Count, Is.EqualTo(2));
        Assert.That(directChildren.First().Name, Is.EqualTo("Sub1"));
        Assert.That(directChildren.Last().Name, Is.EqualTo("Sub2"));
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

        RecycleContainerAndEntityCache();

        var defaultUserId = -1;
        var permissionCheck = new PermissionCheck(defaultUserId);

        var directChildren = EntityCache.GetVisibleChildren(root.Id, permissionCheck, defaultUserId).First();
        Assert.That(directChildren.Name, Is.EqualTo("Sub1"));
    }

    [Test]
    public void Should_get_all_children()
    {
        var context = ContextCategory.New();

        var root = context.Add("RootElement").Persist().All.First();

        var children = context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: CategoryVisibility.Owner)
            .Persist()
            .All;

        context.AddChild(root, children.ByName("Sub1"));
        context.AddChild(children.ByName("Sub1"), children.ByName("SubSub1"));
        context.AddChild(root, children.ByName("Sub2"));

        RecycleContainerAndEntityCache();

        var directChildren = EntityCache.GetAllChildren(root.Id);

        Assert.That(directChildren.Count, Is.EqualTo(3));
        Assert.That(directChildren.Select(i => i.Name), Does.Contain("Sub1"));
        Assert.That(directChildren.Select(i => i.Name), Does.Contain("Sub2"));
        Assert.That(directChildren.Select(i => i.Name), Does.Contain("SubSub1"));
    }

    [Test]
    public void Should_get_all_visible_children()
    {
        var context = ContextCategory.New();

        context.Add("RootElement").Add("RootElement2").Persist();

        var root = context.All.ByName("RootElement");
        var root2 = context.All.ByName("RootElement2");

        context
            .Add("Sub1")
            .Add("SubSub1")
            .Add("Sub2", visibility: CategoryVisibility.Owner)
            .Add("SubSub2", visibility: CategoryVisibility.Owner)
            .Add("SubSub1and2")
            .Add("Sub3", visibility: CategoryVisibility.Owner)
            .Add("SubSub3")
            .Persist();

        context.AddChild(root, context.All.ByName("Sub1"));
        context.AddChild(context.All.ByName("Sub1"), context.All.ByName("SubSub1"));

        context.AddChild(root, context.All.ByName("Sub2"));
        context.AddChild(context.All.ByName("Sub2"), context.All.ByName("SubSub2")); 
        
        context.AddChild(context.All.ByName("Sub1"), context.All.ByName("SubSub1and2"));
        context.AddChild(context.All.ByName("Sub2"), context.All.ByName("SubSub1and2"));

        context.AddChild(root, context.All.ByName("Sub3"));
        context.AddChild(context.All.ByName("Sub3"), context.All.ByName("SubSub3"));
        context.AddChild(root2, context.All.ByName("SubSub3"));

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var defaultUserId = -1;
        var permissionCheck = new PermissionCheck(defaultUserId);

        var allChildren = EntityCache.GetAllVisibleChildren(root.Id, permissionCheck, defaultUserId);
        Assert.That(allChildren.Count, Is.EqualTo(3));
        Assert.That(allChildren.Last().Name, Is.EqualTo("SubSub1and2"));
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



using System.Diagnostics;

class Relation_tests : BaseTest
{
    [Test]
    public void Should_Add_Creation_ToDb_and_EntityCache()
    {
        var context = ContextCategory.New();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub2")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");

        context.AddChild(root, sub1);
        context.AddChild(root, sub2);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetCategory(root);
        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(2));
        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.EqualTo(null));

        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(null));
    }

    //Move sub1 after sub3
    [Test]
    public void Should_MoveRelation_Correctly_AfterSub3()
    {
        var context = ContextCategory.New();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub2")
            .Add("sub3")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");
        var sub3 = context.All.ByName("sub3");

        context.AddChild(root, sub1);
        context.AddChild(root, sub2);
        context.AddChild(root, sub3);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetCategory(root);
        var relationToMove = cachedRoot.ChildRelations[0];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);
        ModifyRelationsEntityCache.MoveAfter(relationToMove, sub3.Id, cachedRoot.Id, 1, modifyRelationsForCategory);

        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(3));

        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.EqualTo(null));
        Assert.That(cachedRoot.ChildRelations[0].ChildId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub3.Id));

        Assert.That(cachedRoot.ChildRelations[1].ChildId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[2].ChildId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[2].PreviousId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[2].NextId, Is.EqualTo(null));

        Task.Delay(200).Wait();

        var allRelationsInDb = categoryRelationRepo.GetAll();

        Assert.That(allRelationsInDb.Count, Is.EqualTo(3));

        var firstCachedId = cachedRoot.ChildRelations[0].Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[0].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[0].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[0].NextId));

        Assert.That(allRelationsInDb[2].Child.Id, Is.EqualTo(cachedRoot.ChildRelations[2].ChildId));
        Assert.That(allRelationsInDb[2].PreviousId, Is.EqualTo(cachedRoot.ChildRelations[2].PreviousId));
        Assert.That(allRelationsInDb[2].NextId, Is.EqualTo(cachedRoot.ChildRelations[2].NextId));
    }

    //Move sub3 before sub1
    [Test]
    public void Should_MoveRelation_Correctly_BeforeSub1()
    {
        var context = ContextCategory.New();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub2")
            .Add("sub3")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");
        var sub3 = context.All.ByName("sub3");

        context.AddChild(root, sub1);
        context.AddChild(root, sub2);
        context.AddChild(root, sub3);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetCategory(root);
        var relationToMove = cachedRoot.ChildRelations[2];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);
        ModifyRelationsEntityCache.MoveBefore(relationToMove, sub1.Id, cachedRoot.Id, 1, modifyRelationsForCategory);

        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(3));

        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.EqualTo(null));
        Assert.That(cachedRoot.ChildRelations[0].ChildId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[1].ChildId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(sub2.Id));

        Assert.That(cachedRoot.ChildRelations[2].ChildId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[2].PreviousId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[2].NextId, Is.EqualTo(null));

        Task.Delay(200).Wait();

        var allRelationsInDb = categoryRelationRepo.GetAll();

        Assert.That(allRelationsInDb.Count, Is.EqualTo(3));

        var firstCachedId = cachedRoot.ChildRelations[0].Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[0].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[0].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[0].NextId));

        var lastCachedId = cachedRoot.ChildRelations.LastOrDefault()?.Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[2].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[2].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[2].NextId));
    }

    //Move sub1 after sub3 and before sub4
    [Test]
    public void Should_MoveRelation_Correctly_AfterSub3_BeforeSub4()
    {
        var context = ContextCategory.New();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub2")
            .Add("sub3")
            .Add("sub4")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub2 = context.All.ByName("sub2");
        var sub3 = context.All.ByName("sub3");
        var sub4 = context.All.ByName("sub4");

        context.AddChild(root, sub1);
        context.AddChild(root, sub2);
        context.AddChild(root, sub3);
        context.AddChild(root, sub4);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetCategory(root);
        var relationToMove = cachedRoot.ChildRelations[0];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);
        ModifyRelationsEntityCache.MoveAfter(relationToMove, sub3.Id, cachedRoot.Id, 1, modifyRelationsForCategory);

        Assert.That(cachedRoot.ChildRelations.Count, Is.EqualTo(4));

        Assert.That(cachedRoot.ChildRelations[0].PreviousId, Is.EqualTo(null));
        Assert.That(cachedRoot.ChildRelations[0].ChildId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[0].NextId, Is.EqualTo(sub3.Id));

        Assert.That(cachedRoot.ChildRelations[1].ChildId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[1].PreviousId, Is.EqualTo(sub2.Id));
        Assert.That(cachedRoot.ChildRelations[1].NextId, Is.EqualTo(sub1.Id));

        Assert.That(cachedRoot.ChildRelations[2].ChildId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[2].PreviousId, Is.EqualTo(sub3.Id));
        Assert.That(cachedRoot.ChildRelations[2].NextId, Is.EqualTo(sub4.Id));

        Assert.That(cachedRoot.ChildRelations[3].ChildId, Is.EqualTo(sub4.Id));
        Assert.That(cachedRoot.ChildRelations[3].PreviousId, Is.EqualTo(sub1.Id));
        Assert.That(cachedRoot.ChildRelations[3].NextId, Is.EqualTo(null));

        Task.Delay(200).Wait();

        var allRelationsInDb = categoryRelationRepo.GetAll();

        Assert.That(allRelationsInDb.Count, Is.EqualTo(4));

        var firstCachedId = cachedRoot.ChildRelations[0].Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[0].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[0].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == firstCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[0].NextId));

        var lastCachedId = cachedRoot.ChildRelations.LastOrDefault()?.Id;

        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.Child.Id, Is.EqualTo(cachedRoot.ChildRelations[3].ChildId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.PreviousId, Is.EqualTo(cachedRoot.ChildRelations[3].PreviousId));
        Assert.That(allRelationsInDb.FirstOrDefault(r => r.Id == lastCachedId)?.NextId, Is.EqualTo(cachedRoot.ChildRelations[3].NextId));
    }

    [Test]
    public void Should_Fail_Move_CircularReference()
    {
        var context = ContextCategory.New();

        context.Add("root").Persist();

        context
            .Add("sub1")
            .Add("sub1sub1")
            .Add("sub1sub1sub1")
            .Persist();

        var root = context.All.ByName("root");
        var sub1 = context.All.ByName("sub1");
        var sub1sub1 = context.All.ByName("sub1sub1");
        var sub1sub1sub1 = context.All.ByName("sub1sub1sub1");

        context.AddChild(root, sub1);
        context.AddChild(sub1, sub1sub1);
        context.AddChild(sub1sub1, sub1sub1sub1);

        RecycleContainerAndEntityCache();

        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init();

        var cachedRoot = EntityCache.GetCategory(root);
        var relationToMove = cachedRoot.ChildRelations[0];
        var categoryRelationRepo = R<CategoryRelationRepo>();
        var modifyRelationsForCategory = new ModifyRelationsForCategory(R<CategoryRepository>(), categoryRelationRepo);

        var ex = Assert.Throws<Exception>(() => ModifyRelationsEntityCache.MoveAfter(relationToMove, sub1sub1sub1.Id, sub1sub1.Id, 1, modifyRelationsForCategory));
        Assert.That(ex.Message, Is.EqualTo("circular reference"));

        var ex2 = Assert.Throws<Exception>(() => ModifyRelationsEntityCache.MoveAfter(relationToMove, sub1sub1.Id, sub1.Id, 1, modifyRelationsForCategory));
        Assert.That(ex2.Message, Is.EqualTo("circular reference"));
    }

}
